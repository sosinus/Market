import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ApiService } from 'src/app/api.service';
import { Item } from 'src/app/models/Item';
import { ItemEditComponent } from '../item-edit/item-edit.component';

@Component({
  selector: 'app-item-list',
  templateUrl: './item-list.component.html',
  styleUrls: ['./item-list.component.css']
})
export class ItemListComponent implements OnInit {
  @ViewChild('openBtn', { static: false }) openBtn: ElementRef;
  constructor(private apiService: ApiService) {

  }

  ngOnInit() {
    this.apiService.getItems();
  }
  onRowClick(item: Item) {
    this.apiService.editItem = item
    this.apiService.getImagesForEdit()
    this.openBtn.nativeElement.click()
  }

}
