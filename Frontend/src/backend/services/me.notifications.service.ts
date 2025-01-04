import { Injectable } from "@angular/core";
import "../models/object-Result";
import { Observable } from "rxjs";
import { MeNotificationsRepository } from "../repositories/me.notifications.repository";

@Injectable({
  providedIn: "root",
})
export class MeNotificationsService {
  constructor(private meNotificationsRepository: MeNotificationsRepository) {}

  getNotifications(fullQuery: string): Observable<ObjectResult> {
    return this.meNotificationsRepository.getNotifications(fullQuery);
  }

  readNotifications(notificationId: string[]): Observable<ObjectResult> {
    return this.meNotificationsRepository.readNotifications({
      NotificationsIds: notificationId,
    });
  }
}
