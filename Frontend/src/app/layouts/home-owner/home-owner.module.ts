import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { HomeOwnerRoutingModule } from "./home-owner-routing.module";
import { HomeOwnerPageComponent } from "./home-owner-page/home-owner-page.component";
import { SidebarComponent } from "../../components/sidebar/sidebar.component";
import { WelcomeHomeOwnerComponentComponent } from "../../business-components/welcome-home-owner-component/welcome-home-owner-component.component";
import { CreateHomeFormComponent } from "../../business-components/create-home-form/create-home-form.component";
import { FormComponent } from "../../components/form-elements/form/form.component";
import { FormButtonComponent } from "../../components/form-elements/form-button/form-button.component";
import { FormInputComponent } from "../../components/form-elements/form-input/form-input.component";
import { MyHomesComponent } from "../../business-components/my-homes/my-homes.component";
import { ListDevicesComponent } from "../../business-components/list-devices/list-devices.component";
import { ListingTableComponent } from "../../components/listing-elements/listing-table/listing-table.component";
import { ListingButtonComponent } from "../../components/listing-elements/listing-button/listing-button.component";
import { ListingFilterComponent } from "../../components/listing-elements/listing-filter/listing-filter.component";
import { InformationAlertComponent } from "../../components/information-alert/information-alert.component";
import { ListSupportedDevicesComponent } from "../../business-components/list-supported-devices/list-supported-devices.component";
import { HomeManagementModule } from "../home-management/home-management.module";
import { ListNotificationsComponent } from "../../business-components/list-notifications/list-notifications.component";
import { ListingRadioInputFilterComponent } from "../../components/listing-elements/listing-radio-input-filter/listing-radio-input-filter.component";
import { ListingSelectInputFilterComponent } from "../../components/listing-elements/listing-select-input-filter/listing-select-input-filter.component";
import { SelectComponent } from "../../components/select/select.component";
import { DateInputComponent } from "../../components/date-input/date-input.component";
import { ListingDateInputFilterComponent } from "../../components/listing-elements/listing-date-input-filter/listing-date-input-filter.component";
import { LoadingComponent } from "../../business-components/loading/loading.component";

@NgModule({
  declarations: [
    HomeOwnerPageComponent,
    CreateHomeFormComponent,
    ListDevicesComponent,
    ListSupportedDevicesComponent,
    MyHomesComponent,
    ListNotificationsComponent,
    ListingSelectInputFilterComponent,
    ListingDateInputFilterComponent,
  ],
  imports: [
    CommonModule,
    HomeOwnerRoutingModule,
    SidebarComponent,
    WelcomeHomeOwnerComponentComponent,
    FormComponent,
    FormButtonComponent,
    FormInputComponent,
    ListingTableComponent,
    ListingRadioInputFilterComponent,
    ListingButtonComponent,
    ListingFilterComponent,
    InformationAlertComponent,
    HomeManagementModule,
    SelectComponent,
    DateInputComponent,
    LoadingComponent
  ],
})
export class HomeOwnerModule {}
