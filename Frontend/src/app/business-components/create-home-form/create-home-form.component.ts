import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import {
  MeHomeService,
} from "../../../backend/services/me.home.service";
import { LoadingService } from "../../../backend/services/loading.service";

@Component({
  selector: "app-create-home-form",
  templateUrl: "./create-home-form.component.html",
  styles: ``,
})
export class CreateHomeFormComponent implements OnInit {
  homeCreation!: FormGroup;
  successMessage: string = "";
  errorMessage: string = "";

  readonly formField: any = {
    name: {
      name: "name",
      text: "name must be text",
      class: "form-control",
    },
    address: {
      name: "address",
      required: "address required",
      text: "address must be text",
      pattern: "addres must be text only",
      class: "form-control",
      minlength: "address must be at least 6 characters long",
    },
    doorNumber: {
      name: "doorNumber",
      required: "doorNumber required",
      min: "doorNumber must be a positive number",
      class: "form-control",
    },
    latitude: {
      name: "latitude",
      required: "latitude required",
      min: "latitude must be at least -90",
      max: "latitude must be at most 90",
      class: "form-control",
    },
    longitude: {
      name: "longitude",
      required: "longitude required",
      min: "longitude must be at least -90",
      max: "longitude must be at most 90",
      class: "form-control",
    },
    memberCount: {
      name: "memberCount",
      required: "memberCount required",
      min: "memberCount must be a positive number",
      class: "form-control",
    },
  };

  constructor(private meHomeService: MeHomeService, private router: Router, private loadingService: LoadingService) {}

  ngOnInit() {
    this.homeCreation = new FormGroup({
      [this.formField.address.name]: new FormControl("", [
        Validators.required,
        Validators.pattern("^[a-zA-Z ]*$"),
        Validators.minLength(6),
      ]),
      [this.formField.doorNumber.name]: new FormControl("", [
        Validators.required,
        Validators.min(0),
      ]),
      [this.formField.name.name]: new FormControl("", [
        Validators.pattern("^[a-zA-Z ]*$"),
      ]),
      [this.formField.latitude.name]: new FormControl("", [
        Validators.required,
        Validators.min(-90),
        Validators.max(90),
      ]),
      [this.formField.longitude.name]: new FormControl("", [
        Validators.required,
        Validators.min(-90),
        Validators.max(90),
      ]),
      [this.formField.memberCount.name]: new FormControl("", [
        Validators.required,
        Validators.min(0),
      ]),
    });
  }
  homeCreationClass = "btn btn-primary w-100 mt-3";

  public onSubmit = (values: any) => {
    if (!this.homeCreation.valid) {
      return;
    }
    const { name, address, doorNumber, latitude, longitude, memberCount } =
      values;
    name.trim();
    this.loadingService.show();
    this.meHomeService
      .createHome(name, address, doorNumber, latitude, longitude, memberCount)
      .subscribe({
        next: () => {
          this.homeCreation.reset();
          this.loadingService.hide();
          this.errorMessage = "";
          this.successMessage = "Home created successfully";
        },
        error: (err: any) => {
          this.loadingService.hide();
          this.errorMessage = err.details
          this.successMessage = "";
        },
      });
  };
}
