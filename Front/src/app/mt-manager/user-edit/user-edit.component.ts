import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ApiService } from 'src/app/api.service';
import { NgForm } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {
  @ViewChild('closeBtn', { static: false })
  closeBtn: ElementRef
  constructor(private apiService:ApiService, private http: HttpClient) { }

  ngOnInit() {
  }

  onDelete(){

  }

  onSubmit(form: NgForm){
    this.http.put(this.apiService.apiURI + 'User', this.apiService.userForEdit)
    .toPromise()
    .then((res:any)=>{
      
      
      if(res.succeeded){
        this.closeBtn.nativeElement.click()
      }
    })
  }
  
}
