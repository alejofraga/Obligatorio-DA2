import { Component, OnInit } from "@angular/core";
import {
  FormGroup,
  FormControl,
  Validators,
  AbstractControl,
} from "@angular/forms";
import { DeviceService } from "../../../backend/services/device.service";
import { DevicesTypesService } from "../../../backend/services/devicestypes.service";
import { LoadingService } from "../../../backend/services/loading.service";

@Component({
  selector: "app-device-registration-form",
  templateUrl: "./device-registration-form.component.html",
  styles: [],
})
export class DeviceRegistrationFormComponent implements OnInit {
  registerForm!: FormGroup;
  photoUrls: string[] = [];
  devicesTypes: { value: string, label: string }[] = [];
  errorMessage: string = "";
  successMessage: string = "";

  readonly formField: any = {
    name: {
      name: "name",
      required: "name required",
      text: "name must be text",
      class: "form-control",
    },
    description: {
      name: "description",
      required: "description required",
      text: "description must be text",
      pattern: "description must be text only",
      class: "form-control",
    },
    modelNumber: {
      name: "modelNumber",
      required: "modelNumber required",
      class: "form-control",
    },
    photos: {
      name: "photos",
      required: "photos required",
      class: "form-control",
    },
    deviceType: {
      name: "Device Type",
      options: this.devicesTypes,
      selectedOption: this.devicesTypes.length > 0 ? this.devicesTypes[0].value : '',
      required: "Device type is required",
      class: "form-select",
    },
    isOutdoor: { name: "Outdoor", class: "form-check-input" },
    isIndoor: { name: "Indoor", class: "form-check-input" },
    HasPersonDetection: {
      name: "Has Person Detection",
      class: "form-check-input",
    },
    HasMovementDetection: {
      name: "Has Movement Detection",
      class: "form-check-input",
    },
  };

  constructor(
    private deviceService: DeviceService,
    private devicesTypesService: DevicesTypesService,
    private loadingService: LoadingService
  ) {}

  ngOnInit() {
    this.fetchDevicesTypes();
    this.registerForm = new FormGroup(
      {
        [this.formField.name.name]: new FormControl("", [
          Validators.required,
          Validators.pattern("^[a-zA-Z ]*$"),
        ]),
        [this.formField.description.name]: new FormControl("", [
          Validators.required,
          Validators.pattern("^[a-zA-Z ]*$"),
        ]),
        [this.formField.modelNumber.name]: new FormControl("", [
          Validators.required,
        ]),
        [this.formField.photos.name]: new FormControl([], [
          Validators.required,
        ]),
        [this.formField.deviceType.name]: new FormControl("", [
          Validators.required,
        ]),
        [this.formField.isOutdoor.name]: new FormControl(true),
        [this.formField.isIndoor.name]: new FormControl(false),
        [this.formField.HasPersonDetection.name]: new FormControl(false),
        [this.formField.HasMovementDetection.name]: new FormControl(false),
      },
      
      { validators: this.validateOutdoorIndoor }
    );
  }

  validateOutdoorIndoor(control: AbstractControl) {
    if (control.get("Device Type")?.value === "Camera") {
      const isOutdoor = control.get("Outdoor")?.value;
      const isIndoor = control.get("Indoor")?.value;
      return isOutdoor || isIndoor ? null : { outdoorIndoorRequired: true };
    }
    else {
      return null;
    }
  }

  onPhotoUrlsInput(event: any) {
    const input = event.target.value;
    this.photoUrls = input.split(',').map((url: string) => url.trim());
  }

  public onSubmit = (values: any) => {
    if (!this.registerForm.valid) {
      return;
    }

    const name = this.registerForm.get(this.formField.name.name)?.value;
    const description = this.registerForm.get(this.formField.description.name)
      ?.value;
    const modelNumber = this.registerForm.get(this.formField.modelNumber.name)
      ?.value;
    const isOutdoor = this.registerForm.get(this.formField.isOutdoor.name)?.value;
    const isIndoor = this.registerForm.get(this.formField.isIndoor.name)?.value;
    const deviceType = this.registerForm.get(this.formField.deviceType.name)
      ?.value;
    const hasPersonDetection = this.registerForm.get(
      this.formField.HasPersonDetection.name
    )?.value;
    const hasMovementDetection = this.registerForm.get(
      this.formField.HasMovementDetection.name
    )?.value;
    this.loadingService.show();

    this.deviceService
      .RegisterDevice(
        deviceType,
        name,
        description,
        modelNumber,
        this.photoUrls,
        isOutdoor,
        isIndoor,
        hasPersonDetection,
        hasMovementDetection
      )
      .subscribe({
        next: (result: any) => {
          this.registerForm.reset();
          this.loadingService.hide();
          this.successMessage = "Device registered successfully";
          this.errorMessage = "";
        },
        error: (error: any) => {
          this.errorMessage = error.details;
          this.successMessage = "";
          this.loadingService.hide();
        }
      });
  };

  fetchDevicesTypes = () => {
    this.loadingService.show();
    this.devicesTypesService.getDevicesTypes().subscribe({
      next: (response: any) => {
        let responseItems = response.data.map((deviceType: any) => ({
          value: deviceType.name,
          label: deviceType.name,
        }));
        this.devicesTypes = responseItems.reverse();
        this.registerForm.get(this.formField.deviceType.name)?.setValue(this.devicesTypes[0].value);
        this.formField.deviceType.options = this.devicesTypes;
        this.loadingService.hide();
      },
      error: (error: any) => {
        console.error("Error fetching validators:", error);
        this.loadingService.hide();
      },
    });
  }

  onSelectChange = (selectedOption: string) => {
    this.formField.deviceType.selectedOption = selectedOption;
    this.registerForm.get(this.formField.deviceType.name)?.setValue(selectedOption);
  }

  onChangeIndoorCheckbox = (isChecked: boolean) => {
    if (isChecked) {
      this.registerForm.get(this.formField.isIndoor.name)?.setValue(true);
    }
    else {
      this.registerForm.get(this.formField.isIndoor.name)?.setValue(false);
    }
  }

  registerClass = "btn btn-primary";
}