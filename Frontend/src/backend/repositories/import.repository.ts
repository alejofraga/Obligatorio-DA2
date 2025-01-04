import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import environments from "../../environments";
import Repository from "./repository";
import "../models/object-Result";

@Injectable({
  providedIn: "root",
})
export class ImportRepository extends Repository {
  constructor(protected readonly http: HttpClient) {
    super(environments.apiUrl, "importers", http);
  }

  getImporters(): Observable<ObjectResult> {
    return this.get<ObjectResult>();
  }

  getImporterParams(
    ImporterName : string
  ): Observable<ObjectResult> {
    return this.get<ObjectResult>(`/${ImporterName}/params`);
  }

    importDevices(
        ImporterName: string,
        ImporterParams: any,
    ): Observable<ObjectResult> {
        return this.post<ObjectResult>(ImporterParams,`/${ImporterName}`);
    }
}
