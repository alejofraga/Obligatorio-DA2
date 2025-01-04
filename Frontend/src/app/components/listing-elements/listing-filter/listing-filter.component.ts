import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { InputComponent } from '../../input/input.component';

@Component({
  selector: 'app-listing-filter',
  templateUrl: './listing-filter.component.html',
  styles: [],
  standalone: true,
  imports: [CommonModule, InputComponent]
})
export class ListingFilterComponent {
  @Input() filterOptions: string[] = [];
  @Output() filterChange = new EventEmitter<{ [key: string]: string }>();

  filterValues: { [key: string]: string } = {};

  onFilterValueChange(option: string, value: string) {
    this.filterValues[option] = value;
    this.emitFilterChange();
  }

  emitFilterChange() {
    this.filterChange.emit(this.filterValues);
  }

  capitalizeFirstLetter(value: string): string {
    return value.charAt(0).toUpperCase() + value.slice(1);
  }
}