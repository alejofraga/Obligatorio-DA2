import { Component, Input, Output, EventEmitter} from "@angular/core";

@Component({
  standalone: true,
  imports: [],
  selector: 'app-listing-button',
  templateUrl: './listing-button.component.html',
  styles: ``
})
export class ListingButtonComponent {
  @Input({ required: true }) title!: string;
  @Input({ required: true }) class!: string;
  @Input ({ required: true }) currentOffset!: number;
  @Input({ required: true }) changeOffset!: number;
  @Input() disabled: boolean = false;
  @Output() changeOffsetEvent = new EventEmitter<number>();

  onButtonClick() {
    if (!this.disabled) {
      if (this.changeOffset + this.currentOffset < 0) {
        this.changeOffsetEvent.emit(this.currentOffset)
      }
      this.changeOffsetEvent.emit(this.currentOffset + this.changeOffset);
    }
  }
}
