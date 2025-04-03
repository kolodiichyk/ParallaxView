using ParallaxView.Example.Helpers;

namespace ParallaxView.Example;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
    }

    protected override void OnNavigated(ShellNavigatedEventArgs args)
    {
        if (args.Current.Location.OriginalString.Contains(FireWatchPage2.Route))
		{
			ThemesManager.SetTheme(Themes.FireWatch2);
		}
		else if (args.Current.Location.OriginalString.Contains(FireWatchPage.Route))
		{
			ThemesManager.SetTheme(Themes.FireWatch);
		}
		else if (args.Current.Location.OriginalString.Contains(PhotoZoom.Route))
		{
			ThemesManager.SetTheme(Themes.PhotoZoom);
		}

		base.OnNavigated(args);
    }
}