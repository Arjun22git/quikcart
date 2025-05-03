import { A } from '@angular/cdk/keycodes';
import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterModule, RouterOutlet } from '@angular/router';
import { AdmintoolsComponent } from '../admintools/admintools.component';
import { AdminhomeComponent } from '../adminhome/adminhome.component';

@Component({
  selector: 'app-adminsidebar',
  standalone: true,
  imports: [RouterLink,RouterModule,AdmintoolsComponent,RouterOutlet,AdminhomeComponent,RouterLinkActive],
  templateUrl: './adminsidebar.component.html',
  styleUrl: './adminsidebar.component.css'
})
export class AdminsidebarComponent {



}
