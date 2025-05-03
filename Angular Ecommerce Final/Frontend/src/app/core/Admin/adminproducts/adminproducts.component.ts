import { Component, OnInit } from '@angular/core';
import { FilterComponent } from '../../products/filter/filter.component';
import { ProductListComponent} from '../../products/productlist/productlist.component';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-adminproducts',
  standalone: true,
  imports: [FilterComponent,ProductListComponent],
  templateUrl: './adminproducts.component.html',
  styleUrl: './adminproducts.component.css'
})
export class AdminproductsComponent implements OnInit {
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
        this.filteredProducts = data; // Initialize with all products
      },
      error: (err: any) => console.log(err)
    });
  }

  filterProducts() {
  //   this.filteredProducts = this.products.filter(product => {
  //     const isWithinPriceRange = product.price <= this.priceRange;
  //     const isInSelectedCategory = this.categories.some(category =>
  //       category.selected && product.category === category.name
  //     );
  //     return isWithinPriceRange && (this.categories.every(category => !category.selected) || isInSelectedCategory);
  //   });
  }


}
