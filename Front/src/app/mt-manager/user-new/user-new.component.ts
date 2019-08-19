import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { User } from 'src/app/models/user';
import { HttpClient } from '@angular/common/http';
import { Customer } from 'src/app/models/customer';
import { NgForm } from '@angular/forms';
import { CreateUserResult } from 'src/app/models/results';
import { ApiService } from 'src/app/api.service';

@Component({
  selector: 'app-user-new',
  templateUrl: './user-new.component.html',
  styles: []
})
export class UserNewComponent implements OnInit {
  user: User
  @ViewChild('closeBtn', { static: false })
  closeBtn: ElementRef
  constructor(private apiService: ApiService, private http: HttpClient) { }

  ngOnInit() {
    this.user = {
      address: "",
      email: "",
      customer: new Customer(),
      userName: "",
      password: ""
    }
  }

  onSubmit(form: NgForm) {
    if (form.valid) {
      this.http.post(this.apiService.apiURI + "User", form.value)      
        .toPromise()
        .then((res: CreateUserResult) => {
          if (res.success) {
            this.closeBtn.nativeElement.click()
            this.apiService.getUsers()
          }
        })
    }
  }

  cancel(form: NgForm){
    form.resetForm()
  }


}
