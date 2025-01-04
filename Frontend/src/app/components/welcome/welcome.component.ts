import { Component, Input } from "@angular/core";
import { CommonModule } from "@angular/common";

@Component({
  selector: "app-welcome",
  templateUrl: "./welcome.component.html",
  styles: ``,
  standalone: true,
  imports: [CommonModule],
})
export class WelcomeComponent {
  @Input({ required: true }) title!: string;
  @Input({ required: true }) imagePath!: string;
  @Input() hasPermissions: boolean = true;

}
