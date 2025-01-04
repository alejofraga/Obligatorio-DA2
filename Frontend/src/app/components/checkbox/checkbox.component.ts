import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-checkbox',
  templateUrl: './checkbox.component.html',
  styles: [],
  imports: [CommonModule],
  standalone: true
})
export class CheckboxComponent {
  @Input() label: string | null = null;
  @Input() checked: boolean = false;
  @Input() disabled: boolean = false;
  @Output() checkedChange = new EventEmitter<boolean>();

  public id: string;

  constructor() {
    this.id = `checkbox-${this.generateId()}`;
  }

  generateId(): string {
    const part1 = Date.now().toString(35);
    const part2 = Math.random().toString(36).slice(2);
    return part1 + part2;
  }

  public onCheckedChange(event: any): void {
    this.checked = event.target.checked;
    this.checkedChange.emit(this.checked);
  }
}