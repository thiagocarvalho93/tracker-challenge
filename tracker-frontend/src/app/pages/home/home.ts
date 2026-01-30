import { Component, inject, OnInit, signal } from '@angular/core';
import { Graph } from '../../components/graph/graph';
import { TrackerService } from '../../../services/tracker';
import { Coordinate } from '../../models/coordinate.type';
import { FormsModule } from '@angular/forms';
import { catchError } from 'rxjs';
import { Status } from '../../models/status.type';
import { DecimalPipe } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatDividerModule } from '@angular/material/divider';

@Component({
  selector: 'app-home',
  imports: [
    Graph,
    FormsModule,
    DecimalPipe,
    MatButton,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatDividerModule,
  ],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  trackerService = inject(TrackerService);
  pathCoordinates = signal<Array<Coordinate>>([]);
  userCoordinates = signal<Coordinate>({ x: 0, y: 0 });
  userCoordinatesInput = signal<Coordinate>({ x: 0, y: 0 });
  status = signal<Status>({});

  handleUpdateLocation() {
    this.userCoordinates.set({
      x: this.userCoordinatesInput().x,
      y: this.userCoordinatesInput().y,
    });

    // this.trackerService
    //   .getStatus(this.userCoordinates())
    //   .pipe(
    //     catchError((error) => {
    //       console.error('Error fetching status:', error);
    //       throw error;
    //     }),
    //   )
    //   .subscribe((status) => {
    //     console.log('Fetched status:', status);
    //     this.status.set(status);
    //   });

    this.trackerService
      .getStatusStateful(this.userCoordinates())
      .pipe(
        catchError((error) => {
          console.error('Error fetching status:', error);
          throw error;
        }),
      )
      .subscribe((status) => {
        console.log('Fetched status:', status);
        this.status.set(status);

        if (status.currentLineIndex !== undefined) {
          sessionStorage.setItem('currentLineIndex', status.currentLineIndex.toString());
        }
      });
  }

  ngOnInit(): void {
    this.trackerService
      .getPathCoordinates()
      .pipe(
        catchError((error) => {
          console.error('Error fetching path coordinates:', error);
          throw error;
        }),
      )
      .subscribe((coordinates) => {
        console.log('Fetched path coordinates:', coordinates);
        this.pathCoordinates.set(coordinates);
        this.userCoordinatesInput.set({ x: coordinates[0].x, y: coordinates[0].y });
      });

    this.handleUpdateLocation();
  }
}
