import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';


import { MainRoutingModule } from './main-routing.module';
import { MainPageComponent } from './main-page/main-page.component';
import { SmartHomeHeaderComponent } from '../../business-components/smarthome-header/smarthome-header.component';
import { CardComponent } from '../../components/card/card.component';
import { AdminModule } from '../admin/admin.module';
import { RouterModule } from '@angular/router';


@NgModule({
  declarations: [
    MainPageComponent,
  ],
  imports: [
    CommonModule,
    MainRoutingModule,
    SmartHomeHeaderComponent,
    CardComponent,
    AdminModule,
    RouterModule
  ]
})
export class MainModule { }
