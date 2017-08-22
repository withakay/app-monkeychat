## MonkeyChat

A Xamarin.Forms and Ably.io powered chat application.
This project shamelessly adapts the work of Microsoft's [James Montemagno](https://github.com/jamesmontemagno) & [Nish Anil](https://github.com/nishanil/Xamarin.Forms-Samples)

## Setup

Get your free API Key from [ably.io](https://www.ably.io).

* Log into your app dashboard
* Under “Your apps”, click on “Manage app” for any app you wish to use for this tutorial, or create a new one with the “Create New App” button
* Click on the “API Keys” tab
* Copy the secret “API Key” value from your Root key and store it so that you can use it later in this demo


Now you have your api key you need to put it in a couple of places in the code.

To do this, replace the api key place holder

  const string AblyApiKey = "YOUR_API_KEY_HERE";

 this needs to be done twice, once in

  MonkeyChat.Droid/Helpers/AblyMessenger.cs

and once in

  MonkeyChat.IOS/Helpers/AblyMessenger.cs


That is it!


## Learn More

* Get Started with Xamarin: http://xamarin.com


![](https://raw.githubusercontent.com/nishanil/Xamarin.Forms-Samples/master/Screenshots/Hero-DataTemplateSelector.png)



