import { Component, OnInit, ViewChild, ElementRef, Output, EventEmitter } from '@angular/core';
import { ApiService } from 'src/app/api.service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { NgForm } from '@angular/forms';
import { GetJwtResult } from 'src/app/models/results';

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
  message: string
  constructor(private apiService: ApiService, private router: Router, private http: HttpClient) { }


  ngOnInit() {
    this.http.get(this.apiService.apiURI + "Auth")
      .toPromise()
      .then((res: any) => {
        this.hasDefaultUser = res.hasDefaultUser
      })
  }

  onSubmit(form: NgForm) {
    if (form.valid) {
      this.apiService.login(form.value)
        .toPromise()
        .then((res: GetJwtResult) => {
          if (res.success) {
            localStorage.setItem('token', res.token);
            this.closeModal.nativeElement.click()
            var payLoad = JSON.parse(window.atob(localStorage.getItem('token').split('.')[1]))
            var userRole = payLoad.role
            if (userRole == "Manager")
              this.router.navigateByUrl('manager-panel')
            this.onChanged.emit()
            form.resetForm()
            this.message = ""
            this.login = true
          }
          else
            this.message = "Неправильное имя пользователя или пароль"
        });
    }
  }

}
