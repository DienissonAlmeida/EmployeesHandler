import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface EmployeeDto {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumbers: string[];
  birthDate: string;
  role: string;
  // Add more fields if needed
}

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {


  private readonly apiUrl = 'http://localhost:5151/api/Employees';

  constructor(private http: HttpClient) { }

  getAll(): Observable<EmployeeDto[]> {
    return this.http.get<EmployeeDto[]>(this.apiUrl);
  }
}
