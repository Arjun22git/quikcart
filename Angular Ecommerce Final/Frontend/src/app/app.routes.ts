import { Routes } from '@angular/router';
import { HomeComponent } from './core/Home/home/home.component';
import { LoginComponent } from './authentication/components/login/login.component';
import { DashboardComponent } from './core/Admin/dashboard/dashboard.component';
import { VerifyotpComponent } from './authentication/components/verifyotp/verifyotp.component';
import { BodyComponent } from './core/body/body.component';
import { CartComponent } from './core/cart/cart.component';
import { ProductComponent } from './core/products/products.component';
import { CheckoutComponent } from './core/checkout/checkout.component';
import { PaymentComponent } from './core/payment/payment.component';
import { AdmintoolsComponent } from './core/Admin/admintools/admintools.component';
import { UsersComponent } from './core/Admin/users/users.component';
import { OrdersComponent } from './core/Admin/orders/orders.component';
import { SettingsComponent } from './core/Admin/settings/settings.component';
import { ReportsComponent } from './core/Admin/reports/reports.component';
import { AdminhomeComponent } from './core/Admin/adminhome/adminhome.component';
import { AdminproductsComponent } from './core/Admin/adminproducts/adminproducts.component';
import { SellerdashboardComponent } from './core/Seller/sellerdashboard/sellerdashboard.component';
import { SellerproductsComponent } from './core/Seller/sellerproducts/sellerproducts.component';
import { SellerOrdersComponent } from './core/Seller/seller-orders/seller-orders.component';
import { SellerhomeComponent } from './core/Seller/sellerhome/sellerhome.component';
import { AddproductComponent } from './core/Seller/addproduct/addproduct.component';
import { EditProductComponent } from './core/Seller/edit-product/edit-product.component';
import { authGuard } from './authentication/guards/auth.guard';

export const routes: Routes = [
    
    {path:'',redirectTo:'login',pathMatch:'full'},
    {path:'login',component:LoginComponent,},
    {path:'homepage',component:HomeComponent,},
    {path:'verifyotp',component:VerifyotpComponent},
    {path:'userview',component:BodyComponent, },
    {path:'cart',component:CartComponent},
    {path:'products',component:ProductComponent},
    {path:'checkout',component:CheckoutComponent, },
    {path:'payment',component:PaymentComponent, },
    //{path:'admindashboard',component:DashboardComponent},
    
    

    { 
        path: 'admindashboard', 
        component: DashboardComponent,
        children: [
            { path: 'adminhome', component: AdminhomeComponent },
            { path: 'adminproducts', component: AdminproductsComponent},
            { 
                path: 'tools', 
                component: AdmintoolsComponent,
                children: [
                    { path: 'orders', component: OrdersComponent},
                    { path: 'users', component: UsersComponent}
                ],
                
            },
            { path: 'settings', component: SettingsComponent },
            { path: 'reports', component: ReportsComponent },
            { path: '', redirectTo: 'admindashboard', pathMatch: 'full' } 
        ]
    },


    {
        path:'sellerdashboard',
        component:SellerdashboardComponent,
        children: [
            { path: 'sellerhome', component: SellerhomeComponent},
            { path: 'sellerorders', component: SellerOrdersComponent},
                { path: 'sellerproducts', component: SellerproductsComponent,
                    children:[
                    {path:'addproduct',component:AddproductComponent},
                    {path:'editproduct',component:EditProductComponent},
                ]
                },
            
            { path: '', redirectTo: 'admindashboard', pathMatch: 'full' } 
        ]

    }
    
];
