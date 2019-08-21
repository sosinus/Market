import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/api.service';
import { HttpClient } from '@angular/common/http';
import { Order } from 'src/app/models/order';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {
  orders: Order[]
  constructor(private apiService: ApiService, private http: HttpClient) { }

  ngOnInit() {
    this.http.get(this.apiService.apiURI + "order")
      .toPromise()
      .then((res: any) => {
        this.orders = res as Order[]
        this.orders.sort((a,b)=>{
          return b.id - a.id
        })
        console.log(res)        
      },
      err=>
      console.log(err))           
  }

  getDate(date: string): string {
    let result
    if(date.slice(0,2) == "00")
      result = "Не доступно"
    else
      result = date.split('T')[0]  
    return result
  }

  cancelOrder(order:Order){
    this.http.post(this.apiService.apiURI + "Order/cancel", order)
    .toPromise()
    .then(()=>this.ngOnInit())
  }
}
