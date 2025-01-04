import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPageComponent } from './admin-page/admin-page.component';
import { WelcomeAdminComponent } from '../../business-components/welcome-admin/welcome-admin.component';
import { RegisterUserComponent } from '../../business-components/register-user/register-user.component';
import { ListingAccountsComponent } from '../../business-components/listing-accounts/listing-accounts.component';
import { ListingCompaniesComponent } from '../../business-components/listing-companies/listing-companies.component';
import { ListDevicesComponent } from '../../business-components/list-devices/list-devices.component';
import { RemoveAdminComponent } from '../../business-components/remove-admin/remove-admin.component';
import { ListSupportedDevicesComponent } from '../../business-components/list-supported-devices/list-supported-devices.component';

const routes: Routes = [
  {
    path: "",
    component: AdminPageComponent,
    children: [
      {path: "", redirectTo: "welcome", pathMatch: "full"},
      {path: "welcome", component: WelcomeAdminComponent},
      { path: "register-admin", component: RegisterUserComponent, data: { role: 'Admin' } },
      { path: "register-company-owner", component: RegisterUserComponent, data: { role: 'CompanyOwner' } },
      { path: "list-accounts", component: ListingAccountsComponent},
      { path: "list-companies", component: ListingCompaniesComponent},
      {path: "list-devices", component: ListDevicesComponent},
      {path: "remove-admin", component: RemoveAdminComponent},
      {path: "list-supported-devices", component: ListSupportedDevicesComponent}
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
