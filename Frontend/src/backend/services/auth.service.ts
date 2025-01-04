import { Injectable } from "@angular/core";
import { AuthRepository } from "../repositories/auth.repository";
import "../models/object-Result";
import { HttpErrorResponse } from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { catchError, tap } from "rxjs/operators";

@Injectable({
  providedIn: "root",
})
export class AuthService {
  private token: string | null = null;

  constructor(private authRepository: AuthRepository) {}

  login(email: string, password: string): Observable<ObjectResult> {
    return this.authRepository.login({ Email: email, Password: password }).pipe(
      tap((response: ObjectResult) => {
        this.token = response.data.token;
        if (this.token) {
          localStorage.setItem("authToken", this.token);
          localStorage.setItem("userEmail", email);
        } else {
          console.error("No se recibió un token en la respuesta del login");
        }
      }),
      catchError((error: HttpErrorResponse) => {
        console.error("Error en el login:", error);
        return throwError(error);
      })
    );
  }

  logout(): void {
    this.token = null;
    localStorage.removeItem("authToken");
    localStorage.removeItem("userRoles");
  }

  isAuthenticated(): boolean {
    return this.getToken() !== null;
  }
  getToken(): string | null {
    const token = this.token || localStorage.getItem("authToken");
    return token;
  }
  getEmail(): string | null {
    return localStorage.getItem("userEmail");
  }
  getRoles(): string[] {
    const roles = localStorage.getItem("userRoles");
    return roles ? JSON.parse(roles) : [];
  }
}
