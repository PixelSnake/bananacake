using System.Linq;
using System.Collections;
using System.Collections.Generic;

public static class EnumerableExtensions {
    public static int IndexOf<T>(this IEnumerable<T> list, T value) {
        return list
            .Select((item, index) => new { item, index })
            .TakeWhile(pair => !pair.item.Equals(value))
            .LastOrDefault()?.index + 1 ?? -1;
    }

    public static IEnumerable<T> TakeRange<T>(this IEnumerable<T> list, int start, int end) {
        return list
            .Skip(start)
            .Take(end - start);
    }

    public static IEnumerable<T> SkipEnd<T>(this IEnumerable<T> list, int num) {
        return list.Take(list.Count() - num);
    }

    public static IEnumerable<string> SelectValues(this IEnumerable<BCake.Parser.Token> list) {
        return list.Select(t => t.Value);
    }

    public static IEnumerable<T> InList<T>(this T item) {
        yield return item;
    }

    public static T[] InArray<T>(this T item) {
        return new T[] { item };
    }

    public static string JoinString(this IEnumerable<string> list, string separator) {
        return string.Join(separator, list);
    }

    public static T FirstOr<T>(this IEnumerable<T> list, T orValue)
    {
        var res = list.ToArray();
        if (res.Length == 0) return orValue;
        return res[0];
    }
}
