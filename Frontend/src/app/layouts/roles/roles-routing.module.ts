import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { RolePageComponent } from "./role-page/role-page.component";
import { AdminGuard } from "../../guards/admin.guard";
import { CompanyOwnerGuard } from "../../guards/companyOwner.guard";
import { HomeOwnerGuard } from "../../guards/homeOwner.guard";
import { UpdateProfilePictureComponent } from "../../business-components/update-profile-picture/update-profile-picture.component";

const routes: Routes = [
  {
    path: "",
    component: RolePageComponent,
    children: [
      {
        path: "admin",
        canActivate: [AdminGuard],
        loadChildren: () =>
          import("../../layouts/admin/admin.module").then((m) => m.AdminModule),
      },
      {
        path: "company-owner",
        canActivate: [CompanyOwnerGuard],
        loadChildren: () =>
          import("../../layouts/company-owner/company-owner.module").then(
            (m) => m.CompanyOwnerModule
          ),
      },
      {
        path: "home-owner",
        canActivate: [HomeOwnerGuard],
        loadChildren: () =>
          import("../../layouts/home-owner/home-owner.module").then(
            (m) => m.HomeOwnerModule
          ),
      },
      {
        path: "home-management/:id",
        loadChildren: () => import('../home-management/home-management.module').then(m => m.HomeManagementModule)
      },
      {
        path: "update-profile-picture",
        canActivate: [HomeOwnerGuard],
        component: UpdateProfilePictureComponent,
      }
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class RolesRoutingModule {}
