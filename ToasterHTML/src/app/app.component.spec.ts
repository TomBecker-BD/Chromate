import { TestBed, async } from '@angular/core/testing';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';

import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import {
  MdCardModule,
  MdCheckboxModule,
  MdFormFieldModule,
  MdInputModule,
  MdSliderModule,
  MdToolbarModule
} from '@angular/material';

import { AppComponent } from './app.component';
import { ToasterStatus } from './toaster.service';

@Component({
  selector: 'app-toaster',
  templateUrl: './toaster/toaster.component.html',
  styleUrls: ['./toaster/toaster.component.css']
})
class ToasterStubComponent {
  status: ToasterStatus = {
    setting: 10,
    content: 'Bagel',
    toasting: false,
    color: 'White'
  };
  constructor() { }
}

describe('AppComponent', () => {
  beforeEach(async(() => {
    const toasterServiceStub = {};
    TestBed.configureTestingModule({
      declarations: [
        AppComponent, ToasterStubComponent
      ],
      imports: [
        FormsModule,
        NoopAnimationsModule,
        MdCardModule,
        MdCheckboxModule,
        MdFormFieldModule,
        MdInputModule,
        MdSliderModule,
        MdToolbarModule
      ],
    }).compileComponents();
  }));

  it('should create the app', async(() => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app).toBeTruthy();
  }));

  it(`should have as title 'app'`, async(() => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app.title).toEqual('app');
  }));

  it('should render the title in the toolbar', async(() => {
    const fixture = TestBed.createComponent(AppComponent);
    fixture.detectChanges();
    const compiled = fixture.debugElement.nativeElement;
    expect(compiled.querySelector('md-toolbar span').textContent).toContain('Welcome to Toaster!');
  }));
});
