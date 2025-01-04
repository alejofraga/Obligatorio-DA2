import { Injectable } from "@angular/core";
import "../models/object-Result";
import { Observable } from "rxjs";
import { MeHomeRepository } from "../repositories/me.home.repository";


@Injectable({
  providedIn: "root",
})
export class MeHomeService {

  constructor(private meHomeRepository : MeHomeRepository) {}


  createHome(name : string , address : string, doorNumber: string, latitude : string, longitude : string, memberCount : Int32Array) : Observable<ObjectResult> {
    return this.meHomeRepository.createHome({ Name: name, Address: address, DoorNumber: doorNumber, Latitude: latitude, Longitude: longitude, MemberCount: memberCount });
  }

  getHomes(fullQuery : string) : Observable<ObjectResult> {
    return this.meHomeRepository.getHomes(fullQuery);
  }
}
