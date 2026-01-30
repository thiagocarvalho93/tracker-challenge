import { inject, Injectable } from '@angular/core';
import { Coordinate } from '../app/models/coordinate.type';
import { HttpClient } from '@angular/common/http';
import { BASE_URL } from '../app/app.constants';
import { Status } from '../app/models/status.type';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TrackerService {
  httpClient = inject(HttpClient);

  getPathCoordinates(): Observable<Coordinate[]> {
    const url = `${BASE_URL}/path`;

    return this.httpClient.get<Array<Coordinate>>(url);
  }

  getStatus(coordinate: Coordinate): Observable<Status> {
    const url = `${BASE_URL}/status?x=${coordinate.x}&y=${coordinate.y}`;

    return this.httpClient.get<Status>(url);
  }

  getStatusStateful(coordinate: Coordinate): Observable<Status> {
    const currentLineIndex = sessionStorage.getItem('currentLineIndex') || '0';

    const url = `${BASE_URL}/status-stateful?x=${coordinate.x}&y=${coordinate.y}&lineIndex=${currentLineIndex}`;

    return this.httpClient.get<Status>(url);
  }
}
