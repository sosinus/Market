import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ApiService } from 'src/app/api.service';
import { Item } from 'src/app/models/Item';
import { element } from 'protractor';
import { ViewChild, ElementRef } from '@angular/core';
import { ItemListComponent } from '../item-list/item-list.component';




@Component({
  selector: 'app-item-new',
  templateUrl: './item-new.component.html',
  styleUrls: ['./item-new.component.css']
})

export class ItemNewComponent implements OnInit {
  @ViewChild('closeBtn', { static: false }) closeBtn: ElementRef;
  constructor(private apiService: ApiService) {
    this.apiService.images = new Array<string>()    
   // this.apiService.images.push("https://localhost:44369/Resources/Images/Temp/e7e85af4-ae4f-4579-b612-10b8b324fdf8/Vh7oLhOuS-8.jpg")
   }

  ngOnInit() {    
  }

 
  async onSubmit(form: NgForm) {
    if (form.valid) {
     await this.apiService.postItem().subscribe(
        (res: any) => {
          console.log(res);
          this.closeBtn.nativeElement.click()
        }
      )
      await this.sleep(100)
      this.apiService.item = new Item()
      this.apiService.getItems()
    }
  }

  sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  onAddItemButton(){
    this.apiService.deleteImages()    
  }
}
