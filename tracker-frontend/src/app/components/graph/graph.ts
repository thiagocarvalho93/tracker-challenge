import { Component, input } from '@angular/core';
import { Coordinate } from '../../models/coordinate.type';

@Component({
  selector: 'app-graph',
  imports: [],
  templateUrl: './graph.html',
  styleUrl: './graph.css',
})
export class Graph {
  pathCoordinates = input.required<Array<Coordinate>>();
  currentLocation = input.required<Coordinate>();
}
