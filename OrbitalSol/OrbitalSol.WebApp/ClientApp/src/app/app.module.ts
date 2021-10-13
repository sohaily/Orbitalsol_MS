import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ProjecTemplateComponent } from './shared-components/projec-template/projec-template.component';
import { TopMenuComponent } from './shared-components/projec-template/top-menu/top-menu.component';
import { AuthGuard } from './shared/core/auth.guard';
import { SharedModule } from './shared/shared.module';
import { TestComponent } from './test/test.component';
import { CustomerService } from './table-demo/lazy-loading/services/customerservice';

@NgModule({
  declarations: [
    AppComponent,
    ProjecTemplateComponent,
    TestComponent,
    TopMenuComponent,
  ],
  imports: [
    BrowserModule,
    CommonModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutingModule,
    SharedModule,
    HttpClientModule,
  ],
  providers: [CustomerService],
  bootstrap: [AppComponent],
})
export class AppModule {}
