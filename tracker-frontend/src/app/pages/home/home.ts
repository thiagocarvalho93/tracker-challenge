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
import { MatInputModule } from '@angular/material/input';
import { MatDividerModule } from '@angular/material/divider';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-home',
  imports: [
    Graph,
    FormsModule,
    DecimalPipe,
    MatButton,
    MatFormFieldModule,
    MatInputModule,
    MatDividerModule,
    MatCheckboxModule,
    MatCardModule,
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
  trackCurrentLine = signal<boolean>(false);
  loading = signal<boolean>(false);

  handleResetCurrentLine() {
    this.trackerService
      .resetCurrentLine()
      .pipe(
        catchError((error) => {
          console.error('Error resetting current line:', error);
          throw error;
        }),
      )
      .subscribe(() => {
        console.log('Current line reset successfully.');
        this.handleUpdateLocation();
      });
  }

  handleUpdateLocation() {
    const { x, y } = this.userCoordinatesInput();
    this.userCoordinates.set({ x, y });
    this.loading.set(true);

    this.trackerService
      .getStatus(this.userCoordinates(), this.trackCurrentLine())
      .pipe(
        catchError((error) => {
          console.error('Error fetching status:', error);
          this.loading.set(false);
          throw error;
        }),
      )
      .subscribe((status) => {
        console.log('Fetched status:', status);
        this.status.set(status);
        this.loading.set(false);
      });
  }

  handleKeyDown(event?: KeyboardEvent) {
    if (event && event.key === 'Enter') {
      this.handleUpdateLocation();
    }
  }

  ngOnInit(): void {
    this.loading.set(true);
    this.trackerService
      .getPathCoordinates()
      .pipe(
        catchError((error) => {
          console.error('Error fetching path coordinates:', error);
          this.loading.set(false);
          throw error;
        }),
      )
      .subscribe((coordinates) => {
        console.log('Fetched path coordinates:', coordinates);
        this.pathCoordinates.set(coordinates);
        const { x, y } = coordinates[0];
        this.userCoordinatesInput.set({ x, y });
        this.handleUpdateLocation();
      });
  }
}
