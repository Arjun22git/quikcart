import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CartDataService {

  private cartItems: any[] = [];
  private CartTotal:number = 0;

  setTotal(totalPrice:number){
    this.CartTotal = totalPrice;
  }

  getTotal(){
    return this.CartTotal;
  }

  setCartItems(items: any[]) {
    this.cartItems = items;
  }

  getCartItems() {
    return this.cartItems;
  }

}
