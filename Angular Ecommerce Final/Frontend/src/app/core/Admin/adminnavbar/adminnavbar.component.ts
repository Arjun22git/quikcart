import { Component, Input } from '@angular/core';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-adminnavbar',
  standalone: true,
  imports: [RouterLink,RouterModule],
  templateUrl: './adminnavbar.component.html',
  styleUrl: './adminnavbar.component.css'
})
export class AdminnavbarComponent {

  @Input() admin: string = '';

  constructor(
    private toast: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) {}

  ngOnInit() {
    
  }

  logout() {
    const confirmLogout = confirm("Are you sure you want to log out?");
    if (!confirmLogout) return; // Exit if user cancels
  
    localStorage.clear();
    this.spinner.show();
    
    this.toast.info("Logged out Successfully!", "Redirecting to login...", {
      timeOut: 3000,
      closeButton: true
    });
  
    setTimeout(() => {
      this.spinner.hide();
      this.router.navigateByUrl('/login');
    }, 3000); // Reduced delay
  }
}
