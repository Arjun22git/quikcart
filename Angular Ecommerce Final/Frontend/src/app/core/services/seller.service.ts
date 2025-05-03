import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SellerService {

  constructor(private httpClient : HttpClient) { }

  getsellerorders(sellerId:string){
    const params = {sellerId};
    return this.httpClient.get('https://localhost:7113/api/Seller/GetOrdersofSeller',{ params });

  }

  getsellerproducts(sellerId:string){
    const params = {sellerId};
    return this.httpClient.get('https://localhost:7113/api/Seller/GetProductsofSeller',{ params });

  }

  getSellerId(Seller:string){
    const params = {Seller}
    return this.httpClient.get('https://localhost:7113/api/Account/GetSellerId',{params});
  }

  
}
