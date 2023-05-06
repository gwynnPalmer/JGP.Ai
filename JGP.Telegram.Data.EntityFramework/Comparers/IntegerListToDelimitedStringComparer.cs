using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace JGP.Telegram.Data.Comparers;

/// <summary>
///     Class lonng list value comparer
/// </summary>
/// <seealso cref="ValueComparer{List}" />
public class LongListValueComparer : ValueComparer<List<long>>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="LongListValueComparer" /> class
    /// </summary>
    public LongListValueComparer()
        : base((list1, list2) => AreLongListsEqual(list1, list2),
            list => GetIntegerListHashCode(list),
            list => list == null ? null : new List<long>(list))
    {
    }

    /// <summary>
    ///     Describes whether the long lists are equal
    /// </summary>
    /// <param name="list1">The list</param>
    /// <param name="list2">The list</param>
    /// <returns>Interop+BOOL</returns>
    private static bool AreLongListsEqual(List<long>? list1, List<long>? list2)
    {
        if (list1 is null && list2 is null) return true;
        if (list1 is null && list2 is not null) return false;
        if (list1 is not null && list2 is null) return false;
        if (list1.Count != list2.Count) return false;

        return list1
            .OrderBy(x => x)
            .SequenceEqual(list2.OrderBy(x => x));
    }

    /// <summary>
    ///     Gets the integer list hash code using the specified list
    /// </summary>
    /// <param name="list">The list</param>
    /// <returns>int</returns>
    private static int GetIntegerListHashCode(IEnumerable<long> list)
    {
        return list
            .OrderBy(x => x)
            .Aggregate(0, (current, item) => (current * 397) ^ item.GetHashCode());
    }
}