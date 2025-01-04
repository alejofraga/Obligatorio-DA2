import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CompanyOwnerRoutingModule } from './company-owner-routing.module';
import { CompanyOwnerPageComponent } from './company-owner-page/company-owner-page.component';
import { SidebarComponent } from '../../components/sidebar/sidebar.component';
import { WelcomeCompanyOwnerComponentComponent } from '../../business-components/welcome-company-owner-component/welcome-company-owner-component.component';
import { DeviceRegistrationFormComponent } from '../../business-components/device-registration-form/device-registration-form.component';
import { FormComponent } from '../../components/form-elements/form/form.component';
import { FormInputComponent } from "../../components/form-elements/form-input/form-input.component";
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormButtonComponent } from "../../components/form-elements/form-button/form-button.component";
import { CreateCompanyFormComponent } from '../../business-components/create-company-form/create-company-form.component';
import { InputComponent } from '../../components/input/input.component';
import { ButtonComponent } from '../../components/button/button.component';
import { SelectComponent } from '../../components/select/select.component';
import { InformationAlertComponent } from '../../components/information-alert/information-alert.component';
import { CheckboxComponent } from '../../components/checkbox/checkbox.component';
import { ListImportersComponent } from '../../business-components/list-importers/list-importers.component';
import { ListingTableComponent } from "../../components/listing-elements/listing-table/listing-table.component";
import { ListingButtonComponent } from '../../components/listing-elements/listing-button/listing-button.component';
import { ListingFilterComponent } from '../../components/listing-elements/listing-filter/listing-filter.component';
import { ImporterMenuComponent } from '../../business-components/importer-menu/importer-menu.component';
import { ImportService } from '../../../backend/services/import.service';


@NgModule({
  declarations: [
    CompanyOwnerPageComponent,CreateCompanyFormComponent, DeviceRegistrationFormComponent, ListImportersComponent, ImporterMenuComponent
  ],
  imports: [
    CommonModule,
    CompanyOwnerRoutingModule,
    SidebarComponent,
    WelcomeCompanyOwnerComponentComponent,
    FormComponent,
    FormsModule,
    FormInputComponent,
    ReactiveFormsModule,
    FormButtonComponent,
    FormInputComponent,
    InputComponent,
    ButtonComponent,
    SelectComponent,
    InformationAlertComponent,
    CheckboxComponent,
    ListingTableComponent,
    ListingButtonComponent,
    ListingFilterComponent,
    InformationAlertComponent,

  ],
  providers: [ImportService]
})
export class CompanyOwnerModule { }
