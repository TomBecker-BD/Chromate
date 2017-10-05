import { ToasterAppPage } from './app.po';

describe('toaster-app App', () => {
  let page: ToasterAppPage;

  beforeEach(() => {
    page = new ToasterAppPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to Toaster!');
  });
});
