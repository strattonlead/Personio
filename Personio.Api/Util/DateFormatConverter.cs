using Newtonsoft.Json.Converters;
using Personio.Api.Common;

namespace Personio.Api.Util
{
    public class DateFormatConverter : IsoDateTimeConverter
    {
        public DateFormatConverter() : this(Constants.DATE_FORMAT) { }
        public DateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}
