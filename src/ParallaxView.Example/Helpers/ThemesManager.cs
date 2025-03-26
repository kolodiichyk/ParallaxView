#if __ANDROID__
using Android.OS;
#endif
using Microsoft.Maui.Platform;
using ParallaxView.Example.Resources.Styles;


namespace ParallaxView.Example.Helpers;

internal static class ThemesManager
{
    private static Dictionary<Themes, ResourceDictionary> _themes;

    static ThemesManager()
    {
        _themes = new Dictionary<Themes, ResourceDictionary>
        {
            {Themes.FireWatch, new FireWatchTheme()},
            {Themes.PhotoZoom, new PhotoZoomTheme()},
            {Themes.FireWatch2, new FireWatchTheme2()},
        };
    }

    public static void SetTheme(Themes theme)
    {
        if (Application.Current == null)
            return;

        Application.Current.Resources.MergedDictionaries.Clear();
        Application.Current.Resources.MergedDictionaries.Add(_themes[theme]);

        FixFlyoutBackgroundColor();
        FixStatusBarColor();
    }

    private static void FixFlyoutBackgroundColor()
    {
        if (Application.Current != null &&
            Application.Current.Resources.TryGetValue("Primary", out var primaryColor))
        {
            Shell.Current.FlyoutBackgroundColor =  (Color)primaryColor;
        }
    }

    public static void FixStatusBarColor()
    {
#if __ANDROID__
        if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
        {
            if (Application.Current != null &&
                Application.Current.Resources.TryGetValue("Secondary", out var secondaryColor))
            {
                Platform.CurrentActivity.Window.SetStatusBarColor(((Color)secondaryColor).ToPlatform());
            }
        }
#endif
    }
}

internal enum Themes
{
    FireWatch,
    PhotoZoom,
    FireWatch2
}
