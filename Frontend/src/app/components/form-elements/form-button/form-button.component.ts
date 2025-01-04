import { Component, Input } from "@angular/core";
import { ButtonComponent } from "../../button/button.component";

@Component({
  selector: "app-form-button",
  standalone: true,
  imports: [ButtonComponent],
  templateUrl: "./form-button.component.html",
  styles: ``,
})
export class FormButtonComponent {
  @Input({ required: true }) title!: string;
  @Input({ required: true }) class!: string;
}
  
