import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-vote',
  templateUrl: './vote.component.html'
})
export class VoteComponent {
  public cats: Cat[];

  constructor(http: HttpClient, @Inject('API_URL') baseUrl: string) {
    http.get<Cat[]>(baseUrl + 'api/cats/random').subscribe(result => {
      this.cats = result;
      console.log(this.cats);
    }, error => console.error(error));
  }
}
