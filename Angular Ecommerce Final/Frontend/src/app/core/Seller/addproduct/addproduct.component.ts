import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { wind } from 'ngx-bootstrap-icons';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-addproduct',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule],
  templateUrl: './addproduct.component.html',
  styleUrl: './addproduct.component.css'
})
export class AddproductComponent implements OnInit {
  productForm: FormGroup;
    isVisible: boolean = false;
    SellerId: string= '';

    @Output() productAdded = new EventEmitter<any>();

    constructor(private fb: FormBuilder,private toastr:ToastrService) {
        this.productForm = this.fb.group({
            name: ['', Validators.required],
            description: [''],
            category: ['', Validators.required],
            price: [null, [Validators.required, Validators.min(0)]],
            quantity: [null, [Validators.required, Validators.min(1)]],
            imageurl: ['', Validators.required],
            sellerId: [localStorage.getItem("sellerId")],
        });
    }

    ngOnInit(): void {
      
      this.SellerId=localStorage.getItem("sellerId") || '';
    }

    onSubmit() {
        if (this.productForm.valid) {
            this.productAdded.emit(this.productForm.value);
            this.closeModal();
            this.toastr.success("Product Added Successfully!");
        }
    }

    openModal() {
        this.isVisible = true;
    }

    closeModal() {
        this.isVisible = false;
        this.productForm.reset();
    }

}

