import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthService } from '../../../backend/services/auth.service';
import { Router } from '@angular/router';
import { LoadingService } from '../../../backend/services/loading.service';
import { SharedService } from '../../../backend/services/shared.service';


@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styles: []
})
export class LoginFormComponent implements OnInit {
  loginForm!: FormGroup;
  errorMessage: string = '';

  readonly formField: any = {
    email: {
      name: 'email',
      required: 'Email es requerido',
      email: 'Email no es valido',
      class: "",
    },
    password: {
      name: 'password',
      required: 'Contraseña es requerida',
      minlength: 'Contraseña debe tener al menos 6 caracteres',
      class: "",
    },
  };

  constructor(
    private authService: AuthService,
    private router: Router,
    private loadingService : LoadingService,
    private sharedService: SharedService) {   }

  ngOnInit() {
    this.loginForm = new FormGroup({
      [this.formField.email.name]: new FormControl('', [
        Validators.required,
        Validators.email,
      ]),
      [this.formField.password.name]: new FormControl('', [
        Validators.required,
        Validators.minLength(6),
      ]),
    });
  }


  public onSubmit = (values: any) => {
    if (!this.loginForm.valid) {
        return;
    }

    const email = values[this.formField.email.name];
    const password = values[this.formField.password.name];
    this.loadingService.show();
    this.authService.login(email, password).subscribe({
        next: () => {
            this.loadingService.hide();
            this.router.navigate(['/']);
            this.sharedService.emitRoleUpdated();
            this.loginForm.reset();
            this.errorMessage = '';
        },
        error: (err: any) => {
            this.loadingService.hide();
            this.errorMessage = err.details;
        }
    });
}


  loginClass = "btn btn-primary";
  registerClass = "btn btn-primary";
  registerOption = () => {
    this.router.navigate(['/register-home-owner']);
  }
}