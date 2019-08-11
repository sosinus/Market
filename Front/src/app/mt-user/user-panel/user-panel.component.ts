import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ApiService } from 'src/app/api.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-panel',
  templateUrl: './user-panel.component.html',
  styleUrls: ['./user-panel.component.css']
})
export class UserPanelComponent implements OnInit {
  
  @ViewChild('openModal', { static: false })
  openModal: ElementRef;
  constructor(private apiService: ApiService, private router: Router) { }


  ngOnInit() {
    if (this.apiService.isUserLoggedIn())
      this.apiService.logInText = "Выход"
    else
      this.apiService.logInText = "Вход"
  }

  logInOut() {
    if (!this.apiService.isUserLoggedIn())
      this.openModal.nativeElement.click()
    else
      this.apiService.logout()
    this.ngOnInit()
  }

  logIn(){
    if (!this.apiService.isUserLoggedIn())
    this.openModal.nativeElement.click()
    else
    this.router.navigateByUrl('orders')
  }

  onChanged(){
    this.ngOnInit()
    
  }

}
