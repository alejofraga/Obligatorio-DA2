import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import "../models/object-Result";
import environments from "../../environments";
import Repository from "./repository";
@Injectable({
  providedIn: "root",
})
export class MeHomeRepository extends Repository {
  constructor(protected readonly http: HttpClient) {
    super(environments.apiUrl, "me/homes", http);
  }

  createHome(createHomeData: {
    Name: string;
    Address: string;
    DoorNumber: string;
    Latitude: string;
    Longitude: string;
    MemberCount: Int32Array;
  }): Observable<ObjectResult> {
    return this.post<ObjectResult>(createHomeData);
  }

  getHomes(fullQuery : string): Observable<ObjectResult> {
    return this.get<ObjectResult>("" , fullQuery);
  }
}
