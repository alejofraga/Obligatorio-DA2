import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import "../models/object-Result";
import environments from "../../environments";
import Repository from "./repository"

@Injectable({
    providedIn: "root",
  })
export class DevicesTypesRepository extends Repository {
    constructor(protected readonly http: HttpClient) {
        super(environments.apiUrl, "devicetypes", http);
    }

    getDevicesTypes(): Observable<ObjectResult> {
        return this.get<ObjectResult>();
    }
}
