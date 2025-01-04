import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { HomeManagementService } from "../../../backend/services/home-management.service";
import { HomeService } from "../../../backend/services/home.service";
import { HardwareService } from "../../../backend/services/hardware.service";
import { LoadingService } from "../../../backend/services/loading.service";

@Component({
  selector: "app-add-hardware-to-room",
  templateUrl: "./add-hardware-to-room.component.html",
  styles: ``,
})
export class AddHardwareToRoomComponent implements OnInit {
  registerForm!: FormGroup;
  hardwares: any;
  hardwareOptions: { value: string; label: string }[] = [];
  rooms: any;
  roomsOptions: { value: string; label: string }[] = [];
  homeId: string = "";
  SuccessMessage: string = "";
  ErrorMessage: string = "";

  readonly formField: any = {
    hardwares: {
      name: "hardwares",
      class: "form-control",
    },
    rooms: {
      name: "rooms",
      class: "form-control",
    },
  };

  constructor(
    private homeService: HomeService,
    private homeManagementService: HomeManagementService,
    private hardwareService: HardwareService,
    private loadingService: LoadingService
  ) {}

  ngOnInit() {
    this.homeManagementService.homeId$.subscribe((id) => {
      if (id) {
        this.homeId = id;
      }
    });
    this.registerForm = new FormGroup({
      [this.formField.rooms.name]: new FormControl("", []),
      [this.formField.hardwares.name]: new FormControl("", []),
    });
    this.loadHardwares();
    this.loadRooms();
  }

  loadHardwares() {
    this.loadingService.show();
    this.homeService.getHardwares(this.homeId).subscribe((data) => {
      this.loadingService.hide();
      this.hardwares = data.data.filter((hardware: any) => !hardware.isInARoom);
      this.hardwareOptions = this.hardwares.map((hardware: any) => ({
        value: hardware.id,
        label: `${hardware.name} - ${hardware.id}`,
      }));
      this.registerForm
        .get(this.formField.hardwares.name)
        ?.setValue(this.hardwareOptions[0].value);
    });
  }

  loadRooms() {
    this.loadingService.show();
    this.homeService.getRooms(this.homeId).subscribe((data) => {
      this.loadingService.hide();
      this.rooms = Array.isArray(data.data.rooms)
        ? data.data.rooms
        : [data.data.rooms];
      this.roomsOptions = this.rooms.map((room: any) => ({
        value: room.id,
        label: `${room.name}`,
      }));
      this.registerForm
        .get(this.formField.rooms.name)
        ?.setValue(this.roomsOptions[0].value);
    });
  }

  onHardwareChange(selectedValues: string[]) {
    this.registerForm.controls["hardwares"].setValue(selectedValues);
  }

  onRoomChange(selectedValue: string) {
    this.registerForm.controls["rooms"].setValue(selectedValue);
  }

  public onSubmit = (values: any) => {
    console.log(values);
    if (!this.registerForm.valid) {
      this.SuccessMessage = "";
      this.ErrorMessage = "Please fill the form correctly";
      return;
    }
    if (!values.hardwares || values.hardwares.length === 0) {
      this.SuccessMessage = "";
      this.ErrorMessage = "Please select at least one hardware";
      return;
    }
    if (!values.rooms) {
      this.SuccessMessage = "";
      this.ErrorMessage = "Please select at least one room";
      return;
    }
    const selectedRooms = values.rooms;
    const selectedHardwares = values.hardwares;
    this.loadingService.show();
    this.hardwareService
      .addHardwareToRoom(selectedHardwares, { roomId: selectedRooms })
      .subscribe({
        next: () => {
          this.loadingService.hide();
          this.SuccessMessage = "Hardware added to room successfully";
          this.ErrorMessage = "";
          this.removeSelectedHardwares(selectedHardwares);
          if (this.hardwareOptions.length === 0) {
            this.registerForm.get(this.formField.hardwares.name)?.setValue("");
          } else {
            this.registerForm
              .get(this.formField.hardwares.name)
              ?.setValue(this.hardwareOptions[0].value);
          }
        },
        error: (error) => {
          this.SuccessMessage = "";
          this.ErrorMessage = error.details;
          this.loadingService.hide();
        }
      });
  };

  removeSelectedHardwares(hardwareId: string) {
    this.hardwares = this.hardwares.filter(
      (hardware: any) => hardware.id !== hardwareId
    );
    this.hardwareOptions = this.hardwares.map((hardware: any) => ({
      value: hardware.id,
      label: `${hardware.name} - ${hardware.id}`,
    }));
  }

  registerClass = "btn btn-primary";
}
