import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  constructor(private httpClient:HttpClient) { }

  getCustomerId(email: string) {
    const params = { email }; 
    return this.httpClient.get('https://localhost:7113/api/Account/Customer', { params });
  }
  

  getcart(customerId:string){
    
    const params = { customerId }; 
    return this.httpClient.get('https://localhost:7113/api/Cart/ViewCart',{ params });
    
  }

  addtocart(newProduct:any){
    return this.httpClient.post('https://localhost:7113/api/Cart/AddtoCart',newProduct);
  }

  removeItem(id:string){
    const params = {id}
    return this.httpClient.delete('https://localhost:7113/api/Cart/RemoveitemfromCart',{ params });
  }

  updateCart(item:any){
    return this.httpClient.put('https://localhost:7113/api/Cart/UpdateQuantity',item);
  }

  clearCart(customer:object)
  {
    return this.httpClient.post('https://localhost:7113/api/Cart/ClearCart',customer);
  }
}
