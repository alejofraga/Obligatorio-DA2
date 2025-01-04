import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CompanyStateService {
  private hasCompanySubject = new BehaviorSubject<boolean>(false);
  hasCompany$ = this.hasCompanySubject.asObservable();

  setHasCompany(hasCompany: boolean) {
    this.hasCompanySubject.next(hasCompany);
  }
}