import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { MainPageComponent } from "./main-page/main-page.component";

const routes: Routes = [
  {
    path: "",
    component: MainPageComponent,
  },
  {
    path: "",
    loadChildren: () => import('../roles/roles.module').then(m => m.RolesModule),
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class MainRoutingModule {}
