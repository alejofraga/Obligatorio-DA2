import { Injectable } from "@angular/core";
import "../models/object-Result";
import { Observable, throwError } from "rxjs";
import { DeviceCreationRepository } from "../repositories/deviceCreation.repository";
import { DeviceRepository } from "../repositories/device.repository";

@Injectable({
  providedIn: "root",
})
export class DeviceService {
  constructor(private deviceCreationRepository: DeviceCreationRepository, private deviceRepository : DeviceRepository) {}

  getDevices(fullQuery : string): Observable<ObjectResult> {
    return this.deviceRepository.getDevices(fullQuery);
  }

  RegisterDevice(
    DeviceType: string,
    Name: string,
    Description: string,
    ModelNumber: string,
    Photos: string[],
    IsOutdoor: boolean,
    IsIndoor: boolean,
    HasPersonDetection: boolean,
    HasMovementDetection: boolean
  ): Observable<ObjectResult> {
    switch (DeviceType) {
      case "Lamp":
        return this.deviceCreationRepository.createLamp({
          Name,
          Description,
          ModelNumber,
          Photos,
        });
      case "Sensor":
        return this.deviceCreationRepository.createSensor({
          Name,
          Description,
          ModelNumber,
          Photos,
        });
      case "MovementSensor":
        return this.deviceCreationRepository.createMovementSensor({
          Name,
          Description,
          ModelNumber,
          Photos,
        });
      case "Camera":
        return this.deviceCreationRepository.createCamera({
          Name,
          Description,
          ModelNumber,
          Photos,
          HasPersonDetection,
          HasMovementDetection,
          IsOutdoor,
          IsIndoor,
        });
      default:
        throw new Error("Invalid device type");
    }
  }
}
