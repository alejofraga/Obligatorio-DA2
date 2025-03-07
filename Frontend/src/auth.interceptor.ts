import { Injectable } from "@angular/core";
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor() {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const token = localStorage.getItem("authToken");

    if (token) {
      const clonedRequest = req.clone({
        setHeaders: {
          Authorization: `${token}`,
        },
      });

      return next.handle(clonedRequest);
    }
    return next.handle(req);
  }
}
