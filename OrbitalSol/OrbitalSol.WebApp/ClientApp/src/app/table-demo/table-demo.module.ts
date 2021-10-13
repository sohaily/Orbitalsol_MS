import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TableDemoComponent } from './table-demo.component';
import { LazyLoadingComponent } from './lazy-loading/lazy-loading.component';

export const routes = [
  { path: '', component: TableDemoComponent, pathMatch: 'full' },
  {
    path: 'lazy-loading-table',
    component: LazyLoadingComponent,
    pathMatch: 'full',
  },
];
@NgModule({
  declarations: [TableDemoComponent, LazyLoadingComponent],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild(routes),
    SharedModule,
  ],
})
export class TableDemoModule {}
