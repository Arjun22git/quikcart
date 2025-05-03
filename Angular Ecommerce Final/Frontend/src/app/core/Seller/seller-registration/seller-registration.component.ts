import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-seller-registration',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './seller-registration.component.html',
  styleUrl: './seller-registration.component.css'
})
export class SellerRegistrationComponent implements OnInit {
  sellerForm!: FormGroup;

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.sellerForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required],
      storeName: ['', Validators.required],
      storeDescription: ['', Validators.required],
      terms: [false, Validators.requiredTrue]
    });
  }

  onSubmit() {
    if (this.sellerForm.valid) {
      console.log(this.sellerForm.value);
      // Submit logic here, such as making an API call
    }
  }
}
