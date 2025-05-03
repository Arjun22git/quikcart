import { Component, OnInit } from '@angular/core';
import { SellerService } from '../../services/seller.service';
import { finalize } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-seller-orders',
  standalone: true,
  imports: [CurrencyPipe],
  templateUrl: './seller-orders.component.html',
  styleUrl: './seller-orders.component.css'
})
export class SellerOrdersComponent implements OnInit {
  
  orders: any[] = [];
  //orderItems:any[] = [];
  seller: string = '';
  sellerId: string = '';

  constructor(
    private sellerService: SellerService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService
  ) {}

  ngOnInit() {
    this.seller = localStorage.getItem('seller') || '';
    this.getSellerId()
      .then(() => this.getOrders())
      .catch(err => this.toastr.error(err));
  }

  getSellerId(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.sellerService.getSellerId(this.seller).subscribe({
        next: (data: any) => {
          console.log(data);
          this.sellerId = data.sellerId; 
          resolve(); 
        },
        error: () => {
          reject('Error fetching seller ID'); // Reject the promise
        }
      });
    });
  }

  getOrders(): void {
    if (!this.sellerId) {
      this.toastr.warning('Seller ID is not available.');
      return; 
    }

    this.spinner.show(); 
    this.sellerService.getsellerorders(this.sellerId).subscribe({
      next: (data: any) => {
        console.log(data);
        this.orders = data || []; 
      },
      error: () => {
        this.toastr.error('Error fetching seller orders');
      },
      complete: () => {
        this.spinner.hide(); 
      }
    });
  }

  parseShippingAddress(address: string): any {
    return address ? JSON.parse(address) : {}; 
  }

  formatDate(dateString: string): string {
    return dateString ? new Date(dateString).toLocaleString() : ''; 
  }
}