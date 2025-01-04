import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SharedService {
  private profilePictureUpdatedSource = new Subject<void>();
  private roleUpdatedSource = new Subject<void>();

  profilePictureUpdated$ = this.profilePictureUpdatedSource.asObservable();
  roleUpdated$ = this.roleUpdatedSource.asObservable();

  emitProfilePictureUpdated() {
    this.profilePictureUpdatedSource.next();
  }

  emitRoleUpdated() {
    this.roleUpdatedSource.next();
  }
}