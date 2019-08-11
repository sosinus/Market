import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ManagerPanelComponent } from './mt-manager/manager-panel/manager-panel.component';
import { ItemListComponent } from './mt-manager/item-list/item-list.component';
import { ItemNewComponent } from './mt-manager/item-new/item-new.component';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { UploadImageComponent } from './upload-image/upload-image.component';
import { LoginComponent } from './auth/login/login.component';
import { ApiService } from './api.service';
import { AuthInterceptor } from './auth/auth.interceptor';
import { ItemEditComponent } from './mt-manager/item-edit/item-edit.component';
import { RegisterComponent } from './auth/register/register.component';
import { UserPanelComponent } from './mt-user/user-panel/user-panel.component';
import { UserItemListComponent } from './mt-user/user-item-list/user-item-list.component';
import { OrdersComponent } from './mt-user/orders/orders.component';
import { CartComponent } from './mt-user/cart/cart.component';
import { CartOrderSubmitComponent } from './mt-user/cart-order-submit/cart-order-submit.component';
import { RegisterManagerComponent } from './auth/register-manager/register-manager.component';
import { AlertComponent } from './alert/alert.component';
import { LoginModalComponent } from './auth/login-modal/login-modal.component';
import { UserListComponent } from './mt-manager/user-list/user-list.component';
import { UserEditComponent } from './mt-manager/user-edit/user-edit.component';
import { UserNewComponent } from './mt-manager/user-new/user-new.component';
import { OrderListComponent } from './mt-manager/order-list/order-list.component';



@NgModule({
  declarations: [
    AppComponent,
    ManagerPanelComponent,
    ItemListComponent, 
    ItemNewComponent, 
    UploadImageComponent,
    LoginComponent, 
    ItemEditComponent, 
    RegisterComponent, 
    UserPanelComponent, 
    UserItemListComponent, 
    OrdersComponent, 
    CartComponent, 
    CartOrderSubmitComponent, RegisterManagerComponent, AlertComponent, LoginModalComponent, UserListComponent, UserEditComponent, UserNewComponent, OrderListComponent   
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    NgbModule
   
  ],
  providers: [ApiService,{
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule { }
