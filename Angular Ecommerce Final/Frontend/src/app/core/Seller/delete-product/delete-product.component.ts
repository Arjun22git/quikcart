import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-delete-product',
  standalone: true,
  imports: [],
  templateUrl: './delete-product.component.html',
  styleUrl: './delete-product.component.css'
})
export class DeleteProductComponent {
  @Input() productName: string = '';
  @Output() confirm = new EventEmitter<void>();
  @Output() cancel = new EventEmitter<void>();

  visible: boolean = false; 

  open() {
    this.visible = true; 
  }

  close() {
    this.visible = false; 
  }

  onConfirm() {
    this.confirm.emit();
    this.close(); 
  }

  onCancel() {
    this.cancel.emit();
    console.log("cancel");
    this.close(); 
  }

}
