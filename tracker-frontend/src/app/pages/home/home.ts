import { Component, inject, OnInit, signal } from '@angular/core';
import { Graph } from '../../components/graph/graph';
import { TrackerService } from '../../../services/tracker';
import { Coordinate } from '../../models/coordinate.type';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-home',
  imports: [Graph, FormsModule],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  trackerService = inject(TrackerService);
  pathCoordinates = signal<Array<Coordinate>>([]);
  userCoordinates = signal<Coordinate>({ x: 0, y: 0 });

  ngOnInit(): void {
    console.log(this.trackerService.coordinates);
    this.pathCoordinates.set(this.trackerService.coordinates);
  }
}
