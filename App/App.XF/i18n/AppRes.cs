using System.Collections.Generic;
using App.XF.DI.DependencyService;
using I18NPortable;
using I18NPortable.CsvReader;
using ToyaMobileNative.XF.i18n;

namespace App.XF.i18n
{
    /// <inheritdoc />
    /// <summary>
    /// Facade for IN18NPortable lib
    /// </summary>
    public class AppRes : IAppRes
    {
        private readonly IDeviceStorageRepository _deviceStorageRepository;
        private readonly IDeviceLog _log;
        private const string ClassTag = "AppRes";
        private const string AppLangKey = "app_lang_key";

        public string LangCode => I18N.Current.Locale;
        public string this[string key] => I18N.Current[key];
        public bool IsLocalCultureSameAsDbCulture { get; set; }

        public AppRes(IDeviceStorageRepository deviceStorageRepository, IDeviceLog deviceLog)
        {
            _deviceStorageRepository = deviceStorageRepository;
            _log = deviceLog;

            I18N.Current.SetResourcesFolder("Resources.Locales")
                .SetFallbackLocale("en")
                .SetNotFoundSymbol("$")
                .SingleFileResourcesMode()
                .SetSingleFileLocaleReader(new CsvColSingleFileReader(), ".csv")
                .Init(GetType().Assembly);

            var savedLocale = ReadLocale();
            if (!savedLocale.Equals(LangCode))
            {
                I18N.Current.Locale = savedLocale;
            }
        }

        /// <summary>
        /// Sets new locale to I18NPortable
        /// </summary>
        /// <param name="locale">new locale code</param>
        /// <returns>false if locale already set</returns>
        public bool SetLang(string locale)
        {
            if (LangCode.Equals(locale)) return false;
            I18N.Current.Locale = locale;
            SaveLocale(locale);
            return true;
        }

        /// <summary>
        /// Save app language to AppLangKey property
        /// </summary>
        public void SaveLocale(string locale)
        {
            _deviceStorageRepository.SaveObj(AppLangKey, locale);
            _log.Info(ClassTag, "saved " + LangCode + " language");
        }

        /// <summary>
        /// Read app language from AppLangKey property
        /// If never set save default read by I18NPortable
        /// </summary>
        /// <returns>string locale code</returns>
        private string ReadLocale()
        {
            if (_deviceStorageRepository.ContainsKey(AppLangKey))
            {
                var locale = _deviceStorageRepository.GetObj<string>(AppLangKey);
                if (string.IsNullOrEmpty(locale))
                {
                    SaveLocale(LangCode);
                    return LangCode;
                }

                _log.Info(ClassTag, "read " + LangCode + " language");
                return locale;
            }

            SaveLocale(LangCode);
            return LangCode;
        }

        /// <summary>
        /// Creates translation list by enum keys
        /// </summary>
        /// <typeparam name="TEnum">Enum with keys</typeparam>
        /// <returns>IList&lt;string&gt; - Translation list</returns>
        public IList<string> TranslateEnumToList<TEnum>()
        {
            return I18N.Current.TranslateEnumToList<TEnum>();
        }
    }
}
