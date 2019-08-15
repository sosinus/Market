import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/api.service';
import { HttpClient } from '@angular/common/http';
import { Order } from 'src/app/models/order';
import { Customer } from 'src/app/models/customer';
import { Item } from 'src/app/models/Item';
import { NgForm } from '@angular/forms';
import { OrderItem } from 'src/app/models/orderItem';
import { User } from 'src/app/models/user';

@Component({
  selector: 'app-order-list',
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.css']
})
export class OrderListComponent implements OnInit {
  orders: Order[]
  ordersCopy: Order[] = new Array<Order>()
  edit: boolean = false
  orderForEdit: Order = new Order()
  users: User[]
  items: Item[]
  searchItem: string
  private orderSnapShot: Order = new Order()
  constructor(private apiService: ApiService, private http: HttpClient) { }

  ngOnInit() {
    this.http.get(this.apiService.apiURI + "order/orderList")
      .toPromise()
      .then((res: any) => {
        this.orders = res as Order[]
        this.orders.sort((a, b) => {
          return b.id - a.id
        })
        this.ordersCopy = JSON.parse(JSON.stringify(this.orders))
      },
        err =>
          console.log(err))

    this.apiService.getUsers()
      .then(() => this.users = this.apiService.users.filter(u => u.customer.id > 0))

    this.apiService.getItems()
    this.orderForEdit = new Order()

  }

  getDate(date: string): string {
    return date.split('T')[0]
  }

  changeStatus(order: Order, status: string) {
    order.status = status
  }

  editOrder(order: Order) {
    this.orderForEdit = JSON.parse(JSON.stringify(order));
    console.log(this.orderForEdit)
  }

  isOrderEdit(id: number) {
    if (id == this.orderForEdit.id)
      return true
    else
      return false
  }

  deleteItem(order: Order, itemId: number) {
    order.orderItems = order.orderItems.filter((ord) => {
      if (ord.id != itemId)
        return true
    })
  }

  cancelEdit(order: Order) {
    Object.assign(order, this.orderForEdit)
    this.orderForEdit.id = null
  }

  onChange() {
    if (this.searchItem != null) {
      if (this.searchItem.length == 0)
        this.searchItem = null
      this.items = this.apiService.itemList.filter((item) => {
        if (item.name.includes(this.searchItem))
          return true
      })
    }

  }

  addOrderItem(order: Order, item: Item) {

      if (order.orderItems.filter(o => o.item.id == item.id).length>0)
        order.orderItems.find(o => o.item.id == item.id).items_count++
      else {
        let _item: OrderItem = {
          item_Id: item.id,
          item: item,
          item_Price: item.price,
          items_count: 1,
          order_id: order.id
        }
        order.orderItems.push(_item)
        this.searchItem = null
        this.items = null
      }

  }

  saveChanges(order: Order) {
    this.http.put(this.apiService.apiURI + 'Order', order)
      .toPromise()
      .then((res) => {
        this.orderForEdit.id = null
      },
        (err) => console.log(err)
      )
      .then(() => this.ngOnInit())

  }

  filterOrdersByStatus(filter: string) {
    if (filter == "All") this.orders = this.ordersCopy
    if (filter == "New") this.orders = this.ordersCopy.filter(o => o.status == "Новый")
    if (filter == "Processing") this.orders = this.ordersCopy.filter(o => o.status == "Выполняется")
    if (filter == "Complete") this.orders = this.ordersCopy.filter(o => o.status == "Выполнен")
    if (filter == "Canceled") this.orders = this.ordersCopy.filter(o => o.status == "Отменен")
  }

  filterOrdersByUser(user: User) {
    this.orders = this.ordersCopy.filter(o => o.customer_Id == user.customer.id)
  }

  increaseQuantity(orderItem: OrderItem) {
    orderItem.items_count++
  }

  reduceQuantity(orderItem: OrderItem) {
    orderItem.items_count--
  }
}
