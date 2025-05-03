import { CurrencyPipe, DatePipe, JsonPipe } from '@angular/common';
import { Component } from '@angular/core';
import { AdminService } from '../../services/admin.service';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [DatePipe,CurrencyPipe],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css'
})
export class OrdersComponent {

  orders: any[] = [];

  constructor(private admin: AdminService) {}

  ngOnInit() {
    this.ViewAllOrders();
  }

  ViewAllOrders() {
    this.admin.getAllOrders().subscribe((data: any) => {
      console.log(data);
      this.orders = data.map((order: any) => {
        return {
          ...order,
          shippingAddress: JSON.parse(order.shippingAddress) // Parse shippingAddress
        };
      });
    });
  }
}
