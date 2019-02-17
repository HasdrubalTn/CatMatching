import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public cats: Cat[];

  constructor(http: HttpClient, @Inject('API_URL') baseUrl: string) {
    http.get<Cat[]>(baseUrl + 'api/cats/').subscribe(result => {
      this.cats = result;
      console.log(this.cats);
    }, error => console.error(error));
  }
}

interface Cat {
  Id: string;
  Name: number;
  Image: CatImage;
}

interface CatImage {
  Id: string;
  Url: string;
}
