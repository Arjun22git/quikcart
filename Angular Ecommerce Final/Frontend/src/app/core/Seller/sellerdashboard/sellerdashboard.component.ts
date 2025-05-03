import { Component } from '@angular/core';
import { SellerNavbarComponent } from "../seller-navbar/seller-navbar.component";
import { SellersidebarComponent } from "../sellersidebar/sellersidebar.component";
import { RouterLink, RouterModule, RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';
import { SellerService } from '../../services/seller.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerComponent, NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';

@Component({
  selector: 'app-sellerdashboard',
  standalone: true,
  imports: [SellerNavbarComponent, SellersidebarComponent,RouterModule,RouterOutlet,RouterLink,CommonModule,NgxSpinnerComponent],
  templateUrl: './sellerdashboard.component.html',
  styleUrl: './sellerdashboard.component.css'
})
export class SellerdashboardComponent {
  seller: string = '';
  sellerId:string = '';

  constructor(private sellerService:SellerService,private toastr:ToastrService,private spinner:NgxSpinnerService){}

  ngOnInit() {
    this.spinner.show();
    this.seller = localStorage.getItem('seller') || '';
    this.getSellerId()
      .then(() => this.setSellerId()).then(()=>this.spinner.hide())
      .catch(err => this.toastr.error(err)).then(()=>this.spinner.hide());
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
          reject('Error occured while fetching Seller Id!'); // Reject the promise
        }
      });
    });
  }

  setSellerId(){
    localStorage.setItem("sellerId",this.sellerId);
  }
}

