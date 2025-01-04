import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { CompanyService } from '../../../backend/services/company.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CompanyStateService } from '../../../backend/services/companyState.service';
import { LoadingService } from '../../../backend/services/loading.service';

@Component({
  selector: 'app-create-company-form',
  templateUrl: './create-company-form.component.html',
})
export class CreateCompanyFormComponent implements OnInit {
  validators: { value: string, label: string }[] = [];
  createCompanyForm!: FormGroup;
  errorMessage: string = "";
  successMessage: string = "";
  formField = {
    name: {
      name: "name",
      required: "Name is required",
      class: "form-control",
    },
    rut: {
      name: "rut",
      required: "Rut is required",
      pattern: "Rut must contain only numbers",
      minlength: "Rut must be exactly 12 characters long",
      maxlength: "Rut must be exactly 12 characters long",
      class: "form-control",
    },
    logoUrl: {
      name: "logoUrl",
      required: "Logo URL is required",
      class: "form-control",
    },
    ModelValidators: {
      name: "Model Validators",
      options: this.validators,
      selectedValue: this.validators.length > 0 ? this.validators[0].value : '',
      required: "Validator is required",
      class: "form-select ",
    }
  };

  onSelectChange(selectedValue: string) {
    this.formField.ModelValidators.selectedValue = selectedValue;
    this.createCompanyForm.get(this.formField.ModelValidators.name)?.setValue(selectedValue);
  }

  constructor(
    private companyService: CompanyService,
    private companyStateService : CompanyStateService,
    private route: ActivatedRoute,
    private router: Router,
    private loadingService : LoadingService
  ) {}

  ngOnInit() {
    this.fetchValidators();
    this.createCompanyForm = new FormGroup({
      [this.formField.name.name]: new FormControl("", [
        Validators.required,
      ]),
      [this.formField.rut.name]: new FormControl("", [
        Validators.required,
        Validators.pattern("^[0-9]*$"),
        Validators.minLength(12),
        Validators.maxLength(12),
      ]),
      [this.formField.logoUrl.name]: new FormControl("", [
        Validators.required,
      ]),
      [this.formField.ModelValidators.name]: new FormControl("", [
        Validators.required,
      ]),
    });
  }

  public onSubmit = (values: any) => {
    if (this.createCompanyForm.valid) {
      this.loadingService.show();
      this.companyService.createCompany(
        this.createCompanyForm.value.name,
        this.createCompanyForm.value.rut,
        this.createCompanyForm.value.logoUrl,
        this.formField.ModelValidators.selectedValue
      ).subscribe({
        next: () => {
          this.companyStateService.setHasCompany(true);
          this.router.navigate(["/company-owner/device-registration"]);
          this.loadingService.hide();
          this.successMessage = "Company created successfully";
          this.errorMessage = "";
        },
        error: (error: any) => {
          this.errorMessage = error.details;
          this.successMessage = "";
          this.loadingService.hide();
        },
      });
    }
  }

  fetchValidators = () => {
    this.loadingService.show();
    this.companyService.getValidators().subscribe({
      next: (response: any) => {
        this.loadingService.hide();
        this.validators = response.data.map((validator: string) => ({
          value: validator,
          label: validator
        }));
        this.formField.ModelValidators.options = this.validators;
        if (this.validators.length > 0) {
          this.formField.ModelValidators.selectedValue = this.validators[0].value;
          this.createCompanyForm.get(this.formField.ModelValidators.name)?.setValue(this.validators[0].value);
        }
      },
      error: (error: any) => {
        console.error("Error fetching validators:", error);
        this.loadingService.hide();
      },
    });
  }

  createClass = "btn btn-primary w-100";
}