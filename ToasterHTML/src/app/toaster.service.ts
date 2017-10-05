import { Injectable } from '@angular/core';
import { Headers, Http, RequestOptions, Response } from '@angular/http';
import { Observable } from 'rxjs/Rx';

export interface ToasterStatus {
  setting: Number;
  content: String;
  toasting: Boolean;
  color: String;
}

const pollDelay = 100;
const defaultTimeout = 30;

@Injectable()
export class ToasterService {

  constructor(private http: Http) { }

  pollStatus(): Observable<ToasterStatus> {
    return Observable.timer(0, pollDelay)
      .exhaustMap(value => {
        const options = new RequestOptions({
          headers: new Headers({ timeout: value ? defaultTimeout : 0 })
        });
        return this.http.get('/api/toaster/status', options)
          .map(response => {
            const status: ToasterStatus = response.json();
            return status;
          });
      });
  }

  putSetting(setting: Number): Promise<ToasterStatus> {
    const body = { setting: setting };
    return this.http.put('/api/toaster/setting', body)
      .map(response => {
        const status: ToasterStatus = response.json();
        return status;
      })
      .toPromise<ToasterStatus>();
  }

  putContent(content: String, color: String): Promise<ToasterStatus> {
    const body = { content: content, color: color };
    return this.http.put('/api/toaster/content', body)
      .map(response => {
        const status: ToasterStatus = response.json();
        return status;
      })
      .toPromise<ToasterStatus>();
  }

  putToasting(toasting: Boolean): Promise<ToasterStatus> {
    const body = { toasting: toasting };
    return this.http.put('/api/toaster/toasting', body)
      .map(response => {
        const status: ToasterStatus = response.json();
        return status;
      })
      .toPromise<ToasterStatus>();
  }
}
