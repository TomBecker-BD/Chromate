# Chromate

Chromate helps you embed a HTML5 user interface in an app. 

* The user interface is standard HTML5/CSS/JavaScript. 
* The UI runs in a [CefSharp](https://github.com/cefsharp/CefSharp) browser control. 
* The back-end presents a RESTful web API. 
* Chromate provides the glue to handle resource requests from CefSharp and route them to 
the web API. 
* There is no TCP/IP transport overhead and latency. 
* There is no server to install or manage. 

This project is in the form of an example, a Toaster app. A toaster is simple, but it has multiple UI controls, and the toasting process is asynchronous. This example uses: 
* [Angular 4](https://angular.io/) UI
* [RxJS](http://reactivex.io/rxjs/) - reactive extensions for JavaScript
* ASP.NET web API
* C#/.NET back-end
* [Rx](https://msdn.microsoft.com/en-us/library/hh242985(v=vs.103).aspx) - reactive extensions for .NET
* Build integration using [MSBuild.Npm](https://github.com/kmees/MSBuild.NodeTools)

## Building and Running

Currently the Toaster example can be built using Visual Studio 2015 and NodeJS (6.9.x or later). It runs on 64-bit Windows. 

To build and run just the user interface: 

* Navigate to the `ToasterHTML` folder
* `npm install`
* `npm start`

To build the complete C#/.NET application: 
* Open `Toaster.sln` in Visual Studio
* Make sure the target platform is `x64`
* Select `ToasterApp` as the Startup Project
* Choose Build > Rebuild Solution

## Using Chromate

To use Chromate in another Windows application: 

* Projects with "Chromate" in the name can be used by reference. 
* Projects with "Toaster" in the name are examples. You can create similar projects and selectively copy the code you want. 

The build integration uses a custom MSBuild task in `ToasterHTML.csproj`. Unload the project and search for `NpmRun` to see where it was added. 

The Toaster app happens to use Ninject, NLog4, NUnit 3 and Rhino Mocks. You don't have to. Chromate does not use them, except for one unit test project. 

## Wish List

* Optimize MSBuild integration - sometimes it triggers on incremental builds
* Enable building the HTML5 UI on a TFS/VSTS server
* UI test integrated app
  * Microsoft Test Manager has a Selenium plugin
* Web socket support
* CefSharp support for Touch Keyboard
  * Need to expose input field type to Accessibility API
* Refactor example code for better reusability
  * Does the web API requester have to be in the web API project?
  * Can EnableHighDPISupport be called in the form instead of the main program? 
  * NuGet package for MSBuild.NPMRun
  * What else can be split off into NuGet packages?

Suggestions are welcome. Please create an issue or a pull request. 

Would love to see similar projects for other environments. 
