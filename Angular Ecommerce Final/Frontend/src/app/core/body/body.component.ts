import { Component } from '@angular/core';
import { ProductService } from '../services/product.service';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-body',
  standalone: true,
  imports: [CurrencyPipe],
  templateUrl: './body.component.html',
  styleUrl: './body.component.css'
})
export class BodyComponent {

  constructor(private product :ProductService ){}
  products:any={};
  

  ngOnInit(){
    this.getallproducts();
  }

  getallproducts(){
    this.product.getproducts().subscribe({
      next: (data: any) => {
        console.log("Products Fetched!",data);
        this.products = data;
      }, error: (err: any) => console.log(err)
    });
  }

  

}
