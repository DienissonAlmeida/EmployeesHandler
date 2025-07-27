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
import { Router } from '@angular/router';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

// @Component({
//   selector: 'app-employee-form',
//   imports: [],
//   templateUrl: './employee-form.component.html',
//   styleUrl: './employee-form.component.css'
// })

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
    MatNativeDateModule
  ],
  styleUrl: './employee-form.component.css',
  templateUrl: './employee-form.component.html',
})
export class EmployeeFormComponent implements OnChanges {

  employeeForm!: FormGroup;

  employee: any;
  roles = ['Employee', 'Leader', 'Director'];

  constructor(private fb: FormBuilder,
    private employeeService: EmployeeService,
    private router: Router,
  ) {

    const nav = this.router.getCurrentNavigation();
    this.employee = nav?.extras?.state?.['employee'];

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
        // Add one empty field by default if no numbers
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
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['employee'] && this.employee) {
      this.employeeForm.patchValue(this.employee);
    }
  }

  get phoneNumbers(): FormArray {
    return this.employeeForm.get('phoneNumbers') as FormArray;
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
          error: (err) => console.error('Error saving employee', err),
        });
      }
      else {
        const newEmployee: EmployeeDto = this.employeeForm.value;
        console.log('Submitted:', newEmployee);
        this.employeeService.add(newEmployee).subscribe({
          next: () => {
            console.log('Employee created');
            this.router.navigate(['/employees']); // Go back to list
          },
          error: (err) => console.error('Error saving employee', err),
        });
      }

    }
  }
}
