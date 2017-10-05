import { browser, by, element } from 'protractor';

export class ToasterAppPage {
  navigateTo() {
    return browser.get('/');
  }

  getParagraphText() {
    return element(by.css('app-root md-toolbar span')).getText();
  }
}
