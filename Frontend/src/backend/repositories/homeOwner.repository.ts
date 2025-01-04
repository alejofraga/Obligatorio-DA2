import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import "../models/object-Result";
import environments from "../../environments";
import Repository from "./repository";
@Injectable({
  providedIn: "root",
})
export class HomeOwnerRepository extends Repository {
  constructor(protected readonly http: HttpClient) {
    super(environments.apiUrl, "homeowners", http);
  }

  register(registerData: {
    Name: string;
    LastName: string;
    Email: string;
    Password: string;
    ProfilePicturePath: string;
  }): Observable<ObjectResult> {
    return this.post<ObjectResult>(registerData);
  }
}
