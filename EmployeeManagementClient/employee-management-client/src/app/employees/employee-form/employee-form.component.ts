import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Inject, Input, OnChanges, Output, SimpleChanges } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatNativeDateModule } from '@angular/material/core';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { EmployeeDto, EmployeeService } from '../../core/employee.service';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';
import { EmployeeDataService } from '../common/employee.data.service';

@Component({
  selector: 'app-employee-form',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatDatepickerModule,
    MatSelectModule,
    MatNativeDateModule,
    RouterModule
  ],
  styleUrl: './employee-form.component.css',
  templateUrl: './employee-form.component.html',
})
export class EmployeeFormComponent implements OnChanges {

  employeeForm!: FormGroup;

  employee: any;
  allEmployees: EmployeeDto[] = [];
  employeesToLink: EmployeeDto[] = [];
  currentEmployeeId!: string;
  allRoles = ['Employee', 'Leader', 'Director'];
  roles: string[] = [];
  roleHierarchy = {
    Director: 3,
    Leader: 2,
    Employee: 1
  };



  constructor(private fb: FormBuilder,
    private employeeService: EmployeeService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private employeeDataService: EmployeeDataService,

  ) {

    const nav = this.router.getCurrentNavigation();
    this.employee = nav?.extras?.state?.['employee'];
    this.currentEmployeeId = this.route.snapshot.paramMap.get('id')!;
    this.allEmployees = this.employeeDataService.employees;
    this.roles = this.getAvailableRoles();
    if (this.employee !== null) {
      this.employeeForm = this.fb.group({
        firstName: [this.employee?.firstName, Validators.required],
        lastName: [this.employee?.lastName, Validators.required],
        email: [this.employee?.email, [Validators.required, Validators.email]],
        documentNumber: [this.employee?.documentNumber, Validators.required],
        phoneNumbers: this.fb.array([]),
        managerId: [this.employee?.managerId],
        password: [this.employee?.password, [Validators.required, Validators.minLength(6)]],
        birthDate: [this.employee?.birthDate, Validators.required],
        role: [this.employee?.role, Validators.required]
      });

      const phoneArray = this.employeeForm.get('phoneNumbers') as FormArray;

      if (this.employee?.phoneNumbers?.length) {
        this.employee.phoneNumbers.forEach((phone: any) => {
          phoneArray.push(this.fb.control(phone, Validators.required));
        });
      } else {
        phoneArray.push(this.fb.control('', Validators.required));
      }
    }
    else {
      this.employeeForm = this.fb.group({
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        documentNumber: ['', Validators.required],
        phoneNumbers: this.fb.array([
          this.fb.control('', Validators.required)
        ]),
        managerId: [null],
        password: ['', [Validators.required, Validators.minLength(6)]],
        birthDate: ['', Validators.required],
        role: ['', Validators.required]
      });
    }
  }
    ngOnInit(): void {
    this.employeeService.getAllToLink(this.currentEmployeeId).subscribe({
      next: (data) => (
            this.employeesToLink = data
      ),
      error: (err) => console.error('Error fetching employees', err),
    });
  }
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['employee'] && this.employee) {
      this.employeeForm.patchValue(this.employee);
    }
  }

  get phoneNumbers(): FormArray {
    return this.employeeForm.get('phoneNumbers') as FormArray;
  }

  getAvailableRoles(): string[] {
    const employeeLoggedIn = this.allEmployees.find(x => x.id === this.currentEmployeeId);
    const currentLevel = this.roleHierarchy[employeeLoggedIn?.role as keyof typeof this.roleHierarchy];
    const roles = this.allRoles.filter(role => this.roleHierarchy[role as keyof typeof this.roleHierarchy] <= currentLevel);
    return roles;
  }
  addPhoneNumber() {
    this.phoneNumbers.push(this.fb.control('', Validators.required));
  }

  removePhoneNumber(index: number) {
    this.phoneNumbers.removeAt(index);
  }

  onSubmit() {
    if (this.employeeForm.valid) {
      if (this.employee) {
        const updatedEmployee: EmployeeDto = this.employeeForm.value;
        updatedEmployee.id = this.employee.id;
        this.employeeService.update(updatedEmployee).subscribe({
          next: () => {
            console.log('Employee created');
            this.router.navigate(['/employees']); // Go back to list
          },
          error: (err) => {
            console.error('Error saving employee', err);

            this.snackBar.open(err.error.errorMessage, 'Close', {
              duration: 5000,
              panelClass: 'snackbar-error'
            });
          }
        });
      }
      else {
        const newEmployee: EmployeeDto = this.employeeForm.value;
        console.log('Submitted:', newEmployee);
        this.employeeService.add(this.currentEmployeeId, newEmployee).subscribe({
          next: () => {
            console.log('Employee created');
            this.router.navigate(['/employees']); // Go back to list
          },
          error: (err) => {
            console.error('Error saving employee', err);

            this.snackBar.open(err.error.errorMessage, 'Close', {
              duration: 5000,
              panelClass: 'snackbar-error'
            });
          }
        });
      }

    }
  }
}
