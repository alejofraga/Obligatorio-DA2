import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import environments from "../../environments";
import Repository from "./repository";
import "../models/object-Result";

@Injectable({
  providedIn: "root",
})
export class DeviceCreationRepository extends Repository {
  constructor(protected readonly http: HttpClient) {
    super(environments.apiUrl, "", http);
  }

  createCamera(createCameraData: {
    Name: string;
    Description: string;
    ModelNumber: string;
    Photos: string[]; // Updated to string[]
    HasPersonDetection: boolean;
    HasMovementDetection: boolean;
    IsOutdoor: boolean;
    IsIndoor: boolean;
  }): Observable<ObjectResult> {
    console.log(`se llamo a crear camara con modelo: ${createCameraData.ModelNumber}`);
    return this.post<ObjectResult>(createCameraData, "cameras");
  }

  createLamp(createLampData: {
    Name: string;
    Description: string;
    ModelNumber: string;
    Photos: string[]; // Updated to string[]
  }): Observable<ObjectResult> {
    return this.post<ObjectResult>(createLampData, "lamps");
  }

  createSensor(createSensorData: {
    Name: string;
    Description: string;
    ModelNumber: string;
    Photos: string[]; // Updated to string[]
  }): Observable<ObjectResult> {
    return this.post<ObjectResult>(createSensorData, "sensors");
  }

  createMovementSensor(createMovementSensorData: {
    Name: string;
    Description: string;
    ModelNumber: string;
    Photos: string[]; // Updated to string[]
  }): Observable<ObjectResult> {
    return this.post<ObjectResult>(createMovementSensorData, "movementSensors");
  }
}