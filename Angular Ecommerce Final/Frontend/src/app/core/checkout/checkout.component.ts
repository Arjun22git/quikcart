import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { NavbarComponent } from '../navbar/navbar/navbar.component';
import { CurrencyPipe } from '@angular/common';
import { CartService } from '../services/cart.service';
import { CartDataService } from '../../shared/CartData/cart-data.service';
import { OrderService } from '../services/order.service';
import { finalize } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { PaymentComponent } from '../payment/payment.component';

@Component({
  selector: 'app-checkout',
  standalone: true,
  imports: [FormsModule,NgxSpinnerModule,NavbarComponent,CurrencyPipe,PaymentComponent],
  templateUrl: './checkout.component.html',
  styleUrl: './checkout.component.css'
})
export class CheckoutComponent {
  cartItems:any[] = []; 
  cartTotal = 0;  
  shippingInfo = {
    name: '',
    address: '',
    phone: ''
  };
  shippingAddress:string ='';
  Customer:string='';
  OrderDetails:any={};
  orderPlaced:boolean=false;
  OrderId:string='';

  constructor(private router: Router,
    private CartData:CartDataService,
    private order:OrderService,
    private spinner:NgxSpinnerService,
    private toastr:ToastrService) 
    {
    this.calculateCartTotal();
    }

  ngOnInit() {
    this.Customer = localStorage.getItem('Id')!;
    this.cartItems=this.CartData.getCartItems();
    this.cartTotal = this.CartData.getTotal();
  }
  ngAfterViewInit(){
    this.cartItems=this.CartData.getCartItems();
    this.cartTotal = this.CartData.getTotal();
  }

  // Method to calculate the total amount in the cart
  calculateCartTotal() {
    this.cartTotal = this.cartItems.reduce((total, item) => total + (item.price * item.quantity), 0);
  }

  submitShippingInfo() {

    if (this.isShippingInfoValid()) {
    
      console.log('Shipping Info:', this.shippingInfo);
      this.shippingAddress = JSON.stringify(this.shippingInfo);
      console.log('Shipping Address :',this.shippingAddress);


      this.PlaceOrder(this.Customer,this.cartItems,this.shippingAddress);

    } else {
      alert('Please fill in all fields correctly.');
    }
  }
  isShippingInfoValid() {
    return this.shippingInfo.name && this.shippingInfo.address && this.shippingInfo.phone;
  }


  PlaceOrder(customerId:string,orderItems:any[],shippingAddress:string){

    this.OrderDetails = {'customerId':customerId,'orderItems':orderItems,'shippingAddress':shippingAddress};

    this.order.PostOrder(this.OrderDetails).pipe(
      finalize(() => this.spinner.hide())
    ).subscribe({
      next: (data: any) => {
        console.log(data,"Order Created");
        this.orderPlaced=true;
        this.OrderId=data.orderId;
      },
      error: () => {
        this.toastr.error('Could not Place Order!');
      }
    });
    
  }

}
