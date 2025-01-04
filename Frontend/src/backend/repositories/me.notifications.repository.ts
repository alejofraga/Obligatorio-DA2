	import { Injectable } from "@angular/core";
	import { HttpClient } from "@angular/common/http";
	import { Observable } from "rxjs";
	import "../models/object-Result";
	import environments from "../../environments";
	import Repository from "./repository";
	@Injectable({
	providedIn: "root",
	})
	export class MeNotificationsRepository extends Repository {
	constructor(protected readonly http: HttpClient) {
		super(environments.apiUrl, "me/notifications", http);
	}

	getNotifications(fullQuery: string): Observable<ObjectResult> {
		return this.get<ObjectResult>("", fullQuery);
	}

	readNotifications(ReadNotificationsRequest: {
		NotificationsIds: string[];
	}): Observable<ObjectResult> {
		return this.patch<ObjectResult>(ReadNotificationsRequest);
	}
	}
