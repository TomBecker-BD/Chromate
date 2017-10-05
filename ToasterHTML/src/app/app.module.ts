import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
  MdCardModule,
  MdCheckboxModule,
  MdFormFieldModule,
  MdInputModule,
  MdSliderModule,
  MdToolbarModule
} from '@angular/material';

import { AppComponent } from './app.component';
import { ToasterService } from './toaster.service';
import { ToasterComponent } from './toaster/toaster.component';

@NgModule({
  declarations: [
    AppComponent,
    ToasterComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpModule,
    BrowserAnimationsModule,
    MdCardModule,
    MdCheckboxModule,
    MdFormFieldModule,
    MdInputModule,
    MdSliderModule,
    MdToolbarModule
  ],
  providers: [ToasterService],
  bootstrap: [AppComponent]
})
export class AppModule { }
