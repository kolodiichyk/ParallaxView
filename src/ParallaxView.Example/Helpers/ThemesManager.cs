#if __ANDROID__
using Android.OS;
using Microsoft.Maui.Platform;
#endif
using ParallaxView.Example.Resources.Styles;


namespace ParallaxView.Example.Helpers;

static class ThemesManager
{
	static readonly Dictionary<Themes, ResourceDictionary> themes;

    static ThemesManager()
    {
        themes = new Dictionary<Themes, ResourceDictionary>
        {
            {Themes.FireWatch, new FireWatchTheme()},
            {Themes.PhotoZoom, new PhotoZoomTheme()},
            {Themes.FireWatch2, new FireWatchTheme2()},
        };
    }

    public static void SetTheme(Themes theme)
    {
        if (Application.Current == null)
		{
			return;
		}

		Application.Current.Resources.MergedDictionaries.Clear();
        Application.Current.Resources.MergedDictionaries.Add(themes[theme]);

        FixFlyoutBackgroundColor();
        FixStatusBarColor();
    }

    static void FixFlyoutBackgroundColor()
    {
        if (Application.Current != null &&
            Application.Current.Resources.TryGetValue("Primary", out var primaryColor))
        {
            Shell.Current.FlyoutBackgroundColor =  (Color)primaryColor;
        }
    }

    static void FixStatusBarColor()
    {
#if __ANDROID__
        var window = Platform.CurrentActivity?.Window;
        if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
        {
            if (Application.Current != null &&
                Application.Current.Resources.TryGetValue("Secondary", out var secondaryColor) &&
                window != null)
            {
                window.SetStatusBarColor(((Color)secondaryColor).ToPlatform());
            }
        }
#endif
    }
}

enum Themes
{
    FireWatch,
    PhotoZoom,
    FireWatch2
}