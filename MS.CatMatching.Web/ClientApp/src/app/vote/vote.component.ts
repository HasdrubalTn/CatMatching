import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { routerNgProbeToken } from '@angular/router/src/router_module';
import { Router } from '@angular/router';

@Component({
  selector: 'app-vote',
  templateUrl: './vote.component.html'
})
export class VoteComponent implements OnInit {
  public cats: Cat[];
  public selectedCat: Cat;

  constructor(public http: HttpClient, public router: Router, @Inject('API_URL') public baseUrl: string) {

  }

  ngOnInit(): void {
    this.getRandomCats();
  }

  getRandomCats() {
    this.http.get<Cat[]>(this.baseUrl + 'api/cats/random').subscribe(result => {
      this.cats = result;
    }, error => console.error(error));
  }

  vote(cat: Cat) {
    this.http.post<Cat>(this.baseUrl + 'api/cats/vote', JSON.parse(cat.id.toString()))
    .subscribe(result => {
      console.log(`vote done for ` + cat.name);
      this.getRandomCats();
    }, error => console.error(error));
  }
}
