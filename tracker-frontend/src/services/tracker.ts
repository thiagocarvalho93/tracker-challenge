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
    const url = `${BASE_URL}/path-coordinates`;

    return this.httpClient.get<Array<Coordinate>>(url);
  }

  getStatus(coordinate: Coordinate): Observable<Status> {
    const url = `${BASE_URL}/info?x=${coordinate.x}&y=${coordinate.y}`;

    return this.httpClient.get<Status>(url);
  }
}
