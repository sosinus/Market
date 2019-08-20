import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api.service';

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.css']
})
export class AlertComponent implements OnInit {
 
  constructor(private apiService: ApiService) { }

  ngOnInit() {
    setInterval(()=>this.apiService.alertMessage = null, 10000)
  }

}
