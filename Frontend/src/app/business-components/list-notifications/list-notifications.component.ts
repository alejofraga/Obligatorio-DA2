import { Component, OnInit } from "@angular/core";
import { MeNotificationsService } from "../../../backend/services/me.notifications.service";
import { DevicesTypesService } from "../../../backend/services/devicestypes.service";
import { LoadingService } from "../../../backend/services/loading.service";

@Component({
  selector: "app-list-notifications",
  templateUrl: "./list-notifications.component.html",
  styles: ``,
})
export class ListNotificationsComponent implements OnInit {
  dataObject: any = {};
  categories: any[] = [
    { name: "message", alias: "Message" },
    { name: "state", alias: "Readed" },
    { name: "dateTime", alias: "Date & Time" },
    { name: "deviceType", alias: "Device Type" },
    { name: "hardwareId", alias: "Hardware ID" },
    { name: "Action", alias: "" },
  ];
  actionCategories = [
    {
      category: "Action",
      class: "btn btn-outline-success",
      title: "Mark as read",
      onClick: (item: any) => this.readNotification(item),
      showWhen: (item: any) => !item.state,
    },
  ];
  groupClass: string = "btn-group btn-group-toggle";
  radioButtonOptions = [
    {
      title: "Unread",
      name: "Read",
      class: "btn btn-secondary",
      checked: true,
      value: false,
    },
    {
      title: "Show all",
      name: "Read",
      class: "btn btn-secondary",
      checked: false,
      value: null,
    },
    {
      title: "Read",
      name: "Read",
      class: "btn btn-secondary",
      checked: false,
      value: true,
    },
  ];
  warningMessage: string = "";
  offsetChange = 100;
  limit = 100;
  offset = 0;
  hasMoreNotifications: boolean = false;
  errorMessage: string = "";
  selectInputOptions: { value: any; label: string }[] = [
    { value: null, label: "Any" },
  ];
  private currentFilters: { [key: string]: any } = { Read: false };

  constructor(
    private meNotificationsService: MeNotificationsService,
    private deviceTypesService: DevicesTypesService,
    private loadingService: LoadingService
  ) {}

  ngOnInit(): void {
    const queryString = this.buildQueryString(this.currentFilters);
    this.fetchData(queryString);
    this.loadDeviceTypesData();
  }

  loadDeviceTypesData() {
    this.deviceTypesService.getDevicesTypes().subscribe({
      next: (response) => {
        const newOptions = response.data.map((deviceType: any) => ({
          value: deviceType.name,
          label: deviceType.name,
        }));
        this.selectInputOptions = [...this.selectInputOptions, ...newOptions];
        this.errorMessage = "";
      },
      error: (error: any) => {
        this.errorMessage =
          error.details || "An error occurred while fetching device types";
      },
    });
  }

  readNotification(item: any) {
    this.meNotificationsService
      .readNotifications([item.notificationId])
      .subscribe({
        next: () => {
          const queryString = this.buildQueryString(this.currentFilters);
          this.fetchData(queryString);
          this.errorMessage = "";
        },
        error: (err: any) => {
          this.errorMessage = err.details;
        },
      });
  }

  fetchData(query?: string) {
    const fullQuery = query ? query : "";
    this.loadingService.show();
    this.meNotificationsService.getNotifications(fullQuery).subscribe({
      next: (response) => {
        this.dataObject = { ...response, originalData: response.data };
        this.loadingService.hide();
        this.errorMessage = "";
        
      },
      error: (error: any) => {
        this.errorMessage =
          error.details || "An error occurred while fetching notifications";
        this.loadingService.hide();
      },
    });
  }

  onDateFilterChange(filterValues: { [key: string]: string }) {
    this.updateFilters(filterValues);
  }

  onSelectFilterChange(filterValues: { [key: string]: any }) {
    this.updateFilters(filterValues);
  }

  onRadioFilterChange(filterValues: { [key: string]: any }) {
    this.updateFilters(filterValues);
  }

  private updateFilters(newFilters: { [key: string]: any }) {
    Object.keys(newFilters).forEach((key) => {
      const value = newFilters[key];
      if (value === null || value === "null" || value === "") {
        delete this.currentFilters[key];
      } else {
        this.currentFilters[key] = value;
      }
    });

    const queryString = this.buildQueryString(this.currentFilters);
    this.fetchData(queryString);
  }

  private buildQueryString(filters: { [key: string]: any }): string {
    return Object.keys(filters)
      .map(
        (key) =>
          `${encodeURIComponent(key)}=${encodeURIComponent(filters[key])}`
      )
      .join("&");
  }
}
