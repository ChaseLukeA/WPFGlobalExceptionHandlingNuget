using System;

namespace WPFGlobalExceptionHandling
{
    /// <summary>
    /// DataBindingErrorException is thrown when a DataBinding error occurs when using the DataBindingErrorExceptionHandler class
    /// <para>This exception contains the DataBinding error-causing exception, its type, its source, and its stack trace</para>
    /// </summary>
    public class DataBindingErrorException : Exception
    {
        private readonly string type;
        private readonly string stackTrace;

        public DataBindingErrorException(string type, string source, string message, string stackTrace) : base(message)
        {
            this.type = type;
            base.Source = source;
            this.stackTrace = stackTrace;
        }

        /// <summary>
        /// Overrides base Message to include the exception that caused DataBindingErrorException to be thrown
        /// <para>Example: The username provided does not exist! (thrown by System.NullReferenceException)</para>
        /// </summary>
        public override string Message => $"{base.Message} ({type})";

        /// <summary>
        /// Overrides the base StacKTrace to be the stack trace of the exception that caused DataBindingErrorException to be thrown
        /// </summary>
        public override string StackTrace => this.stackTrace;

        /// <summary>
        /// Prints the same details as the base Exception's ToString() except it adds the private 'type' after this 'DataBindingErrorException'
        /// <para>Example: DataBindingErrorException: The username provided does not exist! (thrown by System.NullReferenceException)[new line][stack trace]</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{this.GetType().Name}: {Message}";
        }
    }
}
