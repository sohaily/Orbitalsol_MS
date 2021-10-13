import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { TrialBalanceComponent } from './trial-balance/trial-balance.component';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';


export const routes = [
  { path: '', component: TrialBalanceComponent, pathMatch: 'full' }
];
@NgModule({
  declarations: [
    TrialBalanceComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild(routes),
    SharedModule
  ]
})
export class AccountManagementModule { }
