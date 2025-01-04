import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { HomeOwnerService } from "../../../backend/services/homeOwner.service";
import { LoadingService } from "../../../backend/services/loading.service";
import { UserService } from "../../../backend/services/user.service";

@Component({
  selector: "app-register-homeowner-form",
  templateUrl: "./register-homeowner-form.component.html",
  styles: ``,
})
export class RegisterHomeownerFormComponent implements OnInit {
  registerForm!: FormGroup;
  successMessage: string = "";
  errorMessage: string = "";

  readonly formField: any = {
    name: {
      name: "name",
      required: "name required",
      text: "name must be text",
      class: "form-control",
    },
    lastname: {
      name: "lastname",
      required: "lastname required",
      text: "lastname must be text",
      class: "form-control",
    },
    email: {
      name: "email",
      required: "Email required",
      email: "Email is not valid",
      class: "form-control",
    },
    password: {
      name: "password",
      required: "password required",
      minlength: "password must be at least 6 characters",
      class: "form-control",
    },
    profilePicture: {
      name: "profilePicture",
      required: "profile picture required",
      class: "form-control",
    },
  };

  constructor(
    private homeOwnerService : HomeOwnerService, 
    private router: Router, private loadingService: LoadingService,
    private userService: UserService
  ) {}

  ngOnInit() {
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
      [this.formField.profilePicture.name]: new FormControl("", []),
    });
  }
  public onSubmit = (values: any) => {
    if (!this.registerForm.valid) {
      return;
    }
    const email = values[this.formField.email.name];
    const password = values[this.formField.password.name];
    const lastname = values[this.formField.lastname.name];
    const name = values[this.formField.name.name];
    let profilePicture = values[this.formField.profilePicture.name];
    if (!profilePicture){
      profilePicture = this.userService.getDefaultProfilePicture();
    }
    
    console.log(profilePicture);
    this.loadingService.show();
    this.homeOwnerService
      .register(name, lastname, email, password, profilePicture)
      .subscribe({
        next: () => {
          this.loadingService.hide();
          this.registerForm.reset();
          this.errorMessage = "";
          this.successMessage = "User registered successfully";
          this.router.navigate(["/login"]);
        },
        error: (err: any) => {
          this.loadingService.hide();
          this.errorMessage = err.details;
        },
      });
  };

  loginClass = "btn btn-primary";
  registerClass = "btn btn-primary";
  loginOption = () => {
    this.router.navigate(["/login"]);
  };
}
