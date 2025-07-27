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
  constructor(
    private employeeService: EmployeeService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.employeeService.getAll().subscribe({
      next: (data) => (this.employees = data),
      error: (err) => console.error('Error fetching employees', err),
    });
  }

  dataSource = new MatTableDataSource<EmployeeDto>([]);

  editEmployee(employee: EmployeeDto) {
    // Open dialog, navigate, or patch form
    console.log('Edit:', employee);
    this.router.navigate(['/employees', employee.id], { state: { employee } });

  }

  deleteEmployee(employee: EmployeeDto) {
    // Confirm and call delete service
    console.log('Delete:', employee);
    if (confirm(`Are you sure you want to delete ${employee.firstName} ${employee.lastName}?`)) {
      this.employeeService.delete(employee.id).subscribe({
        next: () => {
          // Remove o funcionário da lista local
          this.employees = this.employees.filter(e => e.id !== employee.id);
        },
        error: (err) => {
          console.error('Delete failed', err);
          alert('Failed to delete employee.');
        }
      });
    }
  }
}
