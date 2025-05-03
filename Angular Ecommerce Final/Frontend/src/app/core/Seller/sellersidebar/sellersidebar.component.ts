import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterModule, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-sellersidebar',
  standalone: true,
  imports: [RouterLink,RouterModule,RouterOutlet,RouterLinkActive],
  templateUrl: './sellersidebar.component.html',
  styleUrl: './sellersidebar.component.css'
})
export class SellersidebarComponent {

}
