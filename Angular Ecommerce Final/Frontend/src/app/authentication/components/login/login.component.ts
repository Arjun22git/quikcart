import { CommonModule } from '@angular/common';
import { Component, ElementRef, Injectable, Input, Output, ViewChild, output } from '@angular/core';
import { fromEvent } from 'rxjs';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import { Router, RouterLink, RouterModule, RouterOutlet } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { ToastrService } from 'ngx-toastr';
@Injectable({ providedIn: 'root' })

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule,ReactiveFormsModule,RouterOutlet,RouterLink,RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginDetails:any={};
  registerDetails:any={};
  data:any={};
  access:string='';
  refresh:string='';
  roles:string[]=[];
  tokens:object={};

  @Output() loggedIn :boolean=false;
  @Output() Username : string = '';

  loginform = new FormGroup({
    email : new FormControl(null, Validators.required),
    password : new FormControl(null,Validators.required),
    remember:  new FormControl(false)
  });

  registerform = new FormGroup({
    name : new FormControl(null, Validators.required),
    email : new FormControl(null, Validators.required),
    password : new FormControl(null,Validators.required),
    confirmPassword:  new FormControl(null,Validators.required)
  });

  constructor(private router:Router,private authenticate:AuthService,private toastr:ToastrService){}

  @ViewChild('loginbtn',{static:true}) loginbutton! :ElementRef;
  @ViewChild('registerbtn',{static:true}) registerbutton! :ElementRef;

  ngOnInit(){
    if(this.loggedIn)
    {
      this.router.navigateByUrl('/homepage')
    }
  }

  ngAfterViewInit() {
    this.login();
    this.register();
  }

  login(){ //login
    fromEvent(this.loginbutton.nativeElement,'click').subscribe(() => {
      if (this.loginform.valid) {
     

        this.loginDetails = this.loginform.value;
        console.log(this.loginDetails);

        this.authenticate.postlogin(this.loginDetails).subscribe({
          next: (data: any) => {
            console.log(data);
            this.access=data.accessToken;
            this.refresh=data.refreshToken;
            this.roles = data.roles;
            //set the data to local storage
            localStorage.setItem("accessToken",this.access);
            localStorage.setItem("refreshToken",this.refresh);
            localStorage.setItem("role",JSON.stringify(this.roles));
            this.loggedIn = true;
            this.Username = this.loginDetails.email;
            
            this.toastr.success("Success!","Logged in Successfully",{
              timeOut:3000,
              closeButton:true

            });

            //route to view based on role
            this.routeBasedOnRole(this.roles); 
            
          },
            error(err: any) {console.log(err)},
            complete() {console.log("Logged in")},
          });  
          this.loginform.reset({
            email: null,
            password: null,
            remember: false
          });
        }});

    }

    register(){  //register
      fromEvent(this.registerbutton.nativeElement,'click').subscribe(() => {
        if (this.registerform.valid) {
       
  
          this.registerDetails = this.registerform.value;
          console.log(this.registerDetails);
  
          this.authenticate.postregister(this.registerDetails).subscribe({
            next: (data: any) => {
              console.log(data);
              
              JSON.stringify(localStorage.setItem("user",this.registerDetails.email));
              JSON.stringify(localStorage.setItem("pass",this.registerDetails.password));

              this.toastr.info("Registered Succesfully!","Please Verfiy your account.",{
                timeOut:3000,
                closeButton:true
  
              });

              this.router.navigateByUrl('/verifyotp');
              
            },
              error(err: any) {console.log(err)},
              complete() {console.log("Registered")},
            });  
            this.registerform.reset();
          }});
  
      }

     
    routeBasedOnRole(roles: string[]) {
      if (roles.includes('Admin')) {
        JSON.stringify(localStorage.setItem("admin",this.Username));
        this.router.navigate(['/admindashboard']);
      } 
      else if (roles.includes('Customer')) {
        JSON.stringify(localStorage.setItem("user",this.Username));
        this.router.navigate(['/homepage']);
      } 
      else if (roles.includes('Seller')) {
        JSON.stringify(localStorage.setItem("seller",this.Username));
        this.router.navigate(['/sellerdashboard']);
      }
      
      else {
        
        console.log("No valid role found");
      }
    }

    getrole() {
      const storedRole = localStorage.getItem("roles");
      if (storedRole) {
        this.roles = JSON.parse(storedRole);
      } else {
        this.roles= [];
      }
    }

    isAuthenticated(){
      return this.loggedIn;
    }

}
