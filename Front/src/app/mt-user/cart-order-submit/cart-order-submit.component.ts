import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ApiService } from 'src/app/api.service';

@Component({
  selector: 'app-cart-order-submit',
  templateUrl: './cart-order-submit.component.html',
  styleUrls: ['./cart-order-submit.component.css']
})
export class CartOrderSubmitComponent implements OnInit {
  @ViewChild('closeModal', { static: false })
  closeModal: ElementRef;

  constructor(private apiService: ApiService) { }

  ngOnInit() {

  }

  onSubmit() {
    this.apiService.postCustomer()
      .toPromise()
      .then(res => {
        this.closeModal.nativeElement.click()
      },
        err => {

        }
      )
  }

  getCustomer() {
    this.apiService.getCurrentCustomer()

  }
}
