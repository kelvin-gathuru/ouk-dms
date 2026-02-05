import { Component, inject, Input } from '@angular/core';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { LoadingService } from './loading-service';

@Component({
  selector: 'app-loading-indicator',
  templateUrl: './loading-indicator.component.html',
  styleUrls: ['./loading-indicator.component.scss'],
  standalone: true,
  imports: [
    MatProgressBarModule,
    MatProgressSpinnerModule
  ]
})
export class LoadingIndicatorComponent {
  loaderService = inject(LoadingService);
}


