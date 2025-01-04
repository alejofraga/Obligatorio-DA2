import { Component } from '@angular/core';

@Component({
  selector: 'app-home-owner-page',
  templateUrl: './home-owner-page.component.html',
  styles: ``
})
export class HomeOwnerPageComponent {
  sideBarItems = [
    {title: "User", operations : ["List devices", "List supported devices"]},
    {title : "Home owner", operations : ["Create Home", "Homes", "Notifications"]}
  ]

}
