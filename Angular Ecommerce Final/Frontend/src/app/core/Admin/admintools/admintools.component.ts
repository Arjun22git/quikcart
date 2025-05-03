import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterModule, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-admintools',
  standalone: true,
  imports: [RouterLink,RouterLinkActive,RouterModule,RouterOutlet],
  templateUrl: './admintools.component.html',
  styleUrl: './admintools.component.css'
})
export class AdmintoolsComponent {

}
