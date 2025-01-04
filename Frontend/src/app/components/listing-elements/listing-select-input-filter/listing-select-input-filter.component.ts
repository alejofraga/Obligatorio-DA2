import { Component, EventEmitter, Input, Output } from "@angular/core";

@Component({
  selector: "app-listing-select-input-filter",
  templateUrl: "./listing-select-input-filter.component.html",
  styles: ``,
})
export class ListingSelectInputFilterComponent {
	@Input() groupClass: string = "";
	@Input() selectClass: string = "";
  @Input() options: { value: string; label: string }[] = [];
  @Input() selectedValue: string | null = null;
  @Input() label: string = "";

  @Output() filterChange = new EventEmitter<any>();

  filterValues: { [key: string]: any } = {};

  public onFilterValueChange(option: string): void {
    this.filterValues[this.label] = option;
    this.emitFilterChange();
  }

  emitFilterChange() {
    this.filterChange.emit(this.filterValues);
  }
}
