import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import environments from "../../environments";
import Repository from "./repository";
import "../models/object-Result";

@Injectable({
  providedIn: "root",
})
export class DeviceRepository extends Repository {
  constructor(protected readonly http: HttpClient) {
    super(environments.apiUrl, "devices", http);
  }

  getDevices(fullQuery?: string): Observable<ObjectResult> {
    return this.get<ObjectResult>("", fullQuery);
  }
}
