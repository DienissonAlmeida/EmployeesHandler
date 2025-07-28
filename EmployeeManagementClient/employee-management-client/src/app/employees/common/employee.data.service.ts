import { Injectable } from "@angular/core";
import { EmployeeDto } from "../../core/employee.service";

@Injectable({ providedIn: 'root' })
export class EmployeeDataService {
  private _employees: EmployeeDto[] = [];

  set employees(value: EmployeeDto[]) {
    this._employees = value;
  }

  get employees(): EmployeeDto[] {
    return this._employees;
  }

  clear() {
    this._employees = [];
  }
}
