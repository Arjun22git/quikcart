import { CommonModule, NgClass } from '@angular/common';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgxSpinnerModule, NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-edit-product',
  standalone: true,
  imports: [ReactiveFormsModule,FormsModule,CommonModule,NgxSpinnerModule],
  templateUrl: './edit-product.component.html',
  styleUrl: './edit-product.component.css'
})
export class EditProductComponent implements OnInit {
  productForm!: FormGroup;
  isVisible = false;

  @Output() productEdited = new EventEmitter<any>();

  constructor(private fb: FormBuilder, private toastr: ToastrService,private spinner:NgxSpinnerService) {}

  ngOnInit() {
      this.productForm = this.fb.group({
          productId: [''], 
          name: ['', Validators.required],
          description: [''],
          category: ['', Validators.required],
          price: [null, [Validators.required, Validators.min(0)]],
          quantity: [null, [Validators.required, Validators.min(0)]],
          imageUrl: ['', Validators.required]
      });
  }

  openModal(product: any) {

      this.productForm.patchValue(product); // Populate the form with product data
      this.isVisible = true; // Show modal
  }

  closeModal() {
      this.isVisible = false; // Hide modal
      this.productForm.reset(); // Reset the form
  }

  onSubmit() {
    if (this.productForm.valid) {
        this.productEdited.emit(this.productForm.value);
        this.toastr.success('Product Updated Successfully!');
        this.spinner.show(); // Show spinner before reloading
        
        // Delay for a brief moment to show the spinner
        setTimeout(() => {
            location.reload(); // Reload the page
        }, 1000); // Adjust the delay as needed

        this.closeModal();
    } else {
        this.toastr.error('Please fill in all required fields.');
    }
}
}
