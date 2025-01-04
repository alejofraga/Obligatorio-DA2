import { Injectable } from "@angular/core";
import "../models/object-Result";
import { HttpErrorResponse } from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { catchError, tap } from "rxjs/operators";
import { HomeOwnerRepository } from "../repositories/homeOwner.repository";

@Injectable({
  providedIn: "root",
})
export class HomeOwnerService {

  constructor(private homeOwnerRepository: HomeOwnerRepository) {}


  register(name :string , lastname : string, email: string, password: string, profilePicturePath : string): Observable<ObjectResult> {
    return this.homeOwnerRepository
      .register({ Name: name, LastName: lastname, Email: email, Password: password, ProfilePicturePath: profilePicturePath })
      .pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === 0) {
            console.error("No se pudo establecer la conexión con el servidor");
            return throwError("No se pudo establecer la conexión con el servidor");
          } 
          return throwError(error);
        })
      );
  }
}
