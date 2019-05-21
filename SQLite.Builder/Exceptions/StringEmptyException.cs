using System;

namespace PureFreak.SQLite.Builder.Exceptions
{
    public class StringEmptyException : Exception
    {
        public StringEmptyException(string parameterName)
            : base($"{parameterName} cannot be null or empty.")
        {
        }
    }
}
