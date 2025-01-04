import { Component } from '@angular/core';

@Component({
  selector: 'app-admin-page',
  templateUrl: './admin-page.component.html',
  styles: ``
})
export class AdminPageComponent 
{

  sideBarItems = [
    {title: "User", operations : ["List devices", "List supported devices"]},
    {title : "Admin", operations : ["Register Admin", "Register Company owner", "List accounts", "List companies", "Remove admin"]}
  ]

}
