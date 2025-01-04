import { Component, OnInit } from "@angular/core";
import { DeviceService } from "../../../backend/services/device.service";
import { LoadingService } from "../../../backend/services/loading.service";

@Component({
  selector: "app-list-devices",
  templateUrl: "./list-devices.component.html",
  styles: ``,
})
export class ListDevicesComponent implements OnInit {
  dataObject: any = {};
  categories: any[] = [
    { name: "name", alias: "Name" },
    { name: "companyName", alias: "Company name" },
    { name: "modelNumber", alias: "Model number" },
    {name : "deviceType", alias: "Device type"},
    { name: "mainPhoto", alias: "Main photo" }
  ];
  options: string[] = ["name", "companyName", "modelNumber", "deviceType"];

  warningMessage: string = "";
  offsetChange = 5;
  limit = 5;
  offset = 0;
  hasMoreUsers: boolean = true;
  errorMessage: string = "";

  constructor(
    private deviceService: DeviceService,
    private loadingService: LoadingService
  ) {}

  ngOnInit() {
    this.fetchData();
  }

  onFilterChange(filterValues: { [key: string]: string }) {
    const validFilters = Object.keys(filterValues)
      .filter((key) => filterValues[key] && filterValues[key].trim() !== "")
      .reduce((obj, key) => {
        (obj as { [key: string]: string })[key] = filterValues[key];
        return obj;
      }, {} as { [key: string]: string });

    const queryString = this.buildQueryString(validFilters);
    setTimeout(() => {
      this.offset = 0;
      this.fetchData(queryString);
    }, 500);
  }
  private buildQueryString(filters: { [key: string]: string }): string {
    return Object.keys(filters)
      .map(
        (key) =>
          `${encodeURIComponent(key)}=${encodeURIComponent(filters[key])}`
      )
      .join("&");
  }

  fetchData(query?: string) {
    this.loadingService.show();
    const fullQuery = query
      ? `${query}&offset=${this.offset}&limit=${this.limit}`
      : `offset=${this.offset}&limit=${this.limit}`;
    this.deviceService.getDevices(fullQuery).subscribe({
      next: (response) => {
        this.dataObject = { ...response, originalData: response.data };
        this.hasMoreUsers = response.data.length === this.limit;
        this.loadingService.hide();
        this.errorMessage = "";
      },
      error: (error: any) => {
        this.errorMessage =
          error.details || "An error occurred while fetching devices";
        this.loadingService.hide();
      },
    });
  }

  onChangeOffset(newOffset: number) {
    if (newOffset < 0) {
      return;
    }
    this.offset = newOffset;
    this.fetchData();
  }
}
