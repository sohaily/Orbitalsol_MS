import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';

const TOKEN_HEADER_KEY = 'Authorization';
@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private router:Router) {}
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>{
    if(localStorage.getItem('token')!=null){
      const cloneReq=req.clone({
        headers:req.headers.set('Authorization','Bearer '+localStorage.getItem('token'))
      })
      return next.handle(cloneReq).pipe(
        tap(
          succ=>{

          },
          err=>{
            if(err.status==401){
              localStorage.removeItem('token')
              this.router.navigateByUrl('/login')
            }
            else if(err.status==403){
                this.router.navigateByUrl('/forbidden');
              }
            console.log(err)
          }
        )
      )
    }
    else{
      return next.handle(req.clone());
    }
  }
}
export const authInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
];
