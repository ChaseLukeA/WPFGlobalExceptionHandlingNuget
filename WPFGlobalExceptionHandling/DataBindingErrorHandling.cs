using System.Diagnostics;

/// <summary>
/// Exception handling for WPF applications
/// </summary>
namespace WPFGlobalExceptionHandling
{
    /// <summary>
    /// Utility for adding the ability to throw exceptions with DataBinding errors in Windows Presentation Foundation (WPF) applications
    /// <para>Since DataBinding errors that occur do not throw exceptions, this handler accesses the error message and creates/throws an exception from it</para>
    /// </summary>
    /// <remarks>
    /// 10/31/2017 - Luke A Chase - Created class and private trace listener to throw exceptions when binding errors occur; this has been very
    ///                             helpful with debugging binding errors due to database connections, etc, because they don't always throw an
    ///                             exception and things would just "not bind" with no apparent reason why
    /// </remarks>
    public static class DataBindingErrorHandler
    {
        /// <summary>
        /// Updates PresentationTraceSource's DataBindingSource to throw exceptions when DataBinding errors occur
        /// <para>Exceptions are not thrown when DataBinding errors occur; this allows exceptions to be thrown when objects that are bound to data throw exceptions</para>
        /// </summary>
        public static void ThrowExceptionsWithDataBindingErrors()
        {
            PresentationTraceSources.Refresh();
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;
            PresentationTraceSources.DataBindingSource.Listeners.Add(new BindingErrorTraceListener());
        }

        // Parses the incoming DataBinding error message to get the exception, source (call site), message, and
        // stack trace of the error-causing exception and then throws a DataBindingErrorException using those values
        private class BindingErrorTraceListener : TraceListener
        {
            public override void Write(string message) { }

            public override void WriteLine(string message)
            {
                string exception, exceptionType, exceptionMessage, exceptionStackTrace, exceptionSource;

                // define the keywords where the exception and details will be parsed from the DataBinding error message
                string exceptionStart = "Exception:'", exceptionEnd = ": ", exceptionSplit = "\r\n", callSiteStart = "at ", callSiteEnd = "(";

                // parse the full exception from the error message
                exception = message.Substring(message.IndexOf(exceptionStart) + exceptionStart.Length);

                // parse the exception type
                exceptionType = exception.Substring(0, exception.IndexOf(exceptionEnd));

                // parse the exception message
                exceptionMessage = exception.Substring(exceptionType.Length + exceptionEnd.Length, exception.IndexOf(exceptionSplit) - exceptionType.Length - exceptionEnd.Length);

                // parse the exception stack trace
                exceptionStackTrace = exception.Substring(exception.IndexOf(exceptionSplit) + exceptionSplit.Length);

                // parse the exception source (call site)
                exceptionSource = exceptionStackTrace.Substring(exceptionStackTrace.IndexOf(callSiteStart) + callSiteStart.Length);
                exceptionSource = exceptionSource.Substring(0, exceptionSource.IndexOf(callSiteEnd));

                throw new DataBindingErrorException(exceptionType, exceptionSource, exceptionMessage, exceptionStackTrace);
            }
        }
    }
}
