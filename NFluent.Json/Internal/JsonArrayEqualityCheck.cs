using System.Text.Json;

namespace NFluent.Json.Internal;

internal class JsonArrayEqualityCheck<T>
{
    private readonly JsonElement _element;

    private readonly T[] _other;

    public JsonArrayEqualityCheck(JsonElement element, T[] other)
    {
        if (element.ValueKind != JsonValueKind.Array)
        {
            throw new ArgumentException("The property value is not an array.");
        }

        _element = element;
        _other = other;
    }

    public bool Strict
    {
        get
        {
            if (_element.GetArrayLength() != _other.Length)
            {
                return false;
            }

            var enumerator = _element.EnumerateArray();
            for (var i = 0; enumerator.MoveNext(); i++)
            {
                var expectedItem = _other[i];
                var itemElement = enumerator.Current;
                var item = itemElement.Deserialize<T>();
                if (!Equals(item, expectedItem))
                {
                    return false;
                }
            }

            return true;
        }
    }

    public bool IgnoringOrder
    {
        get
        {
            if (_element.GetArrayLength() != _other.Length)
            {
                return false;
            }

            var actualEnumerator = _element.EnumerateArray();

            var actualItemsCount = new Dictionary<object, int>();
            var actualNullCount = 0;
            var expectedItemsCount = new Dictionary<object, int>();
            var expectedNullCount = 0;

            for (var i = 0; actualEnumerator.MoveNext(); i++)
            {
                var itemElement = actualEnumerator.Current;
                var item = itemElement.Deserialize<T>();
                if (item is null)
                {
                    actualNullCount++;
                }
                else
                {
                    actualItemsCount.TryGetValue(item, out var count);
                    count++;
                    actualItemsCount[item] = count;
                }

                var expectedItem = _other[i];
                if (expectedItem is null)
                {
                    expectedNullCount++;
                }
                else
                {
                    expectedItemsCount.TryGetValue(expectedItem, out var count);
                    count++;
                    expectedItemsCount[expectedItem] = count;
                }
            }

            if (actualNullCount != expectedNullCount)
            {
                return false;
            }

            if (actualItemsCount.Count != expectedItemsCount.Count)
            {
                return false;
            }

            foreach (var (item, count) in actualItemsCount)
            {
                if (!expectedItemsCount.TryGetValue(item, out var expectedCount) || count != expectedCount)
                {
                    return false;
                }
            }

            return true;
        }
    }
}