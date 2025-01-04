import { Component, EventEmitter, Input, Output } from "@angular/core";
import { CommonModule } from "@angular/common";
import { RadioInputComponent } from "../../radio-input/radio-input.component";

@Component({
  selector: "app-listing-radio-input-filter",
  templateUrl: "./listing-radio-input-filter.component.html",
  standalone: true,
  styles: [],
  imports: [CommonModule, RadioInputComponent],
})
export class ListingRadioInputFilterComponent {
  @Input({ required: true }) inputOptions!: any[];
  @Input() groupClass: string = "";
  @Input() hideCircle: boolean = false;

  @Output() filterChange = new EventEmitter<any>();

  filterValues: { [key: string]: any } = {};

  onFilterValueChange(option: any) {
    this.filterValues[option.name] = option.value;
    this.emitFilterChange();
  }

  emitFilterChange() {
    this.filterChange.emit(this.filterValues);
  }
}
