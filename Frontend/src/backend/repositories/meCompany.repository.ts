import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { map, Observable } from "rxjs";
import "../models/object-Result";
import environments from "../../environments";
import Repository from "./repository";

@Injectable({
  providedIn: "root",
})
export class meCompanyRepository extends Repository {
  constructor(protected readonly http: HttpClient) {
    super(environments.apiUrl, "me/imOwner", http);
  }


  userHasCompany(): Observable<boolean> {
    return this.get<any>().pipe(
      map(response => response.data.isOwner)
    );
  }

}