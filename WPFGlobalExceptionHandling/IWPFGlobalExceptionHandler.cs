using System;

namespace WPFGlobalExceptionHandling
{
    public interface IWPFGlobalExceptionHandler
    {
        void HandleException(Exception e);

        void HandleUnrecoverableException(Exception e);
    }
}
