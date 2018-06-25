using System;
using System.Threading.Tasks;
using System.Windows;

namespace WPFGlobalExceptionHandling
{
    /// <summary>
    /// For adding global exception handling to Windows Presentation Foundation (WPF) applications
    /// <para>For 'App' or 'Application' classes that interface IWPFGlobalExceptionHandler</para>
    /// </summary>
    public static class WPFGlobalExceptionHandler
    {
        /// <summary>
        /// Adds global exception handling on all UI threads, non-UI threads, task scheduler threads, and for DataBinding exceptions thrown from DataBinding errors
        /// </summary>
        /// <remarks>
        /// Any WPF application that implements the IWPFGlobalExceptionHandler interface can call this.UseGlobalExceptionHandling() in the constructor and will
        /// then use this WPFGlobalExceptionHandler to handle all uncaught exceptions; the implementation of the HandleException() and HandleUnrecoverableException()
        /// methods will specify what the app should do on normal or unrecoverable exceptions
        /// </remarks>
        public static void UseGlobalExceptionHandling<App>(this App app) where App : Application, IWPFGlobalExceptionHandler
        {
            IWPFGlobalExceptionHandler exceptionHandler = app;

            try
            {
                // handles non-UI thread exceptions thrown; the app terminates after unhandled exceptions are caught here
                AppDomain.CurrentDomain.UnhandledException += (s, e) => HandleException(exceptionHandler, e.ExceptionObject as Exception, e.IsTerminating);

                // handles UI dispatcher thread exceptions thrown
                app.DispatcherUnhandledException += (s, e) => e.Handled = HandleException(exceptionHandler, e.Exception);

                // handles domain-wide exceptions where a task scheduler is used for asynchronous operations
                TaskScheduler.UnobservedTaskException += (s, e) => { if (HandleException(exceptionHandler, e.Exception)) e.SetObserved(); };

                // allows DataBinding errors to throw exceptions, which are then caught by DispatcherUnhandledException
                DataBindingErrorHandler.ThrowExceptionsWithDataBindingErrors();
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Unable to use global exception handling.\n{e.Message}");
            }
        }
        public static Exception GetRootException(this Exception e)
        {
            Exception rootException = e;

            // get the root (deepest level) exception thrown
            while (rootException.InnerException != null)
            {
                rootException = rootException.InnerException;
            }

            return rootException;
        }

        private static bool HandleException(IWPFGlobalExceptionHandler exceptionHandler, Exception exception, bool isTerminating = false)
        {
            if (isTerminating)
            {
                // run the interface-implemented custom handling
                exceptionHandler.HandleUnrecoverableException(exception);

                Application.Current.Shutdown();

                return false;
            }
            else
            {
                // run the interface-implemented custom handling
                exceptionHandler.HandleException(exception);

                return true;
            }
        }
    }
}
