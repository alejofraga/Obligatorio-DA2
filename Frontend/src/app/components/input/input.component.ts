import { Component, EventEmitter, Input, Output } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";

@Component({
  selector: "app-input",
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: "./input.component.html",
  styles: ``,
})
export class InputComponent {
  @Input() label: string | null = null;
  @Input() placeholder: string | null = null;
  @Input() type: "text" | "number" | "password" | "file" = "text";
  @Input() value: string | null = null;
  @Input() class: string | null = null;
  @Input() multiple: boolean = false;

  @Output() valueChange = new EventEmitter<string>();
  @Output() change = new EventEmitter<any>();

  public onValueChange(event: any): void {
    if (this.type === 'file') {
      this.change.emit(event);
    } else {
      this.valueChange.emit(event.target.value);
    }
  }
}