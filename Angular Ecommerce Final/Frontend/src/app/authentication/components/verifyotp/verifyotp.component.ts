import { Component } from '@angular/core';
import { FormGroup, FormsModule, NgModel } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Component({
  selector: 'app-verifyotp',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './verifyotp.component.html',
  styleUrl: './verifyotp.component.css'
})
export class VerifyotpComponent {

  otpvalue:string  = '';
  otp:object={}
  user:string = localStorage.getItem("user") || '';
  value: number[] = new Array(6);
  
  constructor(private authenticate: AuthService, private toastr: ToastrService, private router: Router) {}

  onOtpInput(index: number, event: any) {
    const input = event.target.value;
    if (input.length === 1 && !isNaN(input)) {
      this.value[index] = parseInt(input, 10);
      const nextInput = document.querySelector(`.otp-field:nth-of-type(${index + 2})`) as HTMLInputElement;
      nextInput?.focus();
    } else if (input.length === 0 && index > 0) {
      const prevInput = document.querySelector(`.otp-field:nth-of-type(${index})`) as HTMLInputElement;
      prevInput?.focus();
    }
  }

  verifyOtp() {
    this.otpvalue = this.value.join('');
    this.otp={
      'email':this.user,
      'otp':this.otpvalue}
    console.log('Verifying OTP:', this.otp);

    this.authenticate.postverify(this.otp).subscribe({
      next: (data: any) => {
        console.log(data);
        localStorage.removeItem("user");
        localStorage.removeItem("pass");
        this.toastr.success("User Verified!", "Please login again to continue.", {
          timeOut: 3000,
          closeButton: true
        });
        this.router.navigateByUrl('/login');
      },
      error: (err: any) => {
        console.error(err);
        this.toastr.error("Verification failed. Please try again.");
      },
      complete: () => {
        console.log("Execution completed");
      }
    });
  }

  // resendOtp() {
  //   // Logic to resend OTP
  //   this.authenticate.resendOtp().subscribe({
  //     next: () => {
  //       this.toastr.info("OTP resent. Please check your email.");
  //     },
  //     error: (err) => {
  //       console.error(err);
  //       this.toastr.error("Failed to resend OTP. Please try again.");
  //     }
  //   });
  // }
}
