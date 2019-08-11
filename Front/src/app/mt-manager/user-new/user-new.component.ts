import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';
import { HttpClient } from '@angular/common/http';
import { Customer } from 'src/app/models/customer';

@Component({
  selector: 'app-user-new',
  templateUrl: './user-new.component.html',
  styles: []
})
export class UserNewComponent implements OnInit {
  user: User
  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.user = {
      address: "",
      email: "",
      customer: new Customer(),
      userName: "",
      password: ""
    }
  }

}
