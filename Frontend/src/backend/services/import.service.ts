import { Injectable } from "@angular/core";
import "../models/object-Result";
import { Observable, throwError } from "rxjs";
import { ImportRepository } from "../repositories/import.repository";
import "../models/parameter"

@Injectable({
  providedIn: "root",
})
export class ImportService {
  constructor(
    private importRepository: ImportRepository
  ) {}

  currentImporterParams: parameter[] = [];

  updateImporterParams(ImporterName: string): Promise<void> {
    return new Promise((resolve, reject) => {
      this.GetImporterParams(ImporterName).subscribe({
        next: (response) => {
          this.currentImporterParams = Object.keys(response.data).map(key => ({
            name: key,
            type: response.data[key]
          }));
          resolve();
        },
        error: (err) => {
          console.error('Error updating importer params:', err);
          reject(err);
        }
      });
    });
  }

  getCurrentImporterParams() {
    return this.currentImporterParams;
  }

  getImporters(): Observable<ObjectResult> {
    return this.importRepository.getImporters();
  }

  GetImporterParams(ImporterName: string): Observable<ObjectResult> {
    return this.importRepository.getImporterParams(ImporterName);
  }
  ImportDevices(
    ImporterName: string,
    ImporterParams: {"Params" : any},
  ): Observable<ObjectResult> {
    return this.importRepository.importDevices(ImporterName, ImporterParams);
  }
}
