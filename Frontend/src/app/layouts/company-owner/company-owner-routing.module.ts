import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CompanyOwnerPageComponent } from './company-owner-page/company-owner-page.component';
import { WelcomeCompanyOwnerComponentComponent } from '../../business-components/welcome-company-owner-component/welcome-company-owner-component.component';
import { CreateCompanyFormComponent } from '../../business-components/create-company-form/create-company-form.component';
import { ListDevicesComponent } from '../../business-components/list-devices/list-devices.component';
import { DeviceRegistrationFormComponent } from "../../business-components/device-registration-form/device-registration-form.component";
import { ListImportersComponent } from '../../business-components/list-importers/list-importers.component';
import { ListSupportedDevicesComponent } from '../../business-components/list-supported-devices/list-supported-devices.component';
import { ImporterMenuComponent } from '../../business-components/importer-menu/importer-menu.component';

const routes: Routes = [
  {
    path: "",
    component: CompanyOwnerPageComponent,
    children: [
      {path: "", redirectTo: "welcome", pathMatch: "full"},
      {path: "welcome", component: WelcomeCompanyOwnerComponentComponent},
      {path: "create-company", component: CreateCompanyFormComponent},
      {path: "list-devices", component: ListDevicesComponent},
      {path: "import-devices", component: ListImportersComponent},
      {path: "list-supported-devices", component: ListSupportedDevicesComponent},
      {path: "device-registration", component: DeviceRegistrationFormComponent},
      {path: "importer/:name", component: ImporterMenuComponent}
    ]
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class CompanyOwnerRoutingModule {}
