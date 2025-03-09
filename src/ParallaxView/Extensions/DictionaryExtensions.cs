namespace ParallaxView.Extensions;

public static class DictionaryExtensions
{
    public static TValue FindParameter<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Action<TValue> action = null)
        where TValue : new()
    {
        if (!dictionary.TryGetValue(key, out var param))
            dictionary.Add(key, param = new TValue());

        if (action != null)
            action(param);

        return param;
    }
}
