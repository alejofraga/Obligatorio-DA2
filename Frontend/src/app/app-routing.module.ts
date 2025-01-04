import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  {
    path: '',
    canActivate: [AuthGuard],
    loadChildren: () => import('./layouts/main/main.module').then(m => m.MainModule)
  },
  {
    path: 'login',
    loadChildren: () => import('./layouts/auth/auth.module').then(m => m.AuthModule)
  },
  /*{
    path: '**',
    redirectTo: '',
    pathMatch: 'full'
  }
  */
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
