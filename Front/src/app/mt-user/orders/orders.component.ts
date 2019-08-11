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
        console.log(res)        
      },
      err=>
      console.log(err))           
  }

  getDate(date: string):string{
    return date.split('T')[0]
  }

}
