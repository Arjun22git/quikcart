import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private httpClient:HttpClient) { }

  PostOrder(OrderDetails:any){
    return this.httpClient.post('https://localhost:7113/api/Order/PlaceOrder',OrderDetails);
  }

}
