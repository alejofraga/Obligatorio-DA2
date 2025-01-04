import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import environments from "../../environments";
import Repository from "./repository";
import "../models/object-Result";


@Injectable({
  providedIn: "root",
})
export class CompanyRepository extends Repository {

  constructor(protected readonly http: HttpClient) {
    super(environments.apiUrl, "companies", http);
  }

  createCompany(companyData: {
    Name: string;
    Rut: string;
    LogoUrl: string;
    Validator: string;
  }): Observable<ObjectResult> {
    return this.post<ObjectResult>(companyData);
  }

  getCompanies(filters?: string): Observable<ObjectResult> {
    return this.get<ObjectResult>("", filters);
  }
}