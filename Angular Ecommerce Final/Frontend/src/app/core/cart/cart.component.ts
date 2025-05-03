import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { CartService } from '../services/cart.service';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { QuantityComponent } from "./quantity/quantity.component";
import { CurrencyPipe } from '@angular/common';
import { NavbarComponent } from "../navbar/navbar/navbar.component";
import { Router, RouterLink, RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { wind } from 'ngx-bootstrap-icons';
import { finalize } from 'rxjs';
import { CartDataService } from '../../shared/CartData/cart-data.service';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [NgxSpinnerModule, QuantityComponent, CurrencyPipe, NavbarComponent,RouterLink,RouterModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent {

  constructor(private cart:CartService,private spinner:NgxSpinnerService,private toastr:ToastrService,private CartData:CartDataService,private router:Router ){}

  customername = localStorage.getItem('user');
  CustomerId:string='';

  cartItems :any[]=[];
  cartTotal:number=0;
  ngOnInit() {
   
    this.getcustomer();
    this.loadcart();
  }

  ngAfterViewInit(){
    this.spinner.show();
    setTimeout(()=>this.spinner.hide(),3000);

  }

  ngOnDestroy() {
    this.updateCart(); // Call API to update the cart when the component is destroyed
  }

  getcustomer(){
    this.cart.getCustomerId(this.customername!).subscribe({
      next: (data: any) => {
        console.log(data);
        JSON.stringify(localStorage.setItem("Id",data.customerId)) 
      },
      error: (error) => {
        console.error('Error fetching customer ID:', error);
      }
    });
  }

  loadcart(){
    this.CustomerId = localStorage.getItem("Id")!;
    this.cart.getcart(this.CustomerId).pipe(
      finalize(() => this.spinner.hide())
    ).subscribe({
      next: (data: any) => {
        this.cartItems = data.cartItems;
        this.calculateCartTotal();
      },
      error: () => {
        this.toastr.error('Error fetching customer cart');
      }
    });

    

  }

  updateQuantity(item: any, quantity: number) {
    item.quantity = quantity; 
    this.calculateCartTotal(); 
  }

  calculateCartTotal() {
    this.cartTotal = this.cartItems.reduce((total:any, item:any) => {
      return total + (item.price * item.quantity); // Calculate total based on quantity
    }, 0);
  }

  removeFromCart(itemId: string) {
    this.cart.removeItem(itemId).subscribe({
      next: () => {
        this.spinner.show();
        window.location.reload();
        setTimeout(()=>{
          this.spinner.hide(),2000
        });
        
        console.log("Item removed from cart");
        this.toastr.warning("Item removed from cart!");
        this.cartItems = this.cartItems.filter(item => item.productId !== itemId);
        this.calculateCartTotal();
      },
      error: (error) => {
        console.error('Error removing item from cart:', error);
      }
    });
  }

  checkout() {
    this.CartData.setCartItems(this.cartItems); // Set cart items in the service
    this.CartData.setTotal(this.cartTotal); //set TotalAmount
    this.router.navigate(['/checkout']); // Navigate to checkout
  }

  updateCart() {
    this.cartItems.forEach(item => {
      const newQuantity = {
        cartItemId: item.cartItemId, // Assuming productId corresponds to cartItemId
        quantity: item.quantity
      };
  
      this.cart.updateCart(newQuantity).subscribe({
        next: () => {
          console.log(`Cart updated successfully for item ${item.productId}`);
        },
        error: (error) => {
          console.error('Error updating cart:', error);
        }
      });
    });
  }

}
