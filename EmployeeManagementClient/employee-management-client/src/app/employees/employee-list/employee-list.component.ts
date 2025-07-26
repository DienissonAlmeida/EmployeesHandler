import { Component } from '@angular/core';
import { EmployeeDto, EmployeeService } from '../../core/employee.service';
import { CommonModule } from '@angular/common'; 
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator'; // optional
import { MatSortModule } from '@angular/material/sort'; // optional

@Component({
  selector: 'app-employee-list',
  imports: [CommonModule, MatTableModule, MatPaginatorModule, MatSortModule],
  templateUrl: './employee-list.component.html',
  styleUrl: './employee-list.component.css'
})
export class EmployeeListComponent {
  employees: EmployeeDto[] = [];
  displayedColumns: string[] = ['name', 'email', 'role'];
  constructor(private employeeService: EmployeeService) {}

    ngOnInit(): void {
    this.employeeService.getAll().subscribe({
      next: (data) => (this.employees = data),
      error: (err) => console.error('Error fetching employees', err),
    });
  }
}
