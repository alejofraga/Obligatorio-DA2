import { Component } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { HomeService } from '../../../backend/services/home.service';
import { HomeManagementService } from '../../../backend/services/home-management.service';
import { FormControl, Validators } from '@angular/forms';
import { LoadingService } from '../../../backend/services/loading.service';

@Component({
  selector: 'app-add-hardware',
  templateUrl: './add-hardware.component.html',
  styles: ``
})
export class AddHardwareComponent {
  homeId!: string;
  errorMessage: string = "";
  successMessage: string = "";

  addHardwareForm!: FormGroup;
  formField = {
    modelNumber: {
      name: "modelNumber",
      required: "Model Number is required",
      class: "form-control",
    },
  };
  
  constructor(private homeService: HomeService, private homeManagementService: HomeManagementService, private loadingService : LoadingService) {}

  ngOnInit(): void {
    this.homeManagementService.homeId$.subscribe(id => {
      if (id) {
        this.homeId = id;
      }
    });
    this.addHardwareForm = new FormGroup({
      [this.formField.modelNumber.name]: new FormControl("", [
        Validators.required,
      ])
    })
  }

  public onSubmit = (values: any) => {
    if (!this.addHardwareForm.valid) {
        return;
    }

    let hardwareData = {
      ModelNumber: values[this.formField.modelNumber.name]
    }
    this.loadingService.show();
    this.homeService.addHardware(this.homeId, hardwareData).subscribe({
        next: () => {
            this.addHardwareForm.reset();
            this.loadingService.hide();
            this.successMessage = "Hardware added successfully";
            this.errorMessage = "";
        },
        error: (err: any) => {
            this.errorMessage = err.details;
            this.successMessage = "";
            this.loadingService.hide();
        }
    });
  }
  createClass = "btn btn-primary w-100 mt-3";

}
