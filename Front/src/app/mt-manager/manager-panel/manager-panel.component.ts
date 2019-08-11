import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/api.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-manager-panel',
  templateUrl: './manager-panel.component.html',
  styleUrls: ['./manager-panel.component.css']
})
export class ManagerPanelComponent implements OnInit {

  constructor(private apiService:ApiService, private router: Router) { }

  ngOnInit() {
  }
  logout(){
    this.apiService.logout()
    this.router.navigateByUrl('')

  }
}
