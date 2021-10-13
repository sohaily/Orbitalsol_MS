import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
//import { JwtHelperService } from "@auth0/angular-jwt";
import { Observable, Subject } from 'rxjs';

export const TOKEN_NAME: string = 'jwt_token';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private subject = new Subject<string>();
  //helper = new JwtHelperService();
  private url: string = 'api/auth';
  private headers = new Headers({ 'Content-Type': 'application/json' });

  constructor(private http: HttpClient) { }

  getAuthorizationToken() {
    return 'some-auth-token';
  }
 

  setToken(token: string): void {
    localStorage.setItem("TOKEN_NAME", token);
  }
  getMessage(): Observable<string>{
    return this.subject.asObservable();
  }
  nextMessage(message: string){
    this.subject.next(message);
  }


  // getTokenExpirationDate(token: string): Date {
  //   const decoded = jwt_decode(token);

  //   if (decoded.exp === undefined) return null;

  //   const date = new Date(0); 
  //   date.setUTCSeconds(decoded.exp);
  //   return date;
  // }

  // isTokenExpired(token?: string): boolean {
  //   if(!token) token = this.getToken();
  //   if(!token) return true;

  //   const date = this.getTokenExpirationDate(token);
  //   if(date === undefined) return false;
  //   return !(date.valueOf() > new Date().valueOf());
  // }

  // login(user): Promise<string> {
  //   return this.http
  //     .post(`${this.url}/login`, JSON.stringify(user), { headers: this.headers })
  //     .toPromise()
  //     .then(res => res.text());
  // }


  // getUser():UserInfo {
  //   return this.user;
  // }

  isAuthenticated():boolean {
    //return this.helper.isTokenExpired(localStorage.getItem("token")) ?false:true;
    return localStorage.getItem("token") ?true:true;
  }
}
