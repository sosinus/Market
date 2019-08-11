import { Component, OnInit, ViewChild, ElementRef, Output, EventEmitter } from '@angular/core';
import { ApiService } from 'src/app/api.service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-login-modal',
  templateUrl: './login-modal.component.html',
  styleUrls: ['./login-modal.component.css']
})
export class LoginModalComponent implements OnInit {
  hasDefaultUser: boolean = true
  login: boolean = true
  @Output() onChanged = new EventEmitter();
  @ViewChild('closeModal', { static: false })
  closeModal: ElementRef
  constructor(private apiService: ApiService, private router: Router, private http: HttpClient) { }


  ngOnInit() {
    this.http.get(this.apiService.apiURI + "Auth")
      .toPromise()
      .then((res: any) => {
        this.hasDefaultUser = res.hasDefaultUser
      })
  }

  onSubmit(form: NgForm) {
    this.apiService.login(form.value)
      .toPromise()
      .then((res: any) => {
        localStorage.setItem('token', res.token);
        this.closeModal.nativeElement.click()
        var payLoad = JSON.parse(window.atob(localStorage.getItem('token').split('.')[1]))
        var userRole = payLoad.role
        if (userRole == "Manager")
          this.router.navigateByUrl('manager-panel')
        this.onChanged.emit()
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
