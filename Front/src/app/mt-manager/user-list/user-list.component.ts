import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ApiService } from 'src/app/api.service';
import { HttpClient } from '@angular/common/http';
import { User } from 'src/app/models/user';
import { Customer } from 'src/app/models/customer';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
    @ViewChild('openBtn', {static: false})
  openBtn: ElementRef
  constructor(private apiService: ApiService) { }

  ngOnInit() {
   this.apiService.getUsers()
  }
  onAddItemButton(){

  }
  
  onRowClick(user:User){    
    this.apiService.userForEdit = user
    this.openBtn.nativeElement.click()
  }

}
