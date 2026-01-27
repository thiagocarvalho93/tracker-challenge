import { Injectable } from '@angular/core';
import { Coordinate } from '../app/models/coordinate.type';

@Injectable({
  providedIn: 'root',
})
export class TrackerService {
  coordinates: Array<Coordinate> = [{ x: 50, y: 46 }];
}
