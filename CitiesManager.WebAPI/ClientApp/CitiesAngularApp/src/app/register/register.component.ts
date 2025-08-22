import { Component } from '@angular/core';
import { AccountService } from '../services/account.service';
import { Router } from '@angular/router';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { RegisterUser } from '../models/register-user';
import { CompareValidation } from '../validators/custom-validators';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registerForm: FormGroup;
  isRegisterFormSubmitted: boolean = false;

  constructor(private accountService: AccountService, private router: Router) {
    this.registerForm = new FormGroup({
      personName: new FormControl(null, [Validators.required]),
      email: new FormControl(null, [Validators.required]),
      phone: new FormControl(null, [Validators.required]),
      password: new FormControl(null, [Validators.required]),
      confirmPassword: new FormControl(null, [Validators.required])
    },
      {
        validators: [CompareValidation("password", "confirmPassword")]
      });
  }

  get registerPersonNameControl(): AbstractControl {
    return this.registerForm.controls["personName"];
  }

  get registerEmailControl(): AbstractControl {
    return this.registerForm.controls["email"];
  }

  get registerPhoneControl(): AbstractControl {
    return this.registerForm.controls["phone"];
  }

  get registerPasswordControl(): AbstractControl {
    return this.registerForm.controls["password"];
  }

  get registerConfirmPasswordControl(): AbstractControl {
    return this.registerForm.controls["confirmPassword"];
  }

  public registerSubmitted() {
    this.isRegisterFormSubmitted = true;

    if (!this.registerForm.valid)
      return;

    this.accountService.postRegister(this.registerForm.value).subscribe({
      next: (response: any) => {
        console.log(response);

        this.isRegisterFormSubmitted = false;
        this.accountService.currentUserName = response.email;
        localStorage["token"] = response.token;
        localStorage["refreshToken"] = response.refreshToken;

        this.router.navigate(["/cities"]);

        this.registerForm.reset();
      },
      error: (err: any) => { console.log(err); },
      complete: () => { }
    });
  }
}
