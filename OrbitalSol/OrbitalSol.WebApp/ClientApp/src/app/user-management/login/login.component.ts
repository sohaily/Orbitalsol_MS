import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup = new FormGroup({
    'login': new FormControl('', Validators.required),
    'password': new FormControl('', Validators.required)
   });
   date1:Date = new Date();
  submitted = false;
  
  constructor(){
      
  }
  ngOnInit() {
  }
  
  onSubmit() { 
      this.submitted = true;
      alert(JSON.stringify(this.loginForm.value));
  }
}