import { Component, EventEmitter, Input, Output } from "@angular/core";
import { CommonModule } from "@angular/common";

interface RadioInputOption {
  title: string;
  name: string;
  class: string;
  checked: boolean;
  value: any;
}

@Component({
  selector: "app-radio-input",
  templateUrl: "./radio-input.component.html",
  standalone: true,
  styles: [],
  imports: [CommonModule],
})
export class RadioInputComponent {
  @Input() groupClass: string = "";
  @Input() hideCircle: boolean = false;
  @Input({ required: true }) inputOptions!: RadioInputOption[];

  @Output() valueChange = new EventEmitter<any>();

  onValueChange(option: any) {
    this.inputOptions.forEach((opt) => (opt.checked = false));
    option.checked = true;
    this.valueChange.emit(option);
  }
}
