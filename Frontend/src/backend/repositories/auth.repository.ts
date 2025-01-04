import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import "../models/object-Result";
import environments from "../../environments";
import Repository from "./repository";
@Injectable({
  providedIn: "root",
})
export class AuthRepository extends Repository {
  constructor(protected readonly http: HttpClient) {
    super(environments.apiUrl, "auth", http);
  }
  login(loginData: {
    Email: string;
    Password: string;
  }): Observable<ObjectResult> {
    return this.post<ObjectResult>(loginData);
  }
}
