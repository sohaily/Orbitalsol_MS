import { Component, OnInit } from '@angular/core';
import { Product } from '../models/Product';
import { ProductService } from '../services/productservice';

@Component({
  selector: 'app-table-demo',
  templateUrl: './table-demo.component.html',
  styleUrls: ['./table-demo.component.scss'],
})
export class TableDemoComponent implements OnInit {
  products: Product[] = [];

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.productService
      .getProductsSmall()
      .then((data) => (this.products = data));
  }
}
