import { Component, ElementRef, ViewChild } from '@angular/core';
import { Router, RouterLink, RouterModule } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { CartService } from '../../services/cart.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterModule,RouterLink],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {

  customername = localStorage.getItem('user');
  CustomerId:string='';
  Quantity:number=0; 

  constructor(private toast:ToastrService,private spinner:NgxSpinnerService,private router:Router,private cart:CartService){}

  @ViewChild('cartBadge', { static: false }) cartBadge!: ElementRef;

    logout(){
      localStorage.clear();
      this.spinner.show();

      setTimeout(() => {
      
        this.spinner.hide();
      }, 5000);
      this.toast.info("redirecting to login...","Logged out Successfully!",{
        timeOut:3000,
        closeButton:true

      })
      this.router.navigateByUrl('/login');
  }

  ngOnInit() {
    this.getcustomer();
  }
  
  ngAfterViewInit() {
    this.spinner.show();
    // Call cartbadge only after getcustomer is done
    this.getcustomer().then(() => {
      this.cartbadge();
      this.spinner.hide();
    }).catch(() => {
      this.spinner.hide();
    });
  }
  
  getcustomer(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.cart.getCustomerId(this.customername!).subscribe({
        next: (data: any) => {
          console.log(data);
          localStorage.setItem("Id", data.customerId);
          resolve(); // Resolve when done
        },
        error: (error) => {
          console.error('Error fetching customer ID:', error);
          reject(); // Reject on error
        }
      });
    });
  }
  

  cartbadge() {
    this.CustomerId = localStorage.getItem("Id")!;
    this.cart.getcart(this.CustomerId!).subscribe({
        next: (data: any) => {
            console.log(data);
            const itemCount = data.cartItems.length; 
            this.Quantity = itemCount; 
            
            if (this.cartBadge) {
                this.cartBadge.nativeElement.textContent = itemCount > 0 ? itemCount.toString() : '';
                this.cartBadge.nativeElement.style.display = itemCount > 0 ? 'inline' : 'none'; 
            }
        },
        error: (error) => {
            console.error('Error fetching customer cart:', error);
        }
    });
}
}