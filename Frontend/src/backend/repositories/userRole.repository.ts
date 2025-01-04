import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import "../models/object-Result";
import environments from '../../environments';
import Repository from "./repository";
@Injectable({
  providedIn: "root",
})
export class UserRoleRepository extends Repository {
  constructor(protected readonly http: HttpClient) {
    super(environments.apiUrl, 'me/roles', http);
  }
  getRoles(): Observable<ObjectResult> {
    return this.get<ObjectResult>();
  }

  addRole(addRoleBody: { Role : string }): Observable<ObjectResult> {
    return this.post<ObjectResult>(addRoleBody);
  }
}
