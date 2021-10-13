import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProjecTemplateComponent } from './shared-components/projec-template/projec-template.component';
import { AuthGuard } from './shared/core/auth.guard';
import { LazyLoadingComponent } from './table-demo/lazy-loading/lazy-loading.component';
import { TableDemoComponent } from './table-demo/table-demo.component';
import { TestComponent } from './test/test.component';

const routes: Routes = [
  {
    path: '',
    component: ProjecTemplateComponent,
    canActivate: [AuthGuard],
    canActivateChild: [AuthGuard],
    children: [
      {
        path: '',
        loadChildren: () =>
          import('./account-management/account-management.module').then(
            (m) => m.AccountManagementModule
          ),
        data: { breadcrumb: 'Dashboard' },
      },
      {
        path: 'table-sample',
        loadChildren: () =>
          import('./table-demo/table-demo.module').then(
            (m) => m.TableDemoModule
          ),
        data: { breadcrumb: 'Dashboard' },
      },
    ],
  },
  {
    path: 'login',
    loadChildren: () =>
      import('./user-management/user-management.module').then(
        (m) => m.UserManagementModule
      ),
  },
  { path: 'tests', component: TestComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
