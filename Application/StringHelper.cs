namespace FS.Utilities.Application
{
    public static class StringHelper
    {
        public static string ckey(this string s)
        {
            return s.ToLower().Replace("_", string.Empty);
        }
    }
}
