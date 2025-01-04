import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RolesRoutingModule } from './roles-routing.module';
import { RolePageComponent } from './role-page/role-page.component';
import { SmartHomeHeaderComponent } from "../../business-components/smarthome-header/smarthome-header.component";
import { UpdateProfilePictureComponent } from '../../business-components/update-profile-picture/update-profile-picture.component';
import { FormComponent } from '../../components/form-elements/form/form.component';
import { FormInputComponent } from '../../components/form-elements/form-input/form-input.component';
import { FormButtonComponent } from '../../components/form-elements/form-button/form-button.component';
import { InputComponent } from '../../components/input/input.component';
import { ButtonComponent } from '../../components/button/button.component';
import { InformationAlertComponent } from '../../components/information-alert/information-alert.component';


@NgModule({
  declarations: [
    RolePageComponent,
    UpdateProfilePictureComponent
  ],
  imports: [
    CommonModule,
    RolesRoutingModule,
    SmartHomeHeaderComponent,
    FormComponent,
    FormInputComponent,
    FormButtonComponent,
    InputComponent,
    ButtonComponent,
    InformationAlertComponent
]
})
export class RolesModule { }
