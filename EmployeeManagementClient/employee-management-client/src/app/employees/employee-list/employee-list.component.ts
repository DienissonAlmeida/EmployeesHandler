import { Component } from '@angular/core';
import { EmployeeDto, EmployeeService } from '../../core/employee.service';
import { CommonModule } from '@angular/common';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator'; // optional
import { MatSortModule } from '@angular/material/sort'; // optional
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { Router, RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../core/auth.service';
import { EmployeeDataService } from '../common/employee.data.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-employee-list',
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatToolbarModule,
    MatButtonModule,
    RouterModule,
    MatIconModule
  ],

  templateUrl: './employee-list.component.html',
  styleUrl: './employee-list.component.css'
})
export class EmployeeListComponent {
  employees: EmployeeDto[] = [];
  displayedColumns: string[] = ['name', 'email', 'role', 'actions'];
  currentEmployeeId!: string;
  currentEmployeeName!: string;
  constructor(
    private employeeService: EmployeeService,
    private router: Router,
    private authService: AuthService,
    private employeeDataService: EmployeeDataService,
    private snackBar: MatSnackBar,
  ) { }

  ngOnInit(): void {
    const userInfo = this.authService.getUserInfo()!;

    this.currentEmployeeId = userInfo.userId!;
    this.currentEmployeeName = userInfo.name!;

    this.employeeService.getAll(this.currentEmployeeId).subscribe({
      next: (data) => (
        this.employees = data
      ),
      error: (err) => console.error('Error fetching employees', err),
    });
  }

  dataSource = new MatTableDataSource<EmployeeDto>([]);

  editEmployee(employee: EmployeeDto) {
    this.employeeDataService.employees = this.employees;
    this.router.navigate(['/employees/edit', employee.id], { state: { employee } });
  }

  goToNewEmployee() {
    this.employeeDataService.employees = this.employees;
    this.router.navigate(['/employees/new', this.currentEmployeeId]);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
    this.snackBar.open('Logout was made successfull', 'Close', {
      duration: 5000,
    });
  }
  deleteEmployee(employee: EmployeeDto) {
    console.log('Delete:', employee);
    if (confirm(`Are you sure you want to delete ${employee.firstName} ${employee.lastName}?`)) {
      this.employeeService.delete(employee.id).subscribe({
        next: () => {
          this.employees = this.employees.filter(e => e.id !== employee.id);
          this.snackBar.open('Employee deleted successfull', 'Close', {
            duration: 5000,
          });
        },
        error: (err) => {
          this.snackBar.open('Error in delete employee', 'Close', {
            duration: 5000,
          });
        }
      });
    }
  }
}
