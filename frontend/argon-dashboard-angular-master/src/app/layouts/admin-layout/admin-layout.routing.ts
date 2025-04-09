import { Routes } from '@angular/router';

import { DashboardComponent } from '../../pages/dashboard/dashboard.component';
import { IconsComponent } from '../../pages/icons/icons.component';
import { MapsComponent } from '../../pages/maps/maps.component';
import { UserProfileComponent } from '../../pages/user-profile/user-profile.component';
import { WatercoolersList } from '../../pages/watercoolersList/watercoolers-list.component';
import { CartComponent } from 'src/app/components/cart/cart.component';
import { ClientsListComponent } from 'src/app/pages/clients-list/clients-list.component';
import { EmployeesListComponent } from 'src/app/pages/employees-list/employees-list.component';
import { OrdersListComponent } from 'src/app/pages/orders-list/orders-list.component';

export const AdminLayoutRoutes: Routes = [
    { path: 'dashboard',      component: DashboardComponent },
    { path: 'user-profile',   component: UserProfileComponent },
    { path: 'watercoolers-list', component: WatercoolersList },
    { path: 'employees-list', component: EmployeesListComponent },
    { path: 'clients-list', component: ClientsListComponent },
    { path: 'orders-list', component: OrdersListComponent },
    { path: 'icons',          component: IconsComponent },
    { path: 'maps',           component: MapsComponent },
    { path: 'cart',           component: CartComponent }
];
