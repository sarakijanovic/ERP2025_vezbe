import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

declare interface RouteInfo {
    path: string;
    title: string;
    icon: string;
    class: string;
}
export const ROUTESAll: RouteInfo[] = [
    { path: '/dashboard', title: 'Products',  icon: 'ni-tv-2 text-primary', class: '' },
    //{ path: '/login', title: 'Login',  icon:'ni-key-25 text-info', class: '' },
];

export const ROUTESNoUser: RouteInfo[] = [
  { path: '/login', title: 'Login',  icon:'ni-key-25 text-info', class: '' },
  { path: '/register', title: 'Register',  icon:'ni-circle-08 text-pink', class: '' },
];

export const ROUTESUser: RouteInfo[] = [
  //{ path: '/register', title: 'Register',  icon:'ni-circle-08 text-pink', class: '' },
  { path: '/user-profile', title: 'User profile',  icon:'ni-single-02 text-yellow', class: '' },
  { path: '/orders-list', title: 'Orders list',  icon:'ni-bullet-list-67 text-red', class: '' },
  { path: '/cart', title: 'Cart',  icon:'ni-cart text-blue', class: '' },
];

export const ROUTESEmployee: RouteInfo[] = [
  { path: '/watercoolers-list', title: 'Watercoolers list',  icon:'ni-bullet-list-67 text-red', class: '' },
  { path: '/orders-list', title: 'Orders list',  icon:'ni-bullet-list-67 text-red', class: '' },
  { path: '/employees-list', title: 'Employees list',  icon:'ni-bullet-list-67 text-red', class: '' },
  { path: '/clients-list', title: 'Clients list',  icon:'ni-bullet-list-67 text-red', class: '' },
];

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {

  public menuItemsAll: any[];
  public menuItemsNoUser: any[];
  public menuItemsUser: any[];
  public menuItemsEmployee: any[];
  public isCollapsed = true;
  role: string;

  constructor(private router: Router) { }

  ngOnInit() {
    this.role = localStorage.getItem("uloga");
    this.menuItemsAll = ROUTESAll.filter(menuItem => menuItem);
    this.router.events.subscribe((event) => {
      this.isCollapsed = true;
   });
   this.menuItemsUser = ROUTESUser.filter(menuItem => menuItem);
    this.router.events.subscribe((event) => {
      this.isCollapsed = true;
   });
   this.menuItemsEmployee = ROUTESEmployee.filter(menuItem => menuItem);
    this.router.events.subscribe((event) => {
      this.isCollapsed = true;
   });
   this.menuItemsNoUser = ROUTESNoUser.filter(menuItem => menuItem);
    this.router.events.subscribe((event) => {
      this.isCollapsed = true;
   });
  }
}
