using System.Collections.Generic;

namespace ToyaMobileNative.XF.i18n
{
    /// <summary>
    /// Provides app internationalization
    /// </summary>
    public interface IAppRes
    {
        string LangCode { get; }
        string this[string key] { get; }
        bool IsLocalCultureSameAsDbCulture { get; set; }

        bool SetLang(string locale);
        void SaveLocale(string locale);
        IList<string> TranslateEnumToList<TEnum>();
    }
}
