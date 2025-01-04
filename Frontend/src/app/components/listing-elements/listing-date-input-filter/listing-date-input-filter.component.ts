import { Component, EventEmitter, Input, Output } from "@angular/core";

@Component({
  selector: "app-listing-date-input-filter",
  templateUrl: "./listing-date-input-filter.component.html",
  styles: ``,
})
export class ListingDateInputFilterComponent {
  @Input() label: string = "";
  @Input() initalDate: string = "";

  @Output() filterChange = new EventEmitter<any>();

  filterValues: { [key: string]: any } = {};

  onFilterValueChange(option: any) {
    this.filterValues[this.label] = option;
    this.emitFilterChange();
  }

  emitFilterChange() {
    this.filterChange.emit(this.filterValues);
  }
}
