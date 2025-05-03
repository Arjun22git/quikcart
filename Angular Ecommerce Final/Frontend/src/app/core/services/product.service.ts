import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private httpClient:HttpClient) { }

  getproducts(){
    return this.httpClient.get('https://localhost:7113/api/Product/GetAllProducts');  
  }

  addproduct(newProduct:any){
    return this.httpClient.post('https://localhost:7113/api/Product/AddNewProduct',newProduct);
  }

  updateproduct(Product:any){
    return this.httpClient.put('https://localhost:7113/api/Product/UpdateProductDetails',Product);
  }

  deleteProduct(id:string){
    const params = {id};
    return this.httpClient.delete('https://localhost:7113/api/Product/DeleteProduct',{params})
  }
}
