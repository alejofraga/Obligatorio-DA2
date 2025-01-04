import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HomeManagementService {
  private homeIdSource = new BehaviorSubject<string | null>(null);
  homeId$ = this.homeIdSource.asObservable();

  validHomePermissions = 
  [
    'AddDevice', 
    'ChangeHomeName', 
    'GrantHomePermissions', 
    'ListDevices', 
    'ReceiveNotifications', 
    'ChangeDeviceName', 
    'AddDeviceToRoom'
  ]

  setHomeId(id: string) {
    this.homeIdSource.next(id);
  }

}