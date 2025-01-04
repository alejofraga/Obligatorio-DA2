import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import "../models/object-Result";
import environments from "../../environments";
import Repository from "./repository";

@Injectable({
  providedIn: "root",
})
export class UserRepository extends Repository {
  constructor(protected readonly http: HttpClient) {
    super(environments.apiUrl, "users", http);
  }

  register(registerData: {
    Name: string;
    LastName: string;
    Email: string;
    Password: string;
    Role: string;
  }): Observable<ObjectResult> {
    return this.post<ObjectResult>(registerData);
  }

  getUsers(filters?: string): Observable<ObjectResult> {
    return this.get<ObjectResult>("", filters);
  }

  removeAdmin(removeAdminData: {
  }): Observable<ObjectResult> {
    return this.delete<ObjectResult>(removeAdminData);
  }

  getProfilePicture(email: string): Observable<ObjectResult> {
    return this.get<ObjectResult>(`/${email}/picture`);
  }

  updateProfilePicture(email: string, picture: any): Observable<ObjectResult> {
    return this.patch<ObjectResult>(picture, `/${email}/picture`);
  }
}
