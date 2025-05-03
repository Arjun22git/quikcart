import { Component, ElementRef, Input, ViewChild, viewChild } from '@angular/core';
import { CurrencyPipe } from '@angular/common';
import { CartService } from '../../services/cart.service';
import { ToastrService } from 'ngx-toastr';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-productlist',
  standalone: true,
  imports: [CurrencyPipe,FormsModule],
  templateUrl: './productlist.component.html',
  styleUrl: './productlist.component.css'
})
export class ProductListComponent {
  @Input() products: any[] = [];
  newProduct: any = {};
  sortAscending: boolean = true;
  searchTerm: string = '';
  
  // Pagination
  currentPage: number = 1;
  itemsPerPage: number = 4;

  constructor(private cart: CartService, private toastr: ToastrService) {}

  @ViewChild('addtocartbtn', { static: true }) addbtn!: ElementRef;

  addToCart(productId: string) {
    console.log(`${productId} added to cart!`);
    const customerId = localStorage.getItem("Id");
    this.newProduct = { 'customerId': customerId, 'productId': productId, 'quantity': 1 };

    this.cart.addtocart(this.newProduct).subscribe({
      next: (data: any) => {
        console.log(data);
        this.toastr.success("Item added to cart successfully!", "", {
          closeButton: true,
          timeOut: 1500
        });
      },
      error: (error) => {
        this.toastr.error('Could not add item to cart.', 'Quantity exceeded!', {
          timeOut: 2000,
          tapToDismiss: true
        });
        console.error('Could not add item to cart!', error);
      }
    });
  }

  addToWishlist(productId: string) {
    // Implement wishlist functionality
  }

  searchProducts() {
    return this.products.filter(product => 
      product.name.toLowerCase().includes(this.searchTerm.toLowerCase()) || 
      product.category.toLowerCase().includes(this.searchTerm.toLowerCase())
    );

  }

  sortProducts() {
    this.sortAscending = !this.sortAscending;
    this.products = [...this.products].sort((a, b) => {
      const nameA = a.name.toLowerCase();
      const nameB = b.name.toLowerCase();
      return this.sortAscending ? nameA.localeCompare(nameB) : nameB.localeCompare(nameA);
    });
  }

  get paginatedProducts() {
    const filteredProducts = this.searchProducts();
    const startIndex = (this.currentPage - 1) * this.itemsPerPage;
    return filteredProducts.slice(startIndex, startIndex + this.itemsPerPage);
  }

  get totalPages() {
    return Math.ceil(this.searchProducts().length / this.itemsPerPage);
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
    }
  }

  previousPage() {
    if (this.currentPage > 1) {
      this.currentPage--;
    }
  }
}