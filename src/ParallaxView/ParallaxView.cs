using Microsoft.Maui.Layouts;
using ParallaxView.Extensions;

namespace ParallaxView;

[ContentProperty(nameof(Children))]
public partial class ParallaxView : Layout, IDisposable
{
    protected override ILayoutManager CreateLayoutManager() => new ParallaxViewLayoutManager(this);


    public static BindableProperty SourceProperty = BindableProperty.Create(nameof(Source),
        typeof(View), typeof(ParallaxView), propertyChanged: OnSourcePropertyChanged);

    private readonly Dictionary<VisualElement, ParallaxElementParam> _viewSpeedDictionary = new Dictionary<VisualElement, ParallaxElementParam>();

    private static void OnSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is ParallaxView parallaxView)
        {
            parallaxView.UnSubscribeToSource(oldValue);
            parallaxView.SubscribeToSource(newValue);
        }
    }

    private void SubscribeToSource(object source)
    {
        ManageScrollEventSubscription(source, true);
    }

    private void UnSubscribeToSource(object source)
    {
        ManageScrollEventSubscription(source, false);
    }

    private void ManageScrollEventSubscription(object source, bool subscribe)
    {
        if (source == null)
            return;

        switch (source)
        {
            case ScrollView scrollView:
                if (subscribe)
                    scrollView.Scrolled += ScrollViewOnScrolled;
                else
                    scrollView.Scrolled -= ScrollViewOnScrolled;
                break;
            case CollectionView collectionView:
                if (subscribe)
                    collectionView.Scrolled += CollectionViewOnScrolled;
                else
                    collectionView.Scrolled -= CollectionViewOnScrolled;
                break;
        }
    }

    private void CollectionViewOnScrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        ManageScrolled(e.VerticalOffset + e.VerticalDelta, this);
    }

    private void ScrollViewOnScrolled(object sender, ScrolledEventArgs e)
    {
        ManageScrolled(e.ScrollY, this);
    }

    private void ManageScrolled(double scrollY, Layout view)
    {
        double parentTranslationY = 0;
        if (view.Parent is not ParallaxView)
            parentTranslationY = view.TranslationY;

        foreach (var child in view.Children.Cast<VisualElement>())
        {
            if (!_viewSpeedDictionary.TryGetValue(child, out var param))
                return;

            // On stick, we need to add the difference between the scroll and the stick position
            if (param.StickOnY > 0 && param.StickOnY <= scrollY)
            {
                child.TranslationY = parentTranslationY + param.Y + scrollY
                                     - param.StickOnY * (1 - param.Speed);
            }
            else
            {
                child.TranslationY = parentTranslationY + param.Y + scrollY * param.Speed;
            }

            // Manage scroll for children
            if (child is Layout layout && layout.Children.Any())
                ManageScrolled(scrollY, layout);
        }
    }

    public View Source
    {
        get => (View)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public void Dispose()
    {
        UnSubscribeToSource(Source);
    }
}
