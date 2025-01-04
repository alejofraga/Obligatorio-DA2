import { Injectable } from "@angular/core";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";
import { HomeRepository } from "../repositories/home.repository";
import "../models/object-Result";
import { HttpErrorResponse } from "@angular/common/http";
import { HardwareRepository } from "../repositories/hardware.repository";

@Injectable({
  providedIn: "root",
})
export class HardwareService {
  constructor(private hardwareRepository: HardwareRepository) {}

  addHardwareToRoom(
    hardwareId: string,
    addHardwareToRoomData: { roomId: string }
  ): Observable<ObjectResult> {
    return this.hardwareRepository.setHardwareRoom(
      hardwareId,
      addHardwareToRoomData
    );
  }
  changeHardwarename(
    hardwareId: string,
    nameData: { Name: string }
  ): Observable<ObjectResult> {
    return this.hardwareRepository.changeHardwarename(hardwareId, nameData);
  }
}
