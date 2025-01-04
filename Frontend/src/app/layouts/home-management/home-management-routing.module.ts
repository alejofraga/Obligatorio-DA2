import { GrantPermissionsComponent } from '../../business-components/grant-permissions/grant-permissions.component';
import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { HomeManagmentPageComponent } from "./home-managment-page/home-managment-page.component";
import { WelcomeHomeManagementComponentComponent } from "../../business-components/welcome-home-management-component/welcome-home-management-component.component";
import { AddMemberComponentComponent } from "../../business-components/add-member-component/add-member-component.component";
import { ListingMembersComponent } from "../../business-components/listing-members/listing-members.component";
import { AddHardwareComponent } from "../../business-components/add-hardware/add-hardware.component";
import { ListingHardwareComponent } from "../../business-components/listing-hardware/listing-hardware.component";
import { CreateRoomComponent } from "../../business-components/create-room/create-room.component";
import { AddHardwareToRoomComponent } from "../../business-components/add-hardware-to-room/add-hardware-to-room.component";
import { ChangeHomeNameComponent } from "../../business-components/change-home-name/change-home-name.component";
import { ChangeHardwareNameComponent } from "../../business-components/change-hardware-name/change-hardware-name.component";
import { WelcomeHomeMangementNoPermissionsComponent } from "../../business-components/welcome-home-mangement-no-permissions/welcome-home-mangement-no-permissions.component";

const routes: Routes = [
  {
    path: "",
    component: HomeManagmentPageComponent,
    children: [
      { path: "", redirectTo: "welcome", pathMatch: "full" },
      { path: "welcome", component: WelcomeHomeManagementComponentComponent },
      { path: "welcome-no-permissions", component: WelcomeHomeMangementNoPermissionsComponent },
      { path: "add-member", component: AddMemberComponentComponent },
      { path: "list-members", component: ListingMembersComponent },
      { path: "add-device", component: AddHardwareComponent },
      { path: "list-devices", component: ListingHardwareComponent },
      { path: "create-room", component: CreateRoomComponent },
      { path: "add-device-to-room", component: AddHardwareToRoomComponent },
      { path: "change-home-name", component: ChangeHomeNameComponent },
      { path: "change-device-name" , component: ChangeHardwareNameComponent},
      { path: "grant-home-permissions", component: GrantPermissionsComponent}

    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HomeManagementRoutingModule {}
