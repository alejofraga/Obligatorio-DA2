import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { AdminRoutingModule } from "./admin-routing.module";
import { AdminPageComponent } from "./admin-page/admin-page.component";
import { SidebarComponent } from "../../components/sidebar/sidebar.component";
import { WelcomeAdminComponent } from "../../business-components/welcome-admin/welcome-admin.component";
import { RegisterUserComponent } from "../../business-components/register-user/register-user.component";
import { ButtonComponent } from "../../components/button/button.component";
import { InputComponent } from "../../components/input/input.component";
import { FormComponent } from "../../components/form-elements/form/form.component";
import { FormInputComponent } from "../../components/form-elements/form-input/form-input.component";
import { FormButtonComponent } from "../../components/form-elements/form-button/form-button.component";
import { ListingTableComponent } from "../../components/listing-elements/listing-table/listing-table.component";
import { ListingButtonComponent } from "../../components/listing-elements/listing-button/listing-button.component";
import { ListingFilterComponent } from "../../components/listing-elements/listing-filter/listing-filter.component";
import { ListingAccountsComponent } from "../../business-components/listing-accounts/listing-accounts.component";
import { InformationAlertComponent } from "../../components/information-alert/information-alert.component";
import { ListingCompaniesComponent } from "../../business-components/listing-companies/listing-companies.component";
import { RemoveAdminComponent } from "../../business-components/remove-admin/remove-admin.component";

@NgModule({
  declarations: [AdminPageComponent, RegisterUserComponent, ListingAccountsComponent, ListingCompaniesComponent, RemoveAdminComponent],
  imports: [
    CommonModule,
    AdminRoutingModule,
    SidebarComponent,
    WelcomeAdminComponent,
    ButtonComponent,
    InputComponent,
    FormComponent,
    FormInputComponent,
    FormButtonComponent,
    ListingTableComponent,
    ListingButtonComponent,
    ListingFilterComponent,
    InformationAlertComponent
  ],
})
export class AdminModule {}
