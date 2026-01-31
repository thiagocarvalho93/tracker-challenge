import { Component, inject, OnInit, signal } from '@angular/core';
import { Graph } from '../../components/graph/graph';
import { TrackerApiService } from '../../../services/tracker-api';
import { Coordinate } from '../../models/coordinate.type';
import { FormsModule, ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { catchError } from 'rxjs';
import { Status } from '../../models/status.type';
import { DecimalPipe } from '@angular/common';
import { MatButton } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDividerModule } from '@angular/material/divider';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatCardModule } from '@angular/material/card';
import { CommonModule } from '@angular/common';

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
    ReactiveFormsModule,
    CommonModule,
  ],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  fb = inject(FormBuilder);
  trackerApiService = inject(TrackerApiService);

  coordinateForm = this.fb.group({
    x: [0, [Validators.required, Validators.pattern(/^-?\d+(\.\d+)?$/)]],
    y: [0, [Validators.required, Validators.pattern(/^-?\d+(\.\d+)?$/)]],
  });
  pathCoordinates = signal<Array<Coordinate>>([]);
  userCoordinates = signal<Coordinate>({ x: 0, y: 0 });
  status = signal<Status>({});
  trackCurrentLine = signal<boolean>(false);
  loading = signal<boolean>(false);

  handleResetCurrentLine() {
    this.trackerApiService
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
        console.log('Fetched path coordinates:', coordinates);
        this.pathCoordinates.set(coordinates);
        const { x, y } = coordinates[0];
        this.coordinateForm.setValue({ x, y });
        this.handleUpdateLocation();
      });
  }
}
