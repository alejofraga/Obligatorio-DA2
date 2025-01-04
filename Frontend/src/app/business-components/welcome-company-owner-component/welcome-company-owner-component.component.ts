import { Component } from '@angular/core';
import { WelcomeComponent } from "../../components/welcome/welcome.component";

@Component({
  selector: 'app-welcome-company-owner-component',
  templateUrl: './welcome-company-owner-component.component.html',
  styles: ``,
  standalone: true,
  imports: [WelcomeComponent],
})
export class WelcomeCompanyOwnerComponentComponent {
 companyOwnerImagePath = "company-owner-welcome.png";
}
