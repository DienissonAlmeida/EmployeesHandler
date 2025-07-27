import { Routes } from '@angular/router';
import { EmployeeListComponent } from './employees/employee-list/employee-list.component';
import { EmployeeFormComponent } from './employees/employee-form/employee-form.component';


export const routes: Routes = [
  { path: '', component: EmployeeListComponent },
  { path: 'employees', component: EmployeeListComponent },
  { path: 'employees/new', component: EmployeeFormComponent }, 
  { path: 'employees/:id', component: EmployeeFormComponent }, 
];
