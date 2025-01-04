import { Component } from "@angular/core";
import { WelcomeComponent } from "../../components/welcome/welcome.component";

@Component({
  selector: "app-welcome-home-owner-component",
  templateUrl: "./welcome-home-owner-component.component.html",
  styles: ``,
  standalone: true,
  imports: [WelcomeComponent],
})
export class WelcomeHomeOwnerComponentComponent {
  homeOwnerImagePath = "home-owner-welcome.png";
}
