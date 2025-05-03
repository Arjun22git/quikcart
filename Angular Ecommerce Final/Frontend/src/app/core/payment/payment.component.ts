import { Component, Input } from '@angular/core';
import { NavbarComponent } from "../navbar/navbar/navbar.component";
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { PaymentService } from '../services/payment.service';
import { finalize } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { CartService } from '../services/cart.service';

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [NavbarComponent,CommonModule,FormsModule],
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css'
})
export class PaymentComponent {
  @Input() cartTotal: number = 0; 
  @Input() OrderId:string='';

  isUpiVisible = false;
  isCardVisible = false;

  customerId:string = '';

  ngOnInit(){
    this.customerId = localStorage.getItem("Id") || ''; 

  }
  

  // Properties to bind to the input fields
  upiID: string = '';
  cardNumber: string = '';
  expiryDate: string = '';
  cvv: string = '';



  constructor(private toastr: ToastrService,private payment :PaymentService,private spinner:NgxSpinnerService,private router:Router,private cart:CartService) {}

  toggleCollapse(method: string) {
    if (method === 'upi') {
      this.isUpiVisible = !this.isUpiVisible;
      this.isCardVisible = false; // Close Card section
    } else if (method === 'credit') {
      this.isCardVisible = !this.isCardVisible;
      this.isUpiVisible = false; // Close UPI section
    }
  }

  showInput(method: string) {
    this.toggleCollapse(method); // Show the specific input when focused
  }

  // Check if the user can proceed to pay
  canPay(): boolean {
    if (this.isUpiVisible) {
      return this.upiID.trim() !== '';
    }
    if (this.isCardVisible) {
      return (
        this.cardNumber.trim() !== '' &&
        this.expiryDate.trim() !== '' &&
        this.cvv.trim() !== ''
      );
    }
    return false;
  }

  pay() {
    const paymentData = {
      orderId: this.OrderId,
      amount: this.cartTotal, 
      paymentMethod: this.isUpiVisible ? 'UPI' : 'Credit Card'
    };

    //api call using service
    this.payment.orderPayment(paymentData).pipe(
      finalize(() => this.spinner.hide())
    ).subscribe({
      next: (data: any) => {
        console.log(data,"Payment Success! Order Placed");
        this.toastr.success("Payment Successful! You can view your orders in the orders section.Redirecting to homepage...","Order Placed Successfully!",{
          timeOut:4000,
          tapToDismiss:true,
          progressBar:true
        });       
        this.OrderId=data.orderId;
        this.ClearCart(this.customerId);
        
        this.router.navigateByUrl('/homepage');
      },
      error: () => {
        this.toastr.error("Retry payment",'Payment Failed!',{
          tapToDismiss:true,
          timeOut:3000
        });
      }
    });
  }

  ClearCart(Id:string){
    const Customer = {'customerId': Id}
    this.cart.clearCart(Customer).subscribe({
      next: (data: any) => {
          console.log(data);
      },
      error: () => {
          console.error('Error clearing cart');
      }
  });
  }
}
