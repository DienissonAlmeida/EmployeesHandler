import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface EmployeeDto {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  documentNumber: string;
  phoneNumbers: string[];
  managerId?: string; // optional
  password: string;
  birthDate: string;
  role: string;
}

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {


  private readonly apiUrl = 'http://localhost:5000/api/Employees';

  constructor(private http: HttpClient) { }

  getAll(id: string): Observable<EmployeeDto[]> {
    return this.http.get<EmployeeDto[]>(`${this.apiUrl}/${id}`);
  }
    getAllToLink(id: string): Observable<EmployeeDto[]> {
    return this.http.get<EmployeeDto[]>(`${this.apiUrl}/link/${id}`);
  }
  add(id: string, employee: EmployeeDto) {
    return this.http.post(`${this.apiUrl}/${id}`, employee);
  }
  update(employee: EmployeeDto) {
    return this.http.put(`${this.apiUrl}/${employee.id}`, employee);
  }
  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
