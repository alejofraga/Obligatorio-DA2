import { Component } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { HomeService } from "../../../backend/services/home.service";
import { HomeManagementService } from "../../../backend/services/home-management.service";
import { HardwareService } from "../../../backend/services/hardware.service";
import { LoadingService } from "../../../backend/services/loading.service";

@Component({
  selector: "app-change-hardware-name",
  templateUrl: "./change-hardware-name.component.html",
  styles: ``,
})
export class ChangeHardwareNameComponent {
  registerForm!: FormGroup;
  hardwares: any;
  hardwareOptions: { value: string; label: string }[] = [];
  homeId: string = "";
  SuccessMessage: string = "";
  ErrorMessage: string = "";
  readonly formField: any = {
    name: {
      name: "name",
      required: "name required",
      text: "name must be text",
      pattern: "name must be text only",
      class: "form-control",
    },
    hardwares: {
      name: "hardwares",
      required: "hardwares required",
      class: "form-control",
    },
  };

  constructor(
    private hardwareService: HardwareService,
    private homeService : HomeService,
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
      [this.formField.hardwares.name]: new FormControl("", [
      ]),
    });
    this.loadHardwares();
  }
  onHardwareChange(selectedValues: string[]) {
    this.registerForm.controls['hardwares'].setValue(selectedValues);
  }

  loadHardwares() {
    this.loadingService.show();
    this.homeService.getHardwares(this.homeId).subscribe((data) => {
      this.loadingService.hide();
      this.hardwares = data.data;
      this.hardwareOptions = this.hardwares.map((hardware: any) => ({
        value: hardware.id,
        label: `${hardware.name} - ${hardware.id}`,
      }));
      this.registerForm
        .get(this.formField.hardwares.name)
        ?.setValue(this.hardwareOptions[0].value);

    });
  }
  public onSubmit = (values: any) => {
    if (this.registerForm.invalid) {
      this.ErrorMessage = "Please fill all the required fields";
      this.SuccessMessage = "";
      return;
    }

    const hardwareId = values.hardwares;
    const name = values.name;
    console.log(values);
    console.log(name);
    console.log(hardwareId);
    this.loadingService.show();
    this.hardwareService.changeHardwarename(hardwareId, {Name : name}).subscribe({
      next: (data) => {
      this.loadingService.hide();
      this.SuccessMessage = "Device name changed successfully";
      this.ErrorMessage = "";
      this.loadHardwares();
      },
      error: (error) => {
      this.loadingService.hide();
      this.SuccessMessage = "";
      this.ErrorMessage = error.details;
      }
    });
  };
}
