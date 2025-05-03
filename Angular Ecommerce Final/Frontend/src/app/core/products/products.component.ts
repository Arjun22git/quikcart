import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterModule } from '@angular/router';
import { NavbarComponent } from "../navbar/navbar/navbar.component";
import { CurrencyPipe } from '@angular/common';
import { ProductService } from '../services/product.service';
import { ProductListComponent} from "./productlist/productlist.component";
import { FilterComponent } from "./filter/filter.component";

@Component({
  selector: 'app-products',
  standalone: true,
  imports: [RouterLink, RouterModule, NavbarComponent, CurrencyPipe, FilterComponent,ProductListComponent],
  templateUrl: './products.component.html',
  styleUrl: './products.component.css'
})
export class ProductComponent implements OnInit {
  products: any[] = [];
  filteredProducts: any[] = [];
  
  priceRange: number = 2000; // Default max price
  categories = [
    { name: 'Fashion', selected: false },
    { name: 'Footwear', selected: false },
    { name: 'Electronics', selected: false },
  ];

  constructor(private pdt: ProductService) {}

  ngOnInit() {
    this.getAllProducts();
  }

  getAllProducts() {
    this.pdt.getproducts().subscribe({
      next: (data: any) => {
        console.log("Products Fetched!", data);
        this.products = data.products;
        console.log(this.products);
        this.filteredProducts = data; // Initialize with all products
      },
      error: (err: any) => console.log(err)
    });
  }

  filterProducts() {
    // this.filteredProducts = this.products.filter(product => {
    //   const isWithinPriceRange = product.price <= this.priceRange;
    //   const isInSelectedCategory = this.categories.some(category =>
    //     category.selected && product.category === category.name
    //   );
    //   return isWithinPriceRange && (this.categories.every(category => !category.selected) || isInSelectedCategory);
    // });
  }

  addToCart(product: any) {
    console.log(`${product.name} added to cart!`);
    // Implement cart logic here
  }

  

}