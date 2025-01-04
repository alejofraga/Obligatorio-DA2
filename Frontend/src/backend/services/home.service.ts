import { Injectable } from "@angular/core";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";
import { HomeRepository } from "../repositories/home.repository";
import "../models/object-Result";
import { HttpErrorResponse } from "@angular/common/http";

@Injectable({
  providedIn: "root",
})
export class HomeService {
  constructor(private homeRepository: HomeRepository) {}

  addMember(id: string, memberData: any): Observable<ObjectResult> {
    return this.homeRepository.addMember(id, memberData).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 0) {
          console.error("No se pudo establecer la conexión con el servidor");
        } 
        return throwError(error);
      })
    );
  }

  getMembers(id: string, query?: string): Observable<ObjectResult> {
    return this.homeRepository.getMembers(id, query).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 0) {
          console.error("No se pudo establecer la conexión con el servidor");
        } 
        return throwError(error);
      })
    );
  }

  addHardware(id: string, hardwareData: any): Observable<ObjectResult> {
    return this.homeRepository.addHardware(id, hardwareData).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 0) {
          console.error("No se pudo establecer la conexión con el servidor");
        } 
        return throwError(error);
      })
    );
  }

  getHardwares(id: string, query?: string): Observable<ObjectResult> {
    return this.homeRepository.getHardwares(id, query).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 0) {
          console.error("No se pudo establecer la conexión con el servidor");
        } 
        return throwError(error);
      })
    );
  }

  grantPermissions(id: string, permissionsData: any): Observable<ObjectResult> {
    return this.homeRepository.grantPermissions(id, permissionsData).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 0) {
          console.error("No se pudo establecer la conexión con el servidor");
        } 
        return throwError(error);
      })
    );
  }

  getMemberPermissions(homeId: string): Observable<ObjectResult> {
    return this.homeRepository.getMemberPermissions(homeId).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 0) {
          console.error("No se pudo establecer la conexión con el servidor");
        } 
        return throwError(error);
      })
    );
  }
  
  addRoom(
    homeId: string,
    addRoomData: { Name: string; HardwareIds: any[] }
  ): Observable<ObjectResult> {
    return this.homeRepository.addRoom(homeId, addRoomData).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 0) {
          console.error("No se pudo establecer la conexión con el servidor");
        } 
        return throwError(error);
      })
    );
  }

  userIsTheHomeOwner(homeId: string): Observable<ObjectResult> {
    return this.homeRepository.userIsTheHomeOwner(homeId).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 0) {
          console.error("No se pudo establecer la conexión con el servidor");
        } 
        return throwError(error);
      })
    );
  }

  getRooms(homeId: string): Observable<ObjectResult> {
    return this.homeRepository.getRooms(homeId).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 0) {
          console.error("No se pudo establecer la conexión con el servidor");
        } 
        return throwError(error);
      })
    );
  }

  changeHomeName(
    homeId: string,
    nameData: { Name: string }
  ): Observable<ObjectResult> {
    return this.homeRepository.changeHomeName(homeId, nameData);
  }
}

