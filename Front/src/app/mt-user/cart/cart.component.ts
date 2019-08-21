import { Component, OnInit, Directive, Input, ViewChild, ElementRef } from '@angular/core';
import { ApiService } from 'src/app/api.service';
import { Item } from 'src/app/models/Item';
import { OrderItem } from 'src/app/models/orderItem';


@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  @ViewChild('openModal', { static: false })
  openModal: ElementRef;
  @ViewChild('openLoginModal', { static: false })
  openLoginModal: ElementRef;
  discount: number = 0
  constructor(private apiService: ApiService) { }

  async ngOnInit() {
    this.discount = await this.apiService.getDiscount()   
  }

  countOfItems() {
    let count: number = 0
    this.apiService.cart.forEach(element => {
      count = count + element.items_count
    });
    return count
  }

  totalPrice() {
    let total: number = 0
    this.apiService.cart.forEach(element => {
      total = total + element.item.price * element.items_count
    });
    return total
  }

  priceWithDiscount(price : number){
    var priceWithDiscount = price - (price * this.discount)/100
    return priceWithDiscount
  }

  onMakeOrder() {
    if(this.apiService.isUserLoggedIn()){
      this.apiService.getCurrentCustomer()
      .toPromise()
      .then((res: any) => {
        if (res != null)
          this.apiService.applyOrder()
        else
          this.openModal.nativeElement.click()
      },
        err => {
          this.openModal.nativeElement.click()
        })
    }
    else
    this.openLoginModal.nativeElement.click()
    
  }

  reduceQuantity(orderItem: OrderItem) {
    this.apiService.cart.forEach(
      (ordItm) => {
        if (ordItm.item_Id == orderItem.item_Id && ordItm.items_count > 1) {
          ordItm.items_count = ordItm.items_count - 1
        }
      })
  }

  increaseQuantity(orderItem: OrderItem) {
    this.apiService.cart.forEach(
      (ordItm) => {
        if (ordItm.item_Id == orderItem.item_Id) {
          ordItm.items_count = ordItm.items_count + 1
        }
      })

  }

}
