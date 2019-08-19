import { Injectable } from '@angular/core';
import { Item } from './models/Item';
import { HttpClient, HttpHeaders, HttpRequest, HttpEventType, HttpParams, HttpResponse } from '@angular/common/http';
import { User } from './models/user';
import { async } from '@angular/core/testing';
import { promise } from 'protractor';
import { resolve } from 'q';
import { strictEqual } from 'assert';
import { Router } from '@angular/router';
import { OrderItem } from './models/orderItem';
import { Customer } from './models/customer';
import { flatMap } from 'rxjs/operators';
import { Order } from './models/order';
import { AddOrderResult } from './models/results';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  public logInText: string
  public alertMessage: string
  public item: Item
  public editItem: Item
  public itemList: Item[]
  public images: string[]
  public hideAlert: boolean
  public itemsCount: number
  public users: User[]
  public cart: OrderItem[]
  public cartItems: Item[]
  public currentCustomer: Customer
  public userForEdit: User
  public apiURI = "https://localhost:44369/api/"
  public forImagesApiURI = this.apiURI.slice(0, -4)

  constructor(private http: HttpClient, private router: Router) {

    this.item = new Item()
    this.editItem = new Item()
    this.hideAlert = true
    this.itemsCount = 0
    this.cart = new Array<OrderItem>()
    this.cartItems = new Array<Item>()
    this.currentCustomer = new Customer()
    this.userForEdit = {
      address: "",
      email: "",
      customer: new Customer(),
      userName: "",
      password: ""
    }
  }

  //--------User Managment----------------
  login(userFormData) {
    return this.http.post(this.apiURI + 'Auth/Login', userFormData);
  }
  register(userFormData) {
    return this.http.post(this.apiURI + 'Auth/CreateUser', userFormData)
  }


  logout() {
    localStorage.removeItem('token');
    this.item = new Item()
    this.editItem = new Item()
    this.hideAlert = true
    this.itemsCount = 0
    this.cart = new Array<OrderItem>()
    this.cartItems = new Array<Item>()
    this.currentCustomer = new Customer()
  }

  isUserLoggedIn(): boolean {
    if (localStorage.getItem('token'))
      return true
    else
      return false
  }

  roleMatch(roles: Array<string>): boolean {
    var isMatch = false
    var payLoad = JSON.parse(window.atob(localStorage.getItem('token').split('.')[1]))
    var userRole = payLoad.role
    roles.forEach(element => {
      if (userRole == element) {
        isMatch = true;
        return false;
      }
    });
    return isMatch;
  }

  postCustomer() {
    return this.http.post(this.apiURI + 'Auth/Customer', this.currentCustomer)
  }

  getCurrentCustomer() {
    return this.http.get(this.apiURI + "Auth/Customer")
  }

  applyOrder() {
    this.http.post(this.apiURI + "Order", this.cart).toPromise()
      .then((res: AddOrderResult) => {
        if (res.success)
          this.makeAlert("Ваш заказ успешно оформлен!")
        else
          this.makeAlert("Не удалось добавить заказ")
      },
        err =>
          this.makeAlert("Не удалось добавить заказ")
      )
    this.cart = new Array<OrderItem>()
  }

  getUsers() {
    return this.http.get(this.apiURI + "User")
      .toPromise()
      .then((res: any) => {
        this.users = res as User[]
        this.users.forEach(user => {
          if (!user.customer) user.customer = new Customer()
        });
      })
  }
  //--------Item Managment----------------
  postItem() {
    return this.http.post(this.apiURI + 'items', this.item)
  }

  getItems() {
    this.http.get(this.apiURI + 'items')
      .toPromise()
      .then((res: any) => { this.itemList = res as Item[] })
  }

  deleteItem() {
    let id: string = String(this.editItem.id)
    return this.http.delete(this.apiURI + 'items/' + id)
  }

  updateItem() {
    return this.http.put(this.apiURI + 'Items', this.editItem)
  }


  //--------Images Managment----------------
  deleteImages() {
    this.http.delete(this.apiURI + 'images/deleteAll').subscribe()
    this.images = new Array<string>()
  }

  deleteOneImage(image: string) {

    image = image.replace(/\\/g, '/')

    let splittedUrl = image.split('/')

    let imgName = splittedUrl[splittedUrl.length - 1]
    let imgPathes: string[]

    this.http.delete(this.apiURI + 'images/deleteOne/' + imgName)
      .toPromise()
      .then((res) => {
        imgPathes = Object(res).imgPathes
        imgPathes.forEach((img, index) => {
          imgPathes[index] = this.forImagesApiURI + img
        });
      })
      .then(() => this.images = imgPathes)
  }

  getImages() {
    this.images = new Array<string>()
    let imgPathes: string[]
    this.http.get(this.apiURI + 'images/' + this.editItem.id)
      .toPromise()
      .then((res) => {
        imgPathes = res as string[]
        if (imgPathes != null) {
          imgPathes.forEach((img, index) => {
            imgPathes[index] = this.forImagesApiURI + img
          })
        }
      })
      .then(() => this.images = imgPathes)
  }
  getImagesForEdit() {
    this.images = new Array<string>()
    let imgPathes: string[]
    this.http.get(this.apiURI + 'images/ForEdit/' + this.editItem.id)
      .toPromise()
      .then((res) => {
        imgPathes = res as string[]
        if (imgPathes != null) {
          imgPathes.forEach((img, index) => {
            imgPathes[index] = this.forImagesApiURI + img
          })
        }
      })
      .then(() => this.images = imgPathes)
  }

  //----------------Alerts--------------
  makeAlert(message: string) {
    this.alertMessage = message
  }

}
