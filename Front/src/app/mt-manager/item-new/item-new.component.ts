import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ApiService } from 'src/app/api.service';
import { ViewChild, ElementRef } from '@angular/core';
import { ItemResult } from 'src/app/models/results';




@Component({
  selector: 'app-item-new',
  templateUrl: './item-new.component.html',
  styleUrls: ['./item-new.component.css']
})

export class ItemNewComponent implements OnInit {
  @ViewChild('closeBtn', { static: false }) closeBtn: ElementRef;
  constructor(private apiService: ApiService) {
    this.apiService.images = new Array<string>() 
  }

  ngOnInit() {
  }


  async onSubmit(form: NgForm) {
    if (form.valid) {
      await this.apiService.postItem().
        toPromise()
        .then(
          (res: ItemResult) => {
            if (res.success) {
              this.closeBtn.nativeElement.click()
              this.apiService.getItems()
            }
            else
              this.apiService.makeAlert("Не удалось добавить товар")
          }
        )
    }
  }

  sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  onAddItemButton() {
    this.apiService.deleteImages()
  }
}
