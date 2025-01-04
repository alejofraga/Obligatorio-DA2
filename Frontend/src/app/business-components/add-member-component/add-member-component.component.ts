import { Component, Input, OnInit } from '@angular/core';
import { HomeService } from '../../../backend/services/home.service';
import { FormGroup } from '@angular/forms';
import { HomeManagementService } from '../../../backend/services/home-management.service';
import { FormControl, Validators } from '@angular/forms';
import { LoadingService } from '../../../backend/services/loading.service';

@Component({
  selector: 'app-add-member-component',
  templateUrl: './add-member-component.component.html',
  styles: ``
})
export class AddMemberComponentComponent implements OnInit {
  homeId!: string;
  errorMessage: string = "";
  successMessage: string = "";

  addMemberForm!: FormGroup;
  formField = {
    email: {
      name: "email",
      required: "Email is required",
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
    this.addMemberForm = new FormGroup({
      [this.formField.email.name]: new FormControl("", [
        Validators.required,
      ])
    })
  }

  public onSubmit = (values: any) => {
    if (!this.addMemberForm.valid) {
        return;
    }

    let memberData = {
      UserEmail: values[this.formField.email.name]
    }
    this.loadingService.show();
    this.homeService.addMember(this.homeId, memberData).subscribe({
        next: () => {
            this.addMemberForm.reset();
            this.loadingService.hide();
            this.errorMessage = "";
            this.successMessage = "Member added successfully";
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