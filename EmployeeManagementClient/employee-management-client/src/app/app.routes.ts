import { Routes } from '@angular/router';
import { EmployeeListComponent } from './employees/employee-list/employee-list.component';
import { EmployeeFormComponent } from './employees/employee-form/employee-form.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './core/auth.guard';


export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: '', redirectTo: 'employees', pathMatch: 'full' },
  { path: 'employees', component: EmployeeListComponent , canActivate: [AuthGuard] },
  { path: 'employees/new/:id', component: EmployeeFormComponent, canActivate: [AuthGuard] }, 
  { path: 'employees/edit/:id', component: EmployeeFormComponent, canActivate: [AuthGuard] }, 
];
