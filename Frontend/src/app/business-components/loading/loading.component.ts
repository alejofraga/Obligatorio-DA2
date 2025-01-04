import { Component } from '@angular/core';
import { LoadingService } from '../../../backend/services/loading.service';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-loading',
  templateUrl: './loading.component.html',
  styles: ``,
  standalone: true,
  imports: [CommonModule]
})
export class LoadingComponent {
  isLoading: Observable<boolean>;

  constructor(private loadingService: LoadingService) {
    this.isLoading = this.loadingService.loading$;
  }
}