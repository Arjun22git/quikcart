import { HttpBackend, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private httpClient :HttpClient
  constructor( handler: HttpBackend) {            //backend for services not to be intercepted by interceptor
    this.httpClient = new HttpClient(handler);
 }
  postlogin(details:any){
    return this.httpClient.post('https://localhost:7113/api/Account/Login',details)
  }

  postregister(register:any)
  {
    return this.httpClient.post('https://localhost:7113/api/Account/RegisterCustomer',register)
  }

  postverify(otp:any)
  {
    return this.httpClient.post('https://localhost:7113/api/Account/VerifyOtp',otp)
  }
}
