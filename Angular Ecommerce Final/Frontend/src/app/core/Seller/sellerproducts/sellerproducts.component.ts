import { CommonModule } from '@angular/common';
import { Component, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SellerService } from '../../services/seller.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerComponent, NgxSpinnerService } from 'ngx-spinner';
import { Router, RouterLink, RouterModule, RouterOutlet } from '@angular/router';
import { AddproductComponent } from "../addproduct/addproduct.component";
import { ProductService } from '../../services/product.service';
import { EditProductComponent } from '../edit-product/edit-product.component';
import { DeleteProductComponent } from '../delete-product/delete-product.component';

@Component({
  selector: 'app-sellerproducts',
  standalone: true,
  imports: [FormsModule, CommonModule, NgxSpinnerComponent, RouterLink, RouterModule, RouterOutlet, AddproductComponent, EditProductComponent, DeleteProductComponent],
  templateUrl: './sellerproducts.component.html',
  styleUrl: './sellerproducts.component.css'
})
export class SellerproductsComponent {
  products: any[] = [];
  sellerId: string = '';
  seller: string = '';
  loading: boolean = true; 
  productToDeleteId: string | null = null;
  sortAscending: boolean = true;

  constructor(private sellerservice: SellerService, private toastr: ToastrService, private spinner: NgxSpinnerService,private router:Router,private product:ProductService) {}

  @ViewChild('addProductModal') addProductModal!: AddproductComponent;
  @ViewChild('editProductModal') editProductModal!: EditProductComponent; 
  @ViewChild('deleteProductModal') deleteProductModal!: DeleteProductComponent;

  ngOnInit() {
      this.seller = localStorage.getItem('seller') || '';
      this.getSellerId()
          .then(() => this.getProducts()).then(()=>this.spinner.hide())
          .catch(err => this.toastr.error(err)).then(()=>this.spinner.hide());
  }

  getSellerId(): Promise<void> {
      return new Promise((resolve, reject) => {
          this.sellerservice.getSellerId(this.seller).subscribe({
              next: (data: any) => {
                  this.sellerId = data.sellerId; 
                  resolve(); 
              },
              error: () => {
                  reject('Error fetching seller ID');
              }
          });
      });
  }

  getProducts(): void {
      if (!this.sellerId) {
          this.toastr.warning('Seller ID is not available.');
          return; 
      }

      this.loading = true; // Start loading
      this.sellerservice.getsellerproducts(this.sellerId).subscribe({
          next: (data: any) => {
              this.products = data || []; 
          },
          error: () => {
              this.toastr.error('Error fetching seller products');
          },
          complete: () => {
              this.loading = false; // Stop loading
              this.spinner.hide();
          }
      });
  }

  addProduct() {
    this.addProductModal.openModal();
  }
  onProductAdded(product: any) {
    this.product.addproduct(product).subscribe({
      next:(data:any)=>{console.log(data)},
      error: () => {
        this.toastr.error('Error adding product!');
      },
      complete:()=>{
        this.toastr.success('Product Added!');
      }
      
    });
  } 

  onProductEdited(product: any) {
    this.product.updateproduct(product).subscribe({
      next:(data:any)=>{console.log(data)},
      error: () => {
        this.toastr.error('Error Updating product details!');
      },
      complete:()=>{
        this.toastr.success('Product Details Updated!');
      }
    });
  } 

editProduct(productId: string): void {
  const product = this.products.find(prod => prod.productId === productId);
  if (product) {
      this.editProductModal.openModal(product); // Open modal
  } else {
      this.toastr.error('Product not found');
  }
}


deleteProduct(productId: string): void {
  this.productToDeleteId = productId; 

  this.deleteProductModal.productName = this.getProductNameById(productId); 
  this.deleteProductModal.open(); 
}

confirmDelete() {
  if (this.productToDeleteId) {
  
    this.product.deleteProduct(this.productToDeleteId).subscribe({
      next:(data:any)=>{console.log(data)},
      error: () => {
        this.toastr.error('Error Deleting product!');
      },
      complete:()=>{
        this.toastr.success('Product Deleted Successfully!');
      }
      
    });

    this.products = this.products.filter(product => product.productId !== this.productToDeleteId);
    this.productToDeleteId = null; 
  }
}

cancelDelete() {
  this.productToDeleteId = null; 
}

getProductNameById(productId: string | null): string {
  const product = this.products.find(p => p.productId === productId);
  return product ? product.name : '';
}

sortProducts() {
  this.sortAscending = !this.sortAscending; 
  this.products.sort((a, b) => {
    const nameA = a.name.toLowerCase(); 
    const nameB = b.name.toLowerCase();
    if (this.sortAscending) {
      return nameA < nameB ? -1 : nameA > nameB ? 1 : 0;
    } else {
      return nameA > nameB ? -1 : nameA < nameB ? 1 : 0;
    }
  });
}
searchTerm: string = ''; 

searchProducts() {
  return this.products.filter(product => 
    product.name.toLowerCase().includes(this.searchTerm.toLowerCase()) || 
    product.category.toLowerCase().includes(this.searchTerm.toLowerCase())
  );
}

}

