import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ManagerPanelComponent } from './mt-manager/manager-panel/manager-panel.component';
import { ItemNewComponent } from './mt-manager/item-new/item-new.component';
import { ItemListComponent } from './mt-manager/item-list/item-list.component';
import { LoginComponent } from './auth/login/login.component';
import { ItemEditComponent } from './mt-manager/item-edit/item-edit.component';
import { AuthGuard } from './auth/auth.guard';
import { RegisterComponent } from './auth/register/register.component';
import { UserPanelComponent } from './mt-user/user-panel/user-panel.component';
import { UserItemListComponent } from './mt-user/user-item-list/user-item-list.component';
import { OrdersComponent } from './mt-user/orders/orders.component';
import { CartComponent } from './mt-user/cart/cart.component';
import { UserListComponent } from './mt-manager/user-list/user-list.component';
import { OrderListComponent } from './mt-manager/order-list/order-list.component';

const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  {
    path: 'manager-panel', component: ManagerPanelComponent, canActivate: [AuthGuard], data: { permittedRoles: ['Manager'] },
    children: [
      { path: "", redirectTo: 'item-list', pathMatch: 'full' },
      { path: 'user-list', component: UserListComponent },
      { path: 'new-item', component: ItemNewComponent },
      { path: 'edit-item', component: ItemEditComponent },
      { path: 'item-list', component: ItemListComponent },
      { path: 'order-list', component: OrderListComponent }
    ]    
  },
{
  path: '', component: UserPanelComponent,
    children: [
      { path: '', redirectTo: 'items', pathMatch: 'full' },
      { path: 'items', component: UserItemListComponent },
      { path: 'cart', component: CartComponent },
      { path: 'orders', component: OrdersComponent },
    ]
}

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
