import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";

import { AppRoutingModule } from "./app-routing.module";
import { RouterModule } from '@angular/router';
import { AppComponent } from "./app.component";
import { AuthModule } from "./layouts/auth/auth.module";
import { HTTP_INTERCEPTORS, HttpClientModule } from "@angular/common/http";
import { MainModule } from "./layouts/main/main.module";
import { AuthInterceptor } from "../auth.interceptor";
import { SelectComponent } from "./components/select/select.component";
import { DateInputComponent } from "./components/date-input/date-input.component";
import { InformationAlertComponent } from "./components/information-alert/information-alert.component";
import { FormComponent } from "./components/form-elements/form/form.component";
import { LoadingComponent } from "./business-components/loading/loading.component";

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AuthModule,
    MainModule,
    HttpClientModule,
    RouterModule,
    SelectComponent,
    DateInputComponent,
    InformationAlertComponent,
    FormComponent,
    LoadingComponent
],
providers: [
  {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
  }
],
  bootstrap: [AppComponent],
})
export class AppModule {}
