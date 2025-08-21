import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators, AbstractControl } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from '../services/account.service';
import { LoginUser } from '../models/login-user';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm: FormGroup;
  isLoginFormSubmitted: boolean = false;

  constructor(private accountService: AccountService, private router: Router) {
    this.loginForm = new FormGroup({
      email: new FormControl(null, [Validators.required]),
      password: new FormControl(null, [Validators.required]),
    });
  }

  get loginEmailControl(): AbstractControl {
    return this.loginForm.controls["email"];
  }

  get loginPasswordControl(): AbstractControl {
    return this.loginForm.controls["password"];
  }

  public loginSubmitted() {
    this.isLoginFormSubmitted = true;

    if (!this.loginForm.valid)
      return;

    this.accountService.postLogin(this.loginForm.value).subscribe({
      next: (response: LoginUser) => {
        console.log(response);

        this.isLoginFormSubmitted = false;
        this.accountService.currentUserName = response.email;
        this.router.navigate(["/cities"]);

        this.loginForm.reset();
      },
      error: (err: any) => { console.log(err); },
      complete: () => { }
    });
  }
}
