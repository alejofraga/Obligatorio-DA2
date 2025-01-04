import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import "../models/object-Result";
import environments from "../../environments";
import Repository from "./repository"

@Injectable({
    providedIn: "root",
  })
export class HardwareRepository extends Repository {
    constructor(protected readonly http: HttpClient) {
        super(environments.apiUrl, "hardwares", http);
    }

    setHardwareRoom(hardwareId : string, setHardwareRoomData : {roomId : string}): Observable<ObjectResult> {
        return this.patch<ObjectResult>(setHardwareRoomData,`/${hardwareId}/room`);
    }
    changeHardwarename(hardwareId : string, nameData : {Name : string}): Observable<ObjectResult> {
        return this.patch<ObjectResult>(nameData,`/${hardwareId}/name`);
    }
}
