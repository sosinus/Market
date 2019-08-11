import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ApiService } from 'src/app/api.service';

@Component({
  selector: 'app-item-edit',
  templateUrl: './item-edit.component.html',
  styleUrls: ['./item-edit.component.css']
})
export class ItemEditComponent implements OnInit {
  @ViewChild('closeBtn', { static: false }) closeBtn: ElementRef;

  constructor(private apiService: ApiService) { }

  ngOnInit() {

  }
  async onSubmit(form: NgForm) {
    if (form.valid) {
       this.apiService.updateItem()
      .toPromise()
      .then(
        (res: any) => {
          console.log(res);
          this.closeBtn.nativeElement.click()
        }
      )
      .then(()=>this.apiService.getItems())
      
      
    }
  }

  onDelete() {
    this.apiService.deleteItem()
      .toPromise()
      .then(() => {
        this.closeBtn.nativeElement.click()
        this.apiService.getItems()
      })
  }

  del(image){
    this.apiService.images = this.apiService.images.filter(im=>im!=image)
  }


}
