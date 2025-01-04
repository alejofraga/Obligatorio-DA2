import { Component, OnInit } from "@angular/core";
import { HomeService } from "../../../backend/services/home.service";
import { HomeManagementService } from "../../../backend/services/home-management.service";
import { FormControl, FormGroup } from "@angular/forms";
import { LoadingService } from "../../../backend/services/loading.service";

@Component({
  selector: "app-listing-hardware",
  templateUrl: "./listing-hardware.component.html",
  styles: ``,
})
export class ListingHardwareComponent implements OnInit {
  homeId!: string;
  listForm!: FormGroup;

  dataObject: any = {};
  rooms: any;
  roomsOptions: { value: string; label: string }[] = [];
  readonly formField: any = {
    rooms: {
      name: "rooms",
    },
  };

  categories: any[] = [
    { name: "name", alias: "Name" },
    { name: "modelNumber", alias: "Model number" },
    { name: "mainPhoto", alias: "Main photo" },
    { name: "lampIsOn", alias: "Is On" },
    { name: "doorSensorIsOpen", alias: "Is Open" },
    { name: "connectionStatus", alias: "Is Connected" },
  ];
  errorMessage: string = "";

  constructor(
    private homeService: HomeService,
    private homeManagementService: HomeManagementService,
    private loadingService: LoadingService
  ) {}

  ngOnInit(): void {
    this.homeManagementService.homeId$.subscribe((id) => {
      if (id) {
        this.homeId = id;
        this.loadRooms();
      }
    });
    this.listForm = new FormGroup({
      [this.formField.rooms.name]: new FormControl("", []),
    });
  }

  onRoomChange(selectedValue: string) {
    this.listForm.controls["rooms"].setValue(selectedValue);
    if (selectedValue === "all") {
      this.fetchData(this.homeId);
    } else {
      this.fetchData(this.homeId, selectedValue);
    }
  }

  loadRooms() {
    this.loadingService.show();
    this.homeService.getRooms(this.homeId).subscribe((data) => {
      this.loadingService.hide();
      this.rooms = Array.isArray(data.data.rooms)
        ? data.data.rooms
        : [data.data.rooms];
      this.roomsOptions = [
        { value: "all", label: "All Devices" },
        ...this.rooms.map((room: any) => ({
          value: room.name,
          label: `${room.name}`,
        })),
      ];
      this.listForm
        .get(this.formField.rooms.name)
        ?.setValue(this.roomsOptions[0].value);
      this.fetchData(this.homeId);
    });
  }

  reloadRooms() {
    this.loadingService.show();
    this.homeService.getRooms(this.homeId).subscribe((data) => {
      this.loadingService.hide(); 
      this.rooms = Array.isArray(data.data.rooms)
        ? data.data.rooms
        : [data.data.rooms];
      this.roomsOptions = [
        { value: "all", label: "All Devices" },
        ...this.rooms.map((room: any) => ({
          value: room.name,
          label: `${room.name}`,
        })),
      ];
    });
  }

  fetchData(homeId: string, roomName?: string) {
    let query = '';
    if (roomName && roomName !== "all") {
      query = `roomName=${roomName}`;
    }
    this.loadingService.show();
    this.homeService.getHardwares(homeId, query).subscribe({
      next: (response) => {
        this.loadingService.hide();
        console.log(response)
        this.processData(response.data);
        this.reloadRooms();
      },
      error: (error: any) => {
        console.error("Error fetching hardwares:", error);
        this.errorMessage = error.details;
      },
    });
  }

  processData(data: any[]): void {
    data.forEach(item => {
      console.log(item);
      console.log(item.deviceType);
      if (item.deviceType !== 'Lamp') {
        item.lampIsOn = '-';
      }
      if (item.deviceType !== 'Sensor'){
        item.doorSensorIsOpen = '-'
      }
    });
    this.dataObject = {data};
    console.log(this.dataObject)
  }
}