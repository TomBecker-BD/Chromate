import { TestBed, inject } from '@angular/core/testing';

import { BaseRequestOptions, ConnectionBackend, Http, RequestOptions, XHRBackend } from '@angular/http';
import { MockBackend } from '@angular/http/testing';

import { ToasterService } from './toaster.service';

describe('ToasterService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        { provide: ConnectionBackend, useClass: MockBackend },
        { provide: XHRBackend, useClass: MockBackend },
        { provide: RequestOptions, useClass: BaseRequestOptions },
        Http,
        ToasterService
      ]
    });
  });

  it('should be created', inject([ToasterService], (service: ToasterService) => {
    expect(service).toBeTruthy();
  }));
});
