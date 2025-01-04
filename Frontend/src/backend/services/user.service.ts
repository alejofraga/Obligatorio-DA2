import { Injectable } from "@angular/core";
import "../models/object-Result";
import { HttpErrorResponse } from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { catchError, tap } from "rxjs/operators";
import { UserRepository } from "../repositories/user.repository";
import { meCompanyRepository } from "../repositories/meCompany.repository";

@Injectable({
  providedIn: "root",
})
export class UserService {

  constructor(private userRepository: UserRepository, private meCompanyRepostory: meCompanyRepository) {}

  hasCompany : boolean = false;
  register(
    name: string,
    lastname: string,
    email: string,
    password: string,
    role: string
  ): Observable<ObjectResult> {
    return this.userRepository
      .register({
        Name: name,
        LastName: lastname,
        Email: email,
        Password: password,
        Role: role,
      })
      .pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === 0) {
            console.error("No se pudo establecer la conexión con el servidor");
          }
          return throwError(error);
        })
      );
  }

  getUsers(filters?: string): Observable<ObjectResult> {
    return this.userRepository.getUsers(filters).pipe(
      tap((response) => {
      }),
      catchError((error: HttpErrorResponse) => {
        if (error.status === 0) {
          console.error("No se pudo establecer la conexión con el servidor");
        } 
        return throwError(error);
      })
    );
  }

  removeAdmin(removeAdminData : {
    Email : string
  }): Observable<ObjectResult> {
    return this.userRepository.removeAdmin(removeAdminData);
  }


  loadHasCompany(): Observable<boolean> {
    return this.meCompanyRepostory.userHasCompany().pipe(
      tap((hasCompany: boolean) => {
        this.hasCompany = hasCompany;
      })
    );
  }

  userHasCompany(): boolean {
    return this.hasCompany;
  }

  getProfilePicture(email: string): Observable<ObjectResult> {
    return this.userRepository.getProfilePicture(email);
  }

  updateProfilePicture(email: string, picture: any): Observable<ObjectResult> {
    return this.userRepository.updateProfilePicture(email, picture);
  }

  getDefaultProfilePicture(): string {
    return "https://www.w3schools.com/howto/img_avatar.png";
  }
}
