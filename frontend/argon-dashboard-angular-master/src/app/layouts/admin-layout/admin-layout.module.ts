import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ClipboardModule } from 'ngx-clipboard';

import { AdminLayoutRoutes } from './admin-layout.routing';
import { DashboardComponent } from '../../pages/dashboard/dashboard.component';
import { IconsComponent } from '../../pages/icons/icons.component';
import { MapsComponent } from '../../pages/maps/maps.component';
import { UserProfileComponent } from '../../pages/user-profile/user-profile.component';
import { WatercoolersList } from '../../pages/watercoolersList/watercoolers-list.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { MatDialogModule } from '@angular/material/dialog';
import { CartComponent } from 'src/app/components/cart/cart.component';
import { WatercoolerUpdateDialogComponent } from 'src/app/components/dialogs/watercooler-update-dialog/watercooler-update-dialog.component';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatButtonModule } from '@angular/material/button';
import { MatNativeDateModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { EmployeesListComponent } from 'src/app/pages/employees-list/employees-list.component';
import { ClientsListComponent } from 'src/app/pages/clients-list/clients-list.component';
import { OrdersListComponent } from 'src/app/pages/orders-list/orders-list.component';
import { EmployeeDialogComponent } from 'src/app/components/dialogs/employee-dialog/employee-dialog.component';
import { ClientDialogComponent } from 'src/app/components/dialogs/client-dialog/client-dialog.component';
import { OrderDialogComponent } from 'src/app/components/dialogs/order-dialog/order-dialog.component';
import {MatCheckboxModule} from '@angular/material/checkbox'; 
import { OrderDetailsComponent } from 'src/app/components/dialogs/order-details/order-details.component';


// import { ToastrModule } from 'ngx-toastr';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(AdminLayoutRoutes),
    FormsModule,
    HttpClientModule,
    NgbModule,
    MatDialogModule,
    ClipboardModule,
    ReactiveFormsModule,
    MatSnackBarModule,
    MatFormFieldModule,
    MatDatepickerModule,
    MatInputModule,
    MatIconModule,
    MatButtonModule,
    MatNativeDateModule,
    MatSelectModule,
    MatCheckboxModule
  ],
  declarations: [
    DashboardComponent,
    UserProfileComponent,
    WatercoolersList,
    EmployeesListComponent,
    ClientsListComponent,
    OrdersListComponent,
    IconsComponent,
    MapsComponent,
    CartComponent,
    WatercoolerUpdateDialogComponent,
    EmployeeDialogComponent,
    ClientDialogComponent,
    OrderDialogComponent,
    OrderDetailsComponent
  ]
})

export class AdminLayoutModule {}
