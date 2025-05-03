import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-filter',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './filter.component.html',
  styleUrl: './filter.component.css'
})
export class FilterComponent {

  @Output() filterChanged = new EventEmitter<void>();

  priceRange: number = 10000; // Default max price
  categories = [
    { name: 'Fashion', selected: false },
    { name: 'Footwear', selected: false },
    { name: 'Electronics', selected: false },
  ];

  updateFilters() {
    this.filterChanged.emit();
  }
}
