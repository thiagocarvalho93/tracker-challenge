import { Component, computed, input } from '@angular/core';
import { Coordinate } from '../../models/coordinate.type';
import { Status } from '../../models/status.type';

@Component({
  selector: 'app-graph',
  imports: [],
  templateUrl: './graph.html',
  styleUrl: './graph.css',
})
export class Graph {
  pathCoordinates = input.required<Array<Coordinate>>();
  status = input.required<Status>();
  currentLocation = input.required<Coordinate>();

  startPoint = computed(() => this.pathCoordinates()[0]);
  endPoint = computed(() => {
    const path = this.pathCoordinates();
    return path[path.length - 1];
  });
  polylinePoints = computed(() =>
    this.pathCoordinates()
      .map((p) => `${p.x},${p.y}`)
      .join(' '),
  );
  viewBox = computed(() => {
    const paddingX = 20;
    const paddingY = 10;

    const xs = this.pathCoordinates().map((coord) => coord.x);
    const ys = this.pathCoordinates().map((coord) => coord.y);
    const minX = Math.min(...xs) - paddingX;
    const maxX = Math.max(...xs) + paddingX;
    const minY = Math.min(...ys) - paddingY;
    const maxY = Math.max(...ys) + paddingY;
    const width = maxX - minX;
    const height = maxY - minY;
    return `${minX} ${minY} ${width} ${height}`;
  });
}
