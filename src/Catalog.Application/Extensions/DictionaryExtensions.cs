namespace Catalog.Application.Extensions
{
    public static class DictionaryExtensions
    {
        public static T GetValueOrDefault<T>(this Dictionary<string, T> dictionary, string key, T defaultValue = default)
        {
            if (dictionary == null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }
            return dictionary.TryGetValue(key, out T value) ? value : defaultValue;
        }
    }
}
