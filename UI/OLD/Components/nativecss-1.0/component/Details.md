nativeCSS lets you style your xamarin iOS and Android (rev. 14) apps using just CSS.


To start with try styling all views to have a red background:

```csharp

	using Style;
	

	public override bool FinishedLaunching (UIApplication app, NSDictionary options)
	{
			...

			// load css file from bundle folder
			NativeCSS.StyleWithCSS("view{background:red}");
	}
```

nativeCSS supports most of the CSS 1, 2 and 3 specification. Here's an example snippet:

```css


/* style all iOS navigation bars and Android Action bars with a red gradient */

navbar{

-ios-tint-color:#811;

background:linear-gradient(bottom,red,#811);

}

/* style all iOS and Android buttons to be centered, and with different font and text color */


button {

width:90%;

margin:auto;

font-size:18px;

color:#eee;

-ios-padding: 5px 0px; // -ios- prefix targets ios only

}

```


Next, try loading a complete style sheet from your iOS or Android assets:

```csharp

	using Style;
	using Style.Enums;	

	public override bool FinishedLaunching (UIApplication app, NSDictionary options)
	{
			...

			// load css file from bundle folder
			NativeCSS.StyleWithCSS("styles.css",null,RemoteContentRefreshPeriod.Never);
	}
```

To simplify development you should enable live updating. 

Live updating reloads the CSS from any url on the fly, so you can make changes to the CSS and the app restyles automatically.


```csharp

	using Style;
	using Style.Enums;
	

	public override bool FinishedLaunching (UIApplication app, NSDictionary options)
	{
			...

			NativeCSS.StyleWithCSS("styles.css",
			                       new Uri("http://localhost:8000/styles.css"), 
			                       RemoteContentRefreshPeriod.EverySecond);
	}
```