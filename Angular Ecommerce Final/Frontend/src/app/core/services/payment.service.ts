import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  constructor(private httpClient:HttpClient) { }

  orderPayment(order:any){
    return this.httpClient.post('https://localhost:7113/api/Order/Payment',order)
  }
}
