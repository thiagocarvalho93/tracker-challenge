import { inject, Injectable } from '@angular/core';
import { Coordinate } from '../app/models/coordinate.type';
import { HttpClient } from '@angular/common/http';
import { BASE_URL } from '../app/app.constants';
import { Status } from '../app/models/status.type';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class TrackerApiService {
  httpClient = inject(HttpClient);

  getPathCoordinates(): Observable<Coordinate[]> {
    const url = `${BASE_URL}/path`;

    return this.httpClient.get<Array<Coordinate>>(url);
  }

  getStatus(coordinate: Coordinate, trackLine: boolean = false): Observable<Status> {
    let currentLineIndex = '0';

    if (trackLine) {
      currentLineIndex = localStorage.getItem('currentLineIndex') || '0';
    }
    const url = `${BASE_URL}/status?x=${coordinate.x}&y=${coordinate.y}&trackLine=${trackLine}&currentLineIndex=${currentLineIndex}`;

    return this.httpClient.get<Status>(url);
  }

  resetCurrentLine(): Observable<any> {
    const url = `${BASE_URL}/reset`;

    return this.httpClient.delete(url);
  }
}
