import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ApiService } from './api.service';
import { LoadPageResult } from './models/results';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Front';
  hasDefaultUser : boolean = true
  constructor(private apiService:ApiService, private http:HttpClient) {
 }

 ngOnInit() {
  this.http.get(this.apiService.apiURI + "Auth")
    .toPromise()
    .then((res: LoadPageResult) => {
      this.hasDefaultUser = res.hasDefaultUser
    })
}

}
