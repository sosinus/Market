import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ApiService } from 'src/app/api.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  message: string
  constructor(private apiService: ApiService, private router: Router) { }
  @ViewChild('closeModal', { static: false })
  closeModal: ElementRef

  ngOnInit() {
  }
  onSubmit(form: NgForm) {
    this.message = ""

    if (form.valid) {
      this.apiService.register(form.value)
        .toPromise()
        .then((res: any) => {
          if (res.status == 200) {
            console.log(res.message)
            this.apiService.makeAlert("Вы успешно зарегистрировались!");
            console.log(form.value)
            this.apiService.login(form.value)
              .toPromise()
              .then((res: any) => {
                localStorage.setItem('token', res.token);
                this.closeModal.nativeElement.click()
                form.reset()
                this.apiService.logInText = "Выход"
              }
              )
          }
          if (res.status == 400) {
            this.message = res.message
          }
        })
    }
    else {
      if (form.value.Password) {
        if (form.value.Password.length < 4)
          this.message = 'Слишком короткий пароль'
      }
      else
        this.message = 'Введите пароль'

      if (!form.value.Email) {
        this.message = 'Введите email'
      }
      if (!form.value.UserName) {
        this.message = 'Введите логин'
      }
      else
      if(form.value.UserName.length<3)
      this.message = 'Слишком короткий логин'
     
    }


  }
}
