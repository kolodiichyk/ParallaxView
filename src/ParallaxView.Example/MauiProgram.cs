using Microsoft.Extensions.Logging;

namespace ParallaxView.Example;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Font Awesome 6 Free-Regular-400.otf", "FontAwesomeRegular");
                fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "FontAwesomeSolid");
                fonts.AddFont("Schnyder-S-Demi.otf", "SchnyderS");
                fonts.AddFont("Schnyder-S-Light-Italic.otf", "SchnyderSItalic");
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("FuturaPT-Bold.ttf", "FuturaPTBold");
            })
            .ConfigureMauiHandlers(handlers =>
            {
#if __ANDROID__
                handlers.AddHandler<CollectionView, CustomCollectionViewHandler>();
#endif
            });

        return builder.Build();
    }
}
