using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace PathFinder.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static string GeneralSettings
        {
            get => AppSettings.GetValueOrDefault(nameof(GeneralSettings), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(GeneralSettings), value); 
        }


        public static string CategorySettings
        {
            get => AppSettings.GetValueOrDefault(nameof(CategorySettings), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(CategorySettings), value);
        }


        public static string SubCategorySettings
        {
            get => AppSettings.GetValueOrDefault(nameof(SubCategorySettings), string.Empty);

            set => AppSettings.AddOrUpdateValue(nameof(SubCategorySettings), value);
        }
    }
}
