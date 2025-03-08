using Microsoft.Maui.Layouts;

namespace ParallaxView;

[ContentProperty(nameof(Children))]
public class ParallaxView : Layout, IDisposable
{
    private const double _defaultSpeed = 1.0;

    protected override ILayoutManager CreateLayoutManager() => new ParallaxViewLayoutManager(this);

    public static readonly BindableProperty SpeedProperty =
        BindableProperty.CreateAttached("Speed", typeof(double), typeof(ParallaxView), _defaultSpeed,
            propertyChanged: OnSpeedPropertyChanged);

    public static BindableProperty SourceProperty = BindableProperty.Create(nameof(Source),
        typeof(View), typeof(ParallaxView), propertyChanged: OnSourcePropertyChanged);

    private readonly Dictionary<VisualElement, double> _viewSpeedDictionary = new Dictionary<VisualElement, double>();

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
        ManageScrolled(e.VerticalOffset + e.VerticalDelta);
    }

    private void ScrollViewOnScrolled(object sender, ScrolledEventArgs e)
    {
        ManageScrolled(e.ScrollY);
    }

    private void ManageScrolled(double scrollY)
    {
        foreach (var child in Children.Cast<VisualElement>())
        {
            var speed = _defaultSpeed;
            if (_viewSpeedDictionary.TryGetValue(child, out var viewSpeed))
                speed = viewSpeed;

            child.TranslationY = scrollY * speed;
        }
    }

    public View Source
    {
        get => (View)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    private static void OnSpeedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is View view)
        {
            if (view.Parent is ParallaxView parallaxView)
            {
                parallaxView._viewSpeedDictionary.Add(view, (double)newValue);
            }
            else
            {
                void OnViewParentChanged(object sender, EventArgs e)
                {
                    if (view.Parent is ParallaxView newParallaxView)
                    {
                        newParallaxView._viewSpeedDictionary.Add(view, (double)newValue);
                        view.ParentChanged -= OnViewParentChanged;
                    }
                }

                view.ParentChanged += OnViewParentChanged;
            }
        }
    }


    public static double GetSpeed(BindableObject view)
    {
        return (double)view.GetValue(SpeedProperty);
    }

    public static void SetSpeed(BindableObject view, double value)
    {
        view.SetValue(SpeedProperty, value);
    }

    public void Dispose()
    {
        UnSubscribeToSource(Source);
    }
}
