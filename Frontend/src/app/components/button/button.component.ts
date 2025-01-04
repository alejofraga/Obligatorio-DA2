import { Component, Input } from "@angular/core";
import { CommonModule } from "@angular/common";

@Component({
  selector: "app-button",
  standalone: true,
  imports: [CommonModule],
  templateUrl: "./button.component.html",
  styles: ``,
})
export class ButtonComponent {
  @Input({ required: true }) title!: string;
  @Input({ required: true }) onClick!: (arg: any) => any;
  @Input({ required: false }) class!: string;
  @Input() icon: string = '';
}
