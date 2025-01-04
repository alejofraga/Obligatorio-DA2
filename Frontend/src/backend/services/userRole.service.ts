import { Injectable } from "@angular/core";
import "../models/object-Result";
import { HttpErrorResponse } from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { catchError, tap } from "rxjs/operators";
import { AuthService } from "./auth.service";
import { UserRoleRepository } from "../repositories/userRole.repository";

@Injectable({
  providedIn: "root",
})
export class UserRoleService {
  constructor(
    private userRoleRepostory: UserRoleRepository,
    private authService: AuthService
  ) {}

  UserRoles: string[] = [];

  getToken(): string | null {
    return this.authService.getToken();
  }
  getRolesFromLocalStorage(): string[] {
    const roles = localStorage.getItem("userRoles");
    return roles ? JSON.parse(roles) : [];
  }

  getUserRoles(): string[] {
    return this.UserRoles || this.getRolesFromLocalStorage();
  }

  addUserRole(role: string): Observable<ObjectResult> {
    return this.userRoleRepostory
      .addRole({ Role: role })
      .pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === 0) {
            console.error("No se pudo establecer la conexión con el servidor");
          } 
          return throwError(error);
        })
      )
      .pipe(
        tap((response: ObjectResult) => {
          this.UserRoles.push(role);
          localStorage.setItem("userRoles", JSON.stringify(this.UserRoles));
        })
      );
  }

  loadUserRoles(): Observable<ObjectResult> {
    return this.userRoleRepostory
      .getRoles()
      .pipe(
        catchError((error: HttpErrorResponse) => {
          if (error.status === 0) {
            console.error("No se pudo establecer la conexión con el servidor");
          } 
          return throwError(error);
        })
      )
      .pipe(
        tap((response: ObjectResult) => {
          this.UserRoles = response.data.roles;
          localStorage.setItem("userRoles", JSON.stringify(this.UserRoles));
        })
      );
  }
}
