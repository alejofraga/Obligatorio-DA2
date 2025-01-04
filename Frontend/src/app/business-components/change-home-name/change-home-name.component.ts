import { Component } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { HomeService } from "../../../backend/services/home.service";
import { HomeManagementService } from "../../../backend/services/home-management.service";
import { LoadingService } from "../../../backend/services/loading.service";

@Component({
  selector: "app-change-room-name",
  templateUrl: "./change-home-name.component.html",
  styles: ``,
})
export class ChangeHomeNameComponent {
  registerForm!: FormGroup;
  homeId: string = "";
  SuccessMessage: string = "";
  ErrorMessage: string = "";
  formField = {
    name: {
      name: "name",
      required: "name is required",
      pattern: "name must be text only",
      class: "form-control",
    },
  };

  constructor(
    private homeService: HomeService,
    private homeManagementService: HomeManagementService,
    private loadingService: LoadingService
  ) {}

  ngOnInit(): void {
    this.homeManagementService.homeId$.subscribe((id) => {
      if (id) {
        this.homeId = id;
      }
    });
    this.registerForm = new FormGroup({
      [this.formField.name.name]: new FormControl("", [
        Validators.required,
        Validators.pattern("^[a-zA-Z ]*$"),
      ]),
    });
  }
  public onSubmit = (values: any) => {
    if (this.registerForm.invalid) {
      this.ErrorMessage = "Please fill all the required fields";
      this.SuccessMessage = "";
      return;
    }
    this.loadingService.show();
    this.homeService.changeHomeName(this.homeId, values).subscribe({
      next: () => {
        this.SuccessMessage = "Home name changed successfully";
        this.ErrorMessage = "";
        this.loadingService.hide();
      },
      error: (error) => {
        this.SuccessMessage = "";
        this.ErrorMessage = error.details;
        this.loadingService.hide();
      }
    });
  };
}
