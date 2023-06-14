using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JGP.Telegram.Data.Converters;

/// <summary>
///     Class long list to delimited string converter
/// </summary>
/// <seealso cref="ValueConverter{List{long}, string}" />
public class LongListToDelimitedStringConverter : ValueConverter<List<long>, string>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="LongListToDelimitedStringConverter" /> class
    /// </summary>
    public LongListToDelimitedStringConverter()
        : base(
            list => string.Join('|', list.OrderBy(i => i)),
            str => str.Split("|", StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .OrderBy(i => i)
                .ToList())
    {
    }
}