import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { HttpEventType, HttpClient } from '@angular/common/http';
import { ApiService } from '../api.service';

@Component({
  selector: 'app-upload-image',
  templateUrl: './upload-image.component.html',
  styleUrls: ['./upload-image.component.css']
})
export class UploadImageComponent implements OnInit {
  public progress: number;
  public message: string;
  @Output() public onUploadFinished = new EventEmitter();

  constructor(private http: HttpClient, private apiService: ApiService) { }

  ngOnInit() {
  }

  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }
    const formData = new FormData();
    let imgs: string[] = new Array<string>()

    for (let file of files) {
      formData.append('file', file, file.name);
    }
    this.http.post(this.apiService.apiURI + 'images/upload', formData, { reportProgress: true, observe: 'events' })
      .subscribe(event => {
        if (event.type === HttpEventType.UploadProgress)
          this.progress = Math.round(100 * event.loaded / event.total);
        else if (event.type === HttpEventType.Response) {
          this.message = 'Upload success.';
          this.onUploadFinished.emit(event.body);
          let apiURI = this.apiService.apiURI.slice(0, -4)
          imgs = Object(event.body).imgPathes;
          imgs.forEach(img => {
            this.apiService.images.push(apiURI + img)
          });

        }

      });
  }


}