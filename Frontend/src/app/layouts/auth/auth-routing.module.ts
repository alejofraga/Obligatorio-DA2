import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthenticationPageComponent } from './authentication-page/authentication-page.component';
import { LoginFormComponent } from '../../business-components/login-form/login-form.component';
import { RegisterHomeownerFormComponent } from '../../business-components/register-homeowner-form/register-homeowner-form.component';

const routes: Routes = [
  {
    path: '',
    component: AuthenticationPageComponent,
    children: [
      {
        path: '',
        component: LoginFormComponent,
      },
      {
        path: 'register-home-owner',
        component: RegisterHomeownerFormComponent,
      }
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }