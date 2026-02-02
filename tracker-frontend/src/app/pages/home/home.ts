import { Component, inject, OnInit, signal, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser, DecimalPipe, CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { catchError } from 'rxjs';

import { Graph } from '../../components/graph/graph';
import { TrackerApiService } from '../../../services/tracker-api';
import { Coordinate } from '../../models/coordinate.type';
import { Status } from '../../models/status.type';

import { MatButton } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDividerModule } from '@angular/material/divider';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'app-home',
  standalone: true,
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
    ReactiveFormsModule,
    CommonModule,
  ],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  private platformId = inject(PLATFORM_ID);
  private fb = inject(FormBuilder);
  private trackerApiService = inject(TrackerApiService);

  coordinateForm = this.fb.group({
    x: [0, [Validators.required, Validators.pattern(/^-?\d+(\.\d+)?$/)]],
    y: [0, [Validators.required, Validators.pattern(/^-?\d+(\.\d+)?$/)]],
  });

  pathCoordinates = signal<Coordinate[]>([]);
  userCoordinates = signal<Coordinate>({ x: 0, y: 0 });
  status = signal<Status>({});
  trackCurrentLine = signal(false);
  loading = signal(false);

  ngOnInit(): void {
    if (!isPlatformBrowser(this.platformId)) {
      return;
    }

    this.loading.set(true);

    this.trackerApiService
      .getPathCoordinates()
      .pipe(
        catchError((error) => {
          console.error('Error fetching path coordinates:', error);
          this.loading.set(false);
          throw error;
        }),
      )
      .subscribe((coordinates) => {
        this.pathCoordinates.set(coordinates);

        if (coordinates.length > 0) {
          const { x, y } = coordinates[0];
          this.coordinateForm.setValue({ x, y });
          this.handleUpdateLocation();
        }
      });
  }

  handleResetCurrentLine() {
    if (!isPlatformBrowser(this.platformId)) return;

    localStorage.removeItem('currentLineIndex');
  }

  handleUpdateLocation() {
    if (!isPlatformBrowser(this.platformId)) return;

    if (this.coordinateForm.invalid) {
      this.coordinateForm.markAllAsTouched();
      return;
    }

    const { x, y } = this.coordinateForm.value as Coordinate;

    this.userCoordinates.set({ x, y });
    this.loading.set(true);

    this.trackerApiService
      .getStatus(this.userCoordinates(), this.trackCurrentLine())
      .pipe(
        catchError((error) => {
          console.error('Error fetching status:', error);
          this.loading.set(false);
          throw error;
        }),
      )
      .subscribe((status) => {
        this.status.set(status);
        localStorage.setItem('currentLineIndex', status.currentLineIndex?.toString() || '0');
        this.loading.set(false);
      });
  }

  handleKeyDown(event?: KeyboardEvent) {
    if (event?.key === 'Enter') {
      this.handleUpdateLocation();
    }
  }
}
