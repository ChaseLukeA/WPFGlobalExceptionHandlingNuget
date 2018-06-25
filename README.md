# Windows Presentation Foundation (WPF) Global Exception Handling
For handling exceptions in the `App.xaml.cs` file of any .NET Framework WPF Application.

## About
This project was created with a desire to handle all exceptions (or unhandled exceptions if that's you're desire) at a global level in WPF applications

## Usage
* Add WPFGlobalExceptionHandling to your list of references
* Add `using WPFGlobalExceptionHandling;` to the header of your `App.xaml.cs` file
* Add the interface `IWPFGlobalExceptionHandler` to the `App.xaml.cs` class
	* Example: `public partial class App : Application, IWPFGlobalExceptionHandler { }`
* Implement the two interface member methods that `IWPFGlobalExceptionHandler` interfaces
	* `HandleException()` handles "normal" unhandled exeptions that can be safely handled
		* Example:
		```
	        public void HandleException(Exception e)
			{
				// logs the exception, including inner exceptions
				logger.Error(e);

				// shows a message box with the root exception
				MessageBox.Show(e.GetRootException().Message, "Uh-oh, something went wrong!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		```
	* `HandleUnrecoverableException()` handles "unrecoverable" unhandled exeptions and then closes the application
		* Example:
		````
			public void HandleUnrecoverableException(Exception e)
			{
				// logs the exception, including inner exceptions
				logger.Fatal(e);

				// shows a message box with the root exception
				MessageBox.Show($"The application is now going to close.\n\n'{e.GetRootException().Message}'", "Unrecoverable Error!", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		```
* In the `App.xaml.cs` constructor, add a call to `this.UseGlobalExceptionHandling();`
	* This uses the extension method in WPFGlobalExceptionHandling for Application classes interfacing IWPFGlobalExceptionHandler
	* Four different handlers are used to handle exceptions globally:
		1. `AppDomain.CurrentDomain.UnhandledException` handles non-UI thread exceptions thrown; the app terminates after unhandled exceptions are caught here
		2. `app.DispatcherUnhandledException` handles UI dispatcher thread exceptions thrown
		3. `TaskScheduler.UnobservedTaskException` handles domain-wide exceptions where a task scheduler is used for asynchronous operations
		4. `DataBindingErrorHandler.ThrowExceptionsWithDataBindingErrors()` allows DataBinding errors to throw exceptions, which are then caught by DispatcherUnhandledException

* Now any time an exception occurs at any level, on any thread, it will be caught at `App` level and handled in one of the two interface methods, HandleException() or HandleUnrecoverableException()