import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiService } from 'src/app/api.service';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  hasDefaultUser: boolean = true
  constructor(private apiService: ApiService, private router: Router, private http: HttpClient) { }

  ngOnInit() {
    this.http.get(this.apiService.apiURI + "Auth")
      .toPromise()
      .then((res: any) => {
        this.hasDefaultUser = res.hasDefaultUser
      })
  }

  onSubmit(form: NgForm) {
    this.apiService.login(form.value).subscribe(
      (res: any) => {
        localStorage.setItem('token', res.token);
        var payLoad = JSON.parse(window.atob(localStorage.getItem('token').split('.')[1]));
        let userRole: String = payLoad.role;
        this.router.navigateByUrl(userRole.toLowerCase() + '-panel')
      },
      err => {
        if (err.status == 400)
          console.log('Неправильное имя пользователя или пароль');
        else
          console.log(err);
      }
    );
  }



}
