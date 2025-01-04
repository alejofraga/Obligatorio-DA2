import { Component } from '@angular/core';
import { WelcomeComponent } from "../../components/welcome/welcome.component";

@Component({
  selector: 'app-welcome-admin',
  templateUrl: './welcome-admin.component.html',
  styles: ``,
  standalone: true,
  imports: [WelcomeComponent],
})
export class WelcomeAdminComponent {
  adminImagePath = "admin-home-config.jpg";
}
