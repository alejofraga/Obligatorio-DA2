import { Component, Input, Output, EventEmitter } from "@angular/core";
import { FormGroup, ReactiveFormsModule } from "@angular/forms";


@Component({
  selector: "app-form",
  templateUrl: "./form.component.html",
  styles: ``,
  standalone: true,
  imports: [ReactiveFormsModule],
})
export class FormComponent {
  @Input({ required: true }) form!: FormGroup;

  @Output() ngSubmit = new EventEmitter<any>();

  public handleSubmit() {
    const isValid = this.form.valid;

    if (!isValid) {
      this.form.markAllAsTouched();
      return;
    }

    this.ngSubmit.emit(this.form.value);
  }
}
