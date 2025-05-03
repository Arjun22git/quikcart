import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { LoginComponent } from "./authentication/components/login/login.component";
import { HomeComponent } from "./core/Home/home/home.component";
import { NgxSpinnerModule } from 'ngx-spinner';
import { AuthInterceptor } from './core/interceptors/auth.interceptor';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, LoginComponent, HomeComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Ecom';
  IsLoggedIn:boolean = false;

}
