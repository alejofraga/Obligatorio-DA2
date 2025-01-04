import { Component, OnInit } from "@angular/core";
import {
  FormGroup,
  FormBuilder,
  FormControl,
  Validators,
} from "@angular/forms";
import { ImportService } from "../../../backend/services/import.service";
import { ActivatedRoute } from "@angular/router";
import "../../../backend/models/parameter";
import { LoadingService } from "../../../backend/services/loading.service";

@Component({
  selector: "app-importer-menu",
  templateUrl: "./importer-menu.component.html",
  styles: [],
})
export class ImporterMenuComponent implements OnInit {
  parameters: parameter[] = [];
  importerForm!: FormGroup;
  name: string = "";
  SuccessMessage: string = "";
  ErrorMessage: string = "";
  constructor(
    private importService: ImportService,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private loadingService : LoadingService
  ) {}

  ngOnInit() {
    this.loadingService.show();
    this.route.paramMap.subscribe((params) => {
      const name = params.get("name");
      if (name) {
        this.name = name;
      }
    });
    this.importService.updateImporterParams(this.name).then(() => {
      this.parameters = this.importService.getCurrentImporterParams();
      this.createForm();
      this.loadingService.hide();
    });
  }

  createForm() {
    const formGroup: any = {};
    this.parameters.forEach((param) => {
      formGroup[param.name] = new FormControl("");
    });
    this.importerForm = this.fb.group(formGroup);
  }

  onSubmit(input: any) {
    if (this.importerForm.valid) {
      this.loadingService.show();
      if (!this.importerForm.value) {
        this.ErrorMessage = "Please fill the form correctly";
        this.SuccessMessage = "";
        return;
      }
      this.importService
        .ImportDevices(this.name, { Params: this.importerForm.value })
        .subscribe({
          next: (response) => {
            this.loadingService.hide();
            this.SuccessMessage = "Devices imported successfully";
            this.ErrorMessage = "";
          },
          error: (err) => {
            this.loadingService.hide();
            this.ErrorMessage = "Please check that the data is correct";
            this.SuccessMessage = "";
          },
        });
    } else {
      this.ErrorMessage = "Please fill the form correctly";
      this.SuccessMessage = "";
      return;
    }
  }
}
