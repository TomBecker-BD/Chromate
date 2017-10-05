import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { Observable } from 'rxjs/Rx';

import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import {
  MdCardModule,
  MdCheckboxModule,
  MdFormFieldModule,
  MdInputModule,
  MdSliderModule,
  MdToolbarModule
} from '@angular/material';

import { ToasterComponent } from './toaster.component';
import { ToasterStatus, ToasterService } from '../toaster.service';

describe('ToasterComponent', () => {
  let component: ToasterComponent;
  let fixture: ComponentFixture<ToasterComponent>;

  beforeEach(async(() => {
    const ToasterServiceStub = {
      pollStatus(): Observable<ToasterStatus> {
        return Observable.never<ToasterStatus>()
      }
    };
    TestBed.configureTestingModule({
      declarations: [ ToasterComponent ],
      providers: [
        { provide: ToasterService, useValue: ToasterServiceStub }
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
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ToasterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
