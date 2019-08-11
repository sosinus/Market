import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/api.service';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';


@Component({
  selector: 'app-register-manager',
  templateUrl: './register-manager.component.html',
  styleUrls: ['./register-manager.component.css']
})
export class RegisterManagerComponent implements OnInit {
  message :string
  constructor(private apiService: ApiService, private router:Router, private http: HttpClient) { }

  ngOnInit() {
  }

  onSubmit(form: NgForm){
    this.http.post(this.apiService.apiURI + "Auth/CreateManager", form.value)
    .subscribe((res:any)=>{      
      if(res.status == 200){
         console.log(res.message)
        form.reset()
        this.apiService.hideAlert = false
        //this.router.navigateByUrl('login')
        location.href = 'manager-panel'
      }     
     if(res.status == 400){
       this.message = res.message
     }
    })
  }

}
