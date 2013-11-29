nativeCSS styles your app with standard CSS. CSS can be loaded from a local file, then remotely updated via URL.


## iOS

```csharp

using Style;



	public override bool FinishedLaunching (UIApplication app, NSDictionary options)
	{

			...

			NativeCSS.StyleWithCSS("styles.css",
			                       new Uri("http://localhost:8000/styles.css"), 
			                       RemoteContentRefreshPeriod.EverySecond);

	}
```

## Android
```csharp

using Style;


	protected override void OnCreate (Bundle bundle)
	{
			...

			NativeCSS.StyleWithCSS("styles.css",
			                       new Uri("http://10.0.2.2:8000/styles.css"), 
			                       RemoteContentRefreshPeriod.EverySecond);

		
	}
}
```
## Remote Stylesheets

To simplify development and make post release tweaks you should host your CSS on a server. 

A sample stylesheet named style.css is included in the solution, which we can easily host as a local url using python:

```
cd Tasky
python -m SimpleHTTPServer 

```

The stylesheet is now available at http://localhost:8000/styles.css 

## Other Resources

* [Component Documentation](http://nativecss.com)

