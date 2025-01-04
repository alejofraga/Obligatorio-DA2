import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import environments from "../../environments";
import Repository from "./repository";
import "../models/object-Result";

@Injectable({
  providedIn: "root",
})
export class HomeRepository extends Repository {
  constructor(protected readonly http: HttpClient) {
    super(environments.apiUrl, "homes", http);
  }

  addMember(id: string, memberData: string): Observable<ObjectResult> {
    return this.patch<ObjectResult>(memberData, `/${id}/members`);
  }
  
   getMembers(id: string, query?: string): Observable<ObjectResult> {
    return this.get<ObjectResult>(`/${id}/members`, query);
  }

  addHardware(id: string, hardwareData: string): Observable<ObjectResult> {
    return this.patch<ObjectResult>(hardwareData, `/${id}/hardwares`);
  }

  getHardwares(id: string, query?: string): Observable<ObjectResult> {
    return this.get<ObjectResult>(`/${id}/hardwares`, query);
  }

  grantPermissions(id: string, permissionsData: string): Observable<ObjectResult> {
    return this.patch<ObjectResult>(permissionsData, `/${id}/permissions`);
  }

  getMemberPermissions(homeId: string, query?: string): Observable<ObjectResult> {
    return this.get<ObjectResult>(`/${homeId}/members/permissions`, query);
  }

  userIsTheHomeOwner(homeId: string): Observable<ObjectResult> {
    return this.get<ObjectResult>(`/${homeId}/owner`);
  }

  addRoom(id: string, roomData: { Name: string, HardwareIds : any[]}): Observable<ObjectResult> {
    return this.post<ObjectResult>(roomData, `/${id}/rooms`);
  }

  getRooms(id: string): Observable<ObjectResult> {
    return this.get<ObjectResult>(`/${id}/rooms`);
  }

  changeHomeName(id: string, nameData : {Name : string}) : Observable<ObjectResult> {
    return this.patch<ObjectResult>(nameData, `/${id}/name`);
  }
}