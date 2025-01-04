import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { HomeManagementRoutingModule } from './home-management-routing.module';
import { HomeManagmentPageComponent } from './home-managment-page/home-managment-page.component';
import { SidebarComponent } from '../../components/sidebar/sidebar.component';
import { WelcomeComponent } from '../../components/welcome/welcome.component';
import { WelcomeHomeManagementComponentComponent } from '../../business-components/welcome-home-management-component/welcome-home-management-component.component';
import { AddMemberComponentComponent } from '../../business-components/add-member-component/add-member-component.component';
import { InputComponent } from '../../components/input/input.component';
import { ButtonComponent } from '../../components/button/button.component';
import { FormComponent } from '../../components/form-elements/form/form.component';
import { FormInputComponent } from '../../components/form-elements/form-input/form-input.component';
import { FormButtonComponent } from '../../components/form-elements/form-button/form-button.component';
import { ListingMembersComponent } from '../../business-components/listing-members/listing-members.component';
import { ListingFilterComponent } from '../../components/listing-elements/listing-filter/listing-filter.component';
import { ListingButtonComponent } from '../../components/listing-elements/listing-button/listing-button.component';
import { AddHardwareComponent } from '../../business-components/add-hardware/add-hardware.component';
import { ListingHardwareComponent } from '../../business-components/listing-hardware/listing-hardware.component';
import { GrantPermissionsComponent } from '../../business-components/grant-permissions/grant-permissions.component';
import { CheckboxComponent } from '../../components/checkbox/checkbox.component';
import { ListingTableComponent } from '../../components/listing-elements/listing-table/listing-table.component';
import { InformationAlertComponent } from '../../components/information-alert/information-alert.component';
import { CreateRoomComponent } from "../../business-components/create-room/create-room.component";
import { SelectComponent } from "../../components/select/select.component";
import { ChangeHomeNameComponent } from "../../business-components/change-home-name/change-home-name.component";
import { AddHardwareToRoomComponent } from "../../business-components/add-hardware-to-room/add-hardware-to-room.component";
import { ChangeHardwareNameComponent } from "../../business-components/change-hardware-name/change-hardware-name.component";
import { WelcomeHomeMangementNoPermissionsComponent } from "../../business-components/welcome-home-mangement-no-permissions/welcome-home-mangement-no-permissions.component";

@NgModule({
  declarations: [
    HomeManagmentPageComponent,
    WelcomeHomeManagementComponentComponent,
    AddMemberComponentComponent,
    ListingMembersComponent,
    AddHardwareComponent,
    ListingHardwareComponent,
    GrantPermissionsComponent,
    CreateRoomComponent,
    AddHardwareComponent,
    ChangeHomeNameComponent,
    AddHardwareToRoomComponent,
    ChangeHardwareNameComponent,
    WelcomeHomeMangementNoPermissionsComponent
  ],
  imports: [
    CommonModule,
    HomeManagementRoutingModule,
    SidebarComponent,
    WelcomeComponent,
    InputComponent,
    ButtonComponent,
    FormComponent,
    FormInputComponent,
    FormButtonComponent,
    ListingTableComponent,
    InformationAlertComponent,
    ListingFilterComponent,
    ListingTableComponent,
    ListingButtonComponent,
    CheckboxComponent,
    SelectComponent
  ],
})
export class HomeManagementModule {}
