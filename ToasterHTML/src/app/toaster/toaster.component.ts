import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { ToasterStatus, ToasterService } from '../toaster.service';

@Component({
  selector: 'app-toaster',
  templateUrl: './toaster.component.html',
  styleUrls: ['./toaster.component.css']
})
export class ToasterComponent implements OnInit, OnDestroy {

  status: ToasterStatus = {
    setting: 10,
    content: 'Bagel',
    toasting: false,
    color: 'White'
  };

  private subscription: Subscription;

  constructor(private toasterService: ToasterService) { }

  ngOnInit() {
  }

  ngOnDestroy() {
    this.stopStatus();
  }

  private pollStatus(): void {
    if (!this.subscription) {
      this.subscription = this.toasterService.pollStatus().subscribe(
        (value: ToasterStatus) => this.updateStatus(value),
        (error: any) => this.handleError(error));
    }
  }

  private stopStatus(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
      this.subscription = null;
    }
  }

  private updateStatus(value: ToasterStatus): void {
    this.status.toasting = value.toasting;
    this.status.color = value.color;
    if (!this.status.toasting) {
      this.stopStatus();
    }
  }

  private handleError(error: any): void {
    console.log(error);
  }

  settingChanged() {
    console.log('setting', this.status.setting);
    this.toasterService.putSetting(this.status.setting)
      .then((value: ToasterStatus) => this.updateStatus(value),
      (error: any) => this.handleError(error));
  }

  contentChanged() {
    console.log('content', this.status.content);
    this.toasterService.putContent(this.status.content, '')
      .then((value: ToasterStatus) => this.updateStatus(value),
      (error: any) => this.handleError(error));
  }

  toastingChanged() {
    console.log('toasting', this.status.toasting);
    this.toasterService.putToasting(this.status.toasting)
      .then((value: ToasterStatus) => {
        this.updateStatus(value);
        if (this.status.toasting) {
          this.pollStatus();
        }
      },
      (error: any) => this.handleError(error));
  }
}
