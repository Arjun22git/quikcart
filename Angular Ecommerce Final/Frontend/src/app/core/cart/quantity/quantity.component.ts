import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-quantity',
  standalone: true,
  imports: [],
  templateUrl: './quantity.component.html',
  styleUrl: './quantity.component.css'
})
export class QuantityComponent {
  @Input() quantity: number = 1;
  @Output() quantityChange = new EventEmitter<number>();

  decrement() {
    if (this.quantity > 1) {
      this.quantity--;
      this.quantityChange.emit(this.quantity);
    }
  }

  increment() {
    if (this.quantity < 3) { // Set maximum quantity to 3
      this.quantity++;
      this.quantityChange.emit(this.quantity);
    }
  }
}
