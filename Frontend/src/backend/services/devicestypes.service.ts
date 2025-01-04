import { Injectable } from "@angular/core";
import "../models/object-Result";
import { Observable } from "rxjs";
import { DevicesTypesRepository } from "../repositories/devicestypes.repository";

@Injectable({
    providedIn: "root",
  })
export class DevicesTypesService {
    constructor(private devicesTypesRepository: DevicesTypesRepository) {}

    getDevicesTypes(): Observable<ObjectResult> {
        return this.devicesTypesRepository.getDevicesTypes();
    }
}