import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {DataService} from './core/data.service';
import {CustomersComponent} from './customers/customers.component';
import {RouterModule,Routes} from '@angular/router';
import {HttpModule} from '@angular/http';
import { CustomersGridComponent } from './customers/customers.grid.component';
import { CustomersEditComponent } from './customers/customers.edit.component';
import {FilterTextBoxComponent} from './shared/filter.textbox.component';
import {DataFilterService} from './core/filter.service';
import {FormsModule,ReactiveFormsModule} from '@angular/forms';
import {NavbarComponent} from './shared/navbar.component';
import {LoginComponent} from './users/login.component';
import {RegisterComponent} from './users/register.component';
import {AuthService} from './users/auth.service';


const routes:Routes=[
  {path: 'customers',component:CustomersComponent},
  {path: 'customers/:id',component:CustomersEditComponent},
  {path: 'login',component:LoginComponent},
  {path: 'register',component:RegisterComponent},
  {path: '**',pathMatch:'full', redirectTo: '/customers'}
]
@NgModule({
  declarations: [
    AppComponent,
    CustomersComponent,
    CustomersGridComponent,
    FilterTextBoxComponent,
    CustomersEditComponent,
    NavbarComponent,
    LoginComponent,
    RegisterComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    AppRoutingModule,
    ReactiveFormsModule,
    HttpModule,
    RouterModule.forRoot(routes)
  ],
  providers: [DataService,DataFilterService,AuthService],
  bootstrap: [AppComponent]
})
export class AppModule { }
