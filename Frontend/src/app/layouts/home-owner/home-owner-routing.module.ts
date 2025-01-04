import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeOwnerPageComponent } from './home-owner-page/home-owner-page.component';
import { WelcomeHomeOwnerComponentComponent } from '../../business-components/welcome-home-owner-component/welcome-home-owner-component.component';
import { CreateHomeFormComponent } from '../../business-components/create-home-form/create-home-form.component';
import { MyHomesComponent } from '../../business-components/my-homes/my-homes.component';
import { ListDevicesComponent } from '../../business-components/list-devices/list-devices.component';
import { ListSupportedDevicesComponent } from "../../business-components/list-supported-devices/list-supported-devices.component";
import { ListNotificationsComponent } from "../../business-components/list-notifications/list-notifications.component";

const routes: Routes = [
  {
    path: "",
    component: HomeOwnerPageComponent,
    children: [
      { path: "", redirectTo: "welcome", pathMatch: "full" },
      { path: "welcome", component: WelcomeHomeOwnerComponentComponent },
      { path: "create-home", component: CreateHomeFormComponent },
      { path: "homes", component: MyHomesComponent },
      { path: "list-devices", component: ListDevicesComponent },
      { path: "notifications", component: ListNotificationsComponent },
      { path: "list-supported-devices", component: ListSupportedDevicesComponent },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HomeOwnerRoutingModule {}
