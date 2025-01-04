import { Component, EventEmitter, Input, Output } from "@angular/core";
import { CommonModule } from "@angular/common";

@Component({
  selector: "app-select",
  templateUrl: "./select.component.html",
  styles: [],
  standalone: true,
  imports: [CommonModule],
})
export class SelectComponent {
  @Input() label: string | null = null;
  @Input() options: { value: string; label: string }[] = [];
  @Input() selectedValue: string | null = null;
  @Input() isMultiple: boolean = false;
  @Input() class: string | null = null;

  @Output() selectedValueChange = new EventEmitter<any>();

  public onValueChange(event: any): void {
    this.selectedValueChange.emit(event.target.value);

    if (this.isMultiple) {
      const selectedOptions = Array.from(event.target.selectedOptions).map(
        (option: any) => option.value
      );
      this.selectedValueChange.emit(selectedOptions);
    }
  }
}
