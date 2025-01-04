import {
  HttpClient,
  HttpErrorResponse,
  HttpParams,
} from "@angular/common/http";

import { Observable, catchError, retry, throwError } from "rxjs";

export default abstract class Repository {
  protected fullEndpoint: string;

  constructor(
    protected readonly _apiOrigin: string,
    protected readonly _endpoint: string,
    protected readonly _http: HttpClient
  ) {
    this.fullEndpoint = `${this._apiOrigin}/${this._endpoint}`;
  }

  protected get<T>(extraResource = "", query = ""): Observable<T> {
    query = query ? `?${query}` : "";
    extraResource = extraResource ? `${extraResource}` : "";

    return this._http
      .get<T>(`${this.fullEndpoint}${extraResource}${query}`)
      .pipe(retry(3), catchError(this.handleError));
  }

  protected post<T>(body: any, extraResource = ""): Observable<T> {
    extraResource = extraResource ? `${extraResource}` : "";

    return this._http
      .post<T>(`${this.fullEndpoint}${extraResource}`, body)
      .pipe(retry(3), catchError(this.handleError));
  }

  protected delete<T>(body: any, extraResource = ""): Observable<T> {
    extraResource = extraResource ? `${extraResource}` : "";
  
    return this._http
      .request<T>('delete', `${this.fullEndpoint}${extraResource}`, { body })
      .pipe(retry(3), catchError(this.handleError));
  }

  protected patch<T>(body: any, extraResource = ""): Observable<T> {
    extraResource = extraResource ? `${extraResource}` : "";
  
    return this._http
      .patch<T>(`${this.fullEndpoint}${extraResource}`, body)
      .pipe(retry(3), catchError(this.handleError));
  }


  protected handleError(error: HttpErrorResponse) {
    return throwError(error.error);
  }
}
