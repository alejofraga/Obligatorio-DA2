import { Injectable } from "@angular/core";
import "../models/object-Result";
import { HttpErrorResponse } from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { catchError, tap } from "rxjs/operators";
import { CompanyRepository } from "../repositories/company.repository";
import { ValidatorRepository } from "../repositories/validator.repository";

@Injectable
({
  providedIn
    :
    "root"
})
export class CompanyService {
    private token: string | null = null;

    constructor(private companyRepository: CompanyRepository, private validatorRepository: ValidatorRepository) {}

    createCompany(
        name: string,
        rut: string,
        logoUrl: string,
        validator: string
    ): Observable < ObjectResult > {
        return this.companyRepository.createCompany({
            Name: name,
            Rut: rut,
            LogoUrl: logoUrl,
            Validator: validator
        }).pipe(
            catchError((error: HttpErrorResponse) => {
                if (error.status === 0) {
                    console.error("No se pudo establecer la conexión con el servidor");
                } 
                return throwError(error);
            })
        );
    }
  
  getCompanies(filters?: string): Observable < ObjectResult > {
        return this.companyRepository.getCompanies(filters).pipe(
            tap((response: ObjectResult) => {
            }),
            catchError((error: HttpErrorResponse) => {
                if (error.status === 0) {
                    console.error("No se pudo establecer la conexión con el servidor");
                } 
                return throwError(error);
            })
        );
    }

    getValidators(): Observable < ObjectResult > {
        return this.validatorRepository.getValidators().pipe(
            catchError((error: HttpErrorResponse) => {
                if (error.status === 0) {
                    console.error("No se pudo establecer la conexión con el servidor");
                } 
                return throwError(error);
            })
        );
    }
}