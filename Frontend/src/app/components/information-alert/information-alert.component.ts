import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-information-alert',
  templateUrl: './information-alert.component.html',
  styles: [],
  standalone: true
})
export class InformationAlertComponent {
  @Input() message: string = '';
  @Input() type: 'success' | 'error' | 'warning' | 'info' = 'info';

  get alertClass(): string {
    switch (this.type) {
      case 'success':
        return 'alert alert-success text-center';
      case 'error':
        return 'alert alert-danger text-center';
      case 'warning':
        return 'alert alert-warning text-center';
      case 'info':
      default:
        return 'alert alert-info text-center';
    }
  }
}