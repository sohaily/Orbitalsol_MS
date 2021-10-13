import { Component, OnInit } from '@angular/core';
import { Customer } from './models/customer';
import { LazyLoadEvent } from 'primeng/api';
import { PrimeNGConfig } from 'primeng/api';
import { CustomerService } from './services/customerservice';

@Component({
  selector: 'app-lazy-loading',
  templateUrl: './lazy-loading.component.html',
  styleUrls: ['./lazy-loading.component.scss'],
})
export class LazyLoadingComponent implements OnInit {
  datasource: Customer[] = [];

  customers: Customer[] = [];

  totalRecords: number = 0;

  cols: any[] = [];

  loading: boolean = true;
  tepmFirstLastRow: number | undefined;

  constructor(
    private customerService: CustomerService,
    private primengConfig: PrimeNGConfig
  ) {}

  ngOnInit() {
    //datasource imitation
    this.customerService.getCustomersLarge().then((data) => {
      this.datasource = data;
      this.totalRecords = data.length;
    });

    this.loading = true;
    this.primengConfig.ripple = true;
  }

  loadCustomers(event: LazyLoadEvent) {
    this.loading = true;

    //in a real application, make a remote request to load data using state metadata from event
    //event.first = First row offset
    //event.rows = Number of rows per page
    //event.sortField = Field name to sort with
    //event.sortOrder = Sort order as number, 1 for asc and -1 for dec
    //filters: FilterMetadata object having field as key and filter value, filter matchMode as value

    //imitate db connection over a network
    setTimeout(() => {
      //this.tepmFirstLastRow = event.first //+ event.rows;
      //  this.tepmFirstLastRow = this.tepmFirstLastRow ? +10;

      if (this.datasource) {
        this.customers = this.datasource.slice(event.first, 20);
        this.loading = false;
      }
    }, 1000);
  }
}
