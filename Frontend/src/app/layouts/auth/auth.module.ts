import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthRoutingModule } from './auth-routing.module';
import { LoginFormComponent } from '../../business-components/login-form/login-form.component';
import { FormComponent } from '../../components/form-elements/form/form.component';
import { FormButtonComponent } from '../../components/form-elements/form-button/form-button.component';
import { FormInputComponent } from '../../components/form-elements/form-input/form-input.component';
import { AuthenticationPageComponent } from './authentication-page/authentication-page.component';
import { HttpClientModule } from '@angular/common/http';
import { RegisterHomeownerFormComponent } from '../../business-components/register-homeowner-form/register-homeowner-form.component';
import { ButtonComponent } from "../../components/button/button.component";
import { ReactiveFormsModule } from '@angular/forms';
import { InformationAlertComponent } from '../../components/information-alert/information-alert.component';



@NgModule({
  declarations: [LoginFormComponent, RegisterHomeownerFormComponent, AuthenticationPageComponent],
  imports: [
    CommonModule,
    AuthRoutingModule,
    FormComponent,
    FormButtonComponent,
    FormInputComponent,
    HttpClientModule,
    ButtonComponent,
    ReactiveFormsModule,
    InformationAlertComponent
],
  exports: []
})
export class AuthModule { }
