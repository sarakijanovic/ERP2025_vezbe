import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { EmployeeDTO } from 'src/app/DTOs/employeeDTO';
import { EmployeeDialogComponent } from 'src/app/components/dialogs/employee-dialog/employee-dialog.component';
import { EmployeeService } from 'src/app/services/employeeService/employee.service';
import { RoleService } from 'src/app/services/roleService/role.service';

@Component({
  selector: 'app-employees-list',
  templateUrl: './employees-list.component.html',
  styleUrls: ['./employees-list.component.scss']
})
export class EmployeesListComponent implements OnInit {

  employees: EmployeeDTO[] = [];
  employeeInfo: EmployeeDTO;
  roles: any = [];
  role: string;

  constructor(private employeeService: EmployeeService,
    private roleService: RoleService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.role = localStorage.getItem("uloga")
    this.employeeService.getAllEmployees().subscribe(
      (response: any) => {
        this.employees = response;
        this.employees.forEach(element => {
          this.roleService.getRoleById(element.ulogaID).subscribe(res => {
            element.uloga = res.nazivUloge
          }
          )

        });
      },
      (error: any) => {
        console.log('Error occurred while loading employees:', error);
      }
    );
    this.roleService.getAllRoles().subscribe(res => {
      this.roles = res
    }
    )
  }

  updateEmployee(employee: EmployeeDTO) {
    this.employeeInfo = employee;
    this.employeeInfo.lozinkaZaposlenog = employee.korisnickoImeZaposlenog;
    let dialog = this.dialog.open(EmployeeDialogComponent, {
      width: '600px',
      data: {
        isEdit: true,
        employee: this.employeeInfo,
        roles: this.roles
      }
    });
    dialog.afterClosed().subscribe(result => {
      if (result) {
        result.zaposleniID = this.employeeInfo.zaposleniID
        this.employeeService.updateEmployee(result).subscribe(res => {
          const employeeIndex = this.employees.findIndex( (obj: EmployeeDTO) => obj.zaposleniID == employee.zaposleniID);
          this.employees[employeeIndex] = result;
          this.roles.forEach(element => {
            if(element.ulogaID == result.ulogaID) {
              this.employees[employeeIndex].uloga = element.nazivUloge
            }
          });
        });
      }
    });
  }

  createWatercooler() {
    let dialog = this.dialog.open(EmployeeDialogComponent, {
      width: '600px',
      data: {
        isEdit: false,
        employee: {
          imeZaposlenog: '',
          prezimeZaposlenog: '',
          jmbg: '',
          korisnickoImeZaposlenog: '',
          lozinkaZaposlenog: '',
          emailZaposlenog: '',
          uloga: '',
          ulogaID: ''
        },
        roles: this.roles
      }
    });
    dialog.afterClosed().subscribe(result => {
      if (result) {
        this.employeeService.createEmployee(result).subscribe(res => {
          this.roles.forEach(element => {
            if(element.ulogaID == res.ulogaID) {
              res.uloga = element.nazivUloge
            }
          });
          this.employees.push(res);
        });
      }
    });
  }

  deleteEmployee(employee: EmployeeDTO) {
    if(confirm("Are you sure you want to delete employee " + employee.korisnickoImeZaposlenog + "?")) {
      const employeeIndex = this.employees.findIndex( (obj: EmployeeDTO) => obj.zaposleniID == employee.zaposleniID);
      this.employees.splice(employeeIndex, 1);
      this.employeeService.deleteEmployee(employee.zaposleniID).subscribe();
    }
  }
}
