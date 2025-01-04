import { Component, OnInit } from "@angular/core";
import { DevicesTypesService } from "../../../backend/services/devicestypes.service";
import { LoadingService } from "../../../backend/services/loading.service";

@Component({
  selector: "app-list-supported-devices",
  templateUrl: "./list-supported-devices.component.html",
  styles: ``,
})
export class ListSupportedDevicesComponent implements OnInit {
  dataObject: any = {};
  categories: any[] = [{ name: "name", alias: "Name" }];
  errorMessage: string = "";

  constructor(private deviceTypesService: DevicesTypesService, private loadingService : LoadingService) {}

  ngOnInit() {
    this.fetchData();
  }

  fetchData() {
    this.loadingService.show();
    this.deviceTypesService.getDevicesTypes().subscribe({
      next: (response) => {
        this.dataObject = { ...response, originalData: response.data };
        this.loadingService.hide();
        this.errorMessage = "";
      },
      error: (error: any) => {
        this.errorMessage =
          error.details || "An error occurred while fetching device types";
        this.loadingService.hide();
      },
    });
  }
}
