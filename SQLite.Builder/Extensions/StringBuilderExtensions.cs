using System;
using System.Text;

namespace PureFreak.SQLite.Builder.Extensions
{
    internal static class StringBuilderExtensions
    {

        public static StringBuilder TrimEnd(this StringBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            while (builder[builder.Length - 1] == ' ' || builder[builder.Length - 1] == '\t')
                builder.Length--;

            return builder;
        }

        public static StringBuilder TrimEnd(this StringBuilder builder, params char[] chars)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (chars == null)
                throw new ArgumentNullException(nameof(chars));

            while (true)
            {
                var ch = builder[builder.Length - 1];
                if (Array.IndexOf(chars, ch) != -1)
                {
                    builder.Length--;
                    continue;
                }

                break;
            }

            return builder;
        }

    }
}
