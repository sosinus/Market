import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/api.service';
import { Item } from 'src/app/models/Item';
import { OrderItem } from 'src/app/models/orderItem';

@Component({
  selector: 'app-user-item-list',
  templateUrl: './user-item-list.component.html',
  styleUrls: ['./user-item-list.component.css']
})
export class UserItemListComponent implements OnInit {
  constructor(private apiService: ApiService) { }

  ngOnInit() {
    this.apiService.getItems();
  }

  addToCart(item: Item) {
    let orderItem = new OrderItem()
    orderItem = { item_Id: item.id, items_count: 1 , item: item, item_Price: item.price}
    var cartResult = this.apiService.cart.find(
      (elem) => {
        if (elem.item_Id == item.id) {
          elem.items_count = elem.items_count + 1; return true
        }
        else return false
      }
    )
    if (cartResult == undefined)
      this.apiService.cart.push(orderItem)
  }

}
