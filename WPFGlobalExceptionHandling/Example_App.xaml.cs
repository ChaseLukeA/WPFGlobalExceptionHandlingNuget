using System;
using System.Windows;

namespace WPFGlobalExceptionHandling
{
    /// <summary>
    /// Interaction logic for ExampleApp.xaml
    /// </summary>
    public partial class Example_App : Application, IWPFGlobalExceptionHandler
    {
        public Example_App()
        {
            this.UseGlobalExceptionHandling();
        }

        public void HandleException(Exception e)
        {
            // shows a message box with the root exception
            MessageBox.Show(e.GetRootException().Message, "Something went wrong!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void HandleUnrecoverableException(Exception e)
        {
            // application going to close, prevent multiple subsequent events from presenting message boxes or logging errors
            MessageBox.Show($"Sorry, an unrecoverable error occured! The application is now going to close.\n\n'{e.GetRootException().Message}'", "Unrecoverable Error!", MessageBoxButton.OK, MessageBoxImage.Error );
        }
    }
}
