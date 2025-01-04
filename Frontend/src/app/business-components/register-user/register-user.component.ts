import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { UserService } from '../../../backend/services/user.service';
import { LoadingService } from '../../../backend/services/loading.service';

@Component({
  selector: 'app-register-user',
  templateUrl: './register-user.component.html',
})
export class RegisterUserComponent implements OnInit {
  registerForm!: FormGroup;
  SuccessMessage: string = '';
  ErrorMessage: string = '';

  role!: string;

  formField = {
    email: {
      name: "email",
      required: "email required",
      email: "email must be a valid email address",
      class: "form-control",
    },
    password: {
      name: "password",
      required: "password required",
      minlength: "password must be at least 6 characters",
      class: "form-control",
    },
    name: {
      name: "name",
      required: "name required",
      pattern: "name must contain only letters and spaces",
      class: "form-control",
    },
    lastname: {
      name: "lastname",
      required: "lastname required",
      pattern: "lastname must contain only letters and spaces",
      class: "form-control",
    },
  };

  constructor(
    private userService: UserService,
    private route: ActivatedRoute,
    private loadingService : LoadingService
  ) {}

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.role = data['role'] || this.role;
    });

    this.registerForm = new FormGroup({
      [this.formField.email.name]: new FormControl("", [
        Validators.required,
        Validators.email,
      ]),
      [this.formField.password.name]: new FormControl("", [
        Validators.required,
        Validators.minLength(6),
      ]),
      [this.formField.name.name]: new FormControl("", [
        Validators.required,
        Validators.pattern("^[a-zA-Z ]*$"),
      ]),
      [this.formField.lastname.name]: new FormControl("", [
        Validators.required,
        Validators.pattern("^[a-zA-Z ]*$"),
      ]),
    });
  }

  public onSubmit = (values: any) => {
    if (!this.registerForm.valid) {
      this.ErrorMessage = "Please fill the form correctly";
      this.SuccessMessage = "";
      return;
    }
    const email = values[this.formField.email.name];
    const password = values[this.formField.password.name];
    const lastname = values[this.formField.lastname.name];
    const name = values[this.formField.name.name];
    const role = this.role;
    this.loadingService.show();
    this.userService
      .register(name, lastname, email, password, role)
      .subscribe({
        next: () => {
          this.SuccessMessage = "User registered successfully";
          this.ErrorMessage = "";
          this.registerForm.reset();
          this.loadingService.hide();
        },
        error: (error: any) => {
          this.ErrorMessage = error.details;
          this.SuccessMessage = "";
          console.error("Error en el registro:", error);
          this.loadingService.hide();
        },
      });
  }

  registerClass = "btn btn-primary ";
}