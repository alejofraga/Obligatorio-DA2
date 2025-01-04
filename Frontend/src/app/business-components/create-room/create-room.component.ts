import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { HomeService } from "../../../backend/services/home.service";
import { HomeManagementService } from "../../../backend/services/home-management.service";
import { LoadingService } from "../../../backend/services/loading.service";

@Component({
  selector: "app-create-room",
  templateUrl: "./create-room.component.html",
  styles: [],
})
export class CreateRoomComponent implements OnInit {
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
      class: "form-control",
    },
  };

  constructor(
    private homeService: HomeService,
    private homeManagementService: HomeManagementService,
    private loadingService: LoadingService
  ) {}

  ngOnInit() {
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

  loadHardwares() {
    this.loadingService.show();
    this.homeService.getHardwares(this.homeId).subscribe((data) => {
      this.hardwares = data.data.filter((hardware: any) => !hardware.isInARoom);
      this.hardwareOptions = this.hardwares.map((hardware: any) => ({
        value: hardware.id,
        label: `${hardware.name} - ${hardware.id}`,
      }));
      this.loadingService.hide();
    });
  }

  onHardwareChange(selectedValues: string[]) {
    this.registerForm.controls["hardwares"].setValue(selectedValues);
  }

  public onSubmit = (values: any) => {
    if (
      !values.hardwares ||
      values.hardwares.length === 0 ||
      !Array.isArray(values.hardwares)
    ) {
      this.SuccessMessage = "";
      this.ErrorMessage = "Please select at least one hardware";
      return;
    }

    const name = values[this.formField.name.name];
    const selectedHardwares = values.hardwares;
    this.loadingService.show();
    this.homeService
      .addRoom(this.homeId, { Name: name, HardwareIds: selectedHardwares })
      .subscribe({
        next: () => {
          this.SuccessMessage = "Room added successfully";
          this.ErrorMessage = "";
          this.registerForm.reset();
          this.removeSelectedHardwares(selectedHardwares);
          this.loadingService.hide();
        },
        error: (err: any) => {
          this.SuccessMessage = "";
          this.ErrorMessage = err.details;
          this.loadingService.hide();
        },
      });
  };

  removeSelectedHardwares(selectedHardwares: string[]) {
    this.hardwares = this.hardwares.filter(
      (hardware: any) => !selectedHardwares.includes(hardware.id)
    );
    this.hardwareOptions = this.hardwares.map((hardware: any) => ({
      value: hardware.id,
      label: `${hardware.name} - ${hardware.id}`,
    }));
  }
}
