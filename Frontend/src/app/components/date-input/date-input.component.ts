import { Component, EventEmitter, Input, Output } from "@angular/core";
import { FormsModule } from "@angular/forms";

@Component({
  selector: "app-date-input",
  templateUrl: "./date-input.component.html",
  standalone: true,
  styleUrls: [],
  imports: [FormsModule],
})
export class DateInputComponent {
  @Input() initalDate: string = "";
  @Input() label: string = "";

  @Output() valueChange = new EventEmitter<any>();

  onValueChange(event: any) {
    this.valueChange.emit(event.target.value);
  }
}
