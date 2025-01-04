import { Component, DoCheck, Input, OnInit, Output, EventEmitter} from "@angular/core";
import { FormGroup, ReactiveFormsModule } from "@angular/forms";
import { InputComponent } from "../../input/input.component";
import { NgIf } from "@angular/common";

@Component({
  selector: "app-form-input",
  standalone: true,
  imports: [ReactiveFormsModule, NgIf, InputComponent],
  templateUrl: "./form-input.component.html",
  styles: ``,
})
export class FormInputComponent {
  @Input() type: "text" | "number" | "password" | "file" = "text";
  @Input() label: string | null = null;
  @Input() placeholder: string | null = null;
  @Input({ required: true }) name!: string;
  @Input({ required: true }) form!: FormGroup;
  @Input({ required: true }) formField!: any;
  @Input({ required: true }) class!: string;
  @Input() multiple: boolean = false;
  @Output() change = new EventEmitter<any>();

  get error() {
    const control = this.form.get(this.name)!;

    if (!control.errors || !control.touched) {
      return null;
    }

    const errorKey = Object.keys(control.errors)[0];

    return this.formField[this.name][errorKey];
  }

  get value() {
    return this.form.get(this.name)!.value;
  }

  set value(value: string) {
    if (this.type !== 'file') {
      this.form.get(this.name)!.setValue(value);
    }
  }

  onChange(event: any): void {
    if (this.type === 'file') {
      this.form.get(this.name)!.setValue(event.target.files);
    }
    this.change.emit(event);
  }
}