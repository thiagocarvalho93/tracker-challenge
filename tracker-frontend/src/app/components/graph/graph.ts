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

  lines = computed(() =>
    this.pathCoordinates()
      .slice(0, -1)
      .map((point, i) => [point, this.pathCoordinates()[i + 1]]),
  );

  viewBox = computed(() => {
    const padding = 10;
    const xs = this.pathCoordinates().map((coord) => coord.x);
    const ys = this.pathCoordinates().map((coord) => coord.y);
    const minX = Math.min(...xs) - padding;
    const maxX = Math.max(...xs) + padding;
    const minY = Math.min(...ys) - padding;
    const maxY = Math.max(...ys) + padding;
    const width = maxX - minX;
    const height = maxY - minY;
    return `${minX} ${minY} ${width} ${height}`;
  });
}
