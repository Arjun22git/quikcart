import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AdminService {

  constructor(private httpclient:HttpClient) { }

  getAllOrders(){
    return this.httpclient.get('https://localhost:7113/api/Order/GetAllOrders');
  }

  getAllUsers(){
    return this.httpclient.get('https://localhost:7113/api/Account/ViewAllUsers');
  }

}
