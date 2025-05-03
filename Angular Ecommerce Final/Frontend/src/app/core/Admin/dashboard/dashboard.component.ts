import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterModule, RouterOutlet } from '@angular/router';
import { AdminnavbarComponent } from "../adminnavbar/adminnavbar.component";
import { AdminsidebarComponent } from "../adminsidebar/adminsidebar.component";
import { AdminhomeComponent } from "../adminhome/adminhome.component";

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [AdminnavbarComponent, DashboardComponent, RouterLink, RouterModule, AdminsidebarComponent, RouterOutlet, AdminhomeComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
  admin: string = '';

  ngOnInit() {
    this.admin = localStorage.getItem('admin') || ''; 
  }
  

}
