import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgxSpinnerComponent, NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-seller-navbar',
  standalone: true,
  imports: [NgxSpinnerComponent,],
  templateUrl: './seller-navbar.component.html',
  styleUrl: './seller-navbar.component.css'
})
export class SellerNavbarComponent implements OnInit {
  @Input() seller: string = '';

  constructor(
    private toast: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) {}

  ngOnInit() {
    
  }

  logout() {
    const confirmLogout = confirm("Are you sure you want to log out?");
    if (!confirmLogout) return; 

   
    this.spinner.show();

    
    localStorage.clear();
    
    
    this.toast.info("Logged out successfully!", "Redirecting to login...", {
        timeOut: 2000,
        closeButton: true
    });


    setTimeout(() => {
        this.spinner.hide();
        this.router.navigateByUrl('/login');
    }, 1000); 
}
  
}