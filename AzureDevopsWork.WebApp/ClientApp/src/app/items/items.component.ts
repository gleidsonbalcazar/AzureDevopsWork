import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Items } from "../models/items";

@Component({
  selector: 'app-item',
  templateUrl: './items.component.html'
})
export class ItemsComponent {
  public order = 'createdDate';
  public reverse: boolean = false;
  public items: Items[];
  itemFilter: any = { workItemType: '' };

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Items[]>(baseUrl + 'api/items').subscribe(result => {
      this.items = result;
    }, error => console.error(error));
  }

  ordenar(value: string) {
    if (this.order === value) {
      this.reverse = !this.reverse;
    }
    this.order = value;
  }
}


