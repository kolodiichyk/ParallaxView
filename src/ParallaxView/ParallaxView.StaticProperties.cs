using ParallaxView.Extensions;

namespace ParallaxView;

public partial class  ParallaxView
{
    public static readonly BindableProperty SpeedProperty =
        BindableProperty.CreateAttached(nameof(ParallaxElementParam.Speed), typeof(double), typeof(ParallaxView), 1.0d,
            propertyChanged: OnSpeedPropertyChanged);

    public static readonly BindableProperty StickOnYProperty =
        BindableProperty.CreateAttached(nameof(ParallaxElementParam.StickOnY), typeof(double), typeof(ParallaxView), default(double),
            propertyChanged: StickOnYPropertyChanged);

    private static void OnSpeedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        OnStaticPropertyChanged(bindable, oldValue, newValue, nameof(ParallaxElementParam.Speed));
    }

    private static void StickOnYPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        OnStaticPropertyChanged(bindable, oldValue, newValue, nameof(ParallaxElementParam.StickOnY));
    }

    private static void OnStaticPropertyChanged(BindableObject bindable, object oldValue, object newValue, string propertyName)
    {
        if (bindable is not View view)
            return;

        if (view.FindParent<ParallaxView>() is ParallaxView parallaxView)
        {
            parallaxView._viewSpeedDictionary.FindParameter(view, p =>
            {
                SetParameterFromView(p, view);
                SetParameterFromValue(p, newValue, propertyName);
            });
        }
        else
        {
            view.ParentChanged += OnViewParentChanged;
        }
    }

    private static void SetParameterFromView(ParallaxElementParam param, View view)
    {
        param.Y = view.TranslationY;
    }

    private static void OnViewParentChanged(object sender, EventArgs e)
    {
        if (sender is not View senderView)
            return;

        if (senderView.FindParent<ParallaxView>() is ParallaxView newParallaxView)
        {
            newParallaxView._viewSpeedDictionary.FindParameter(senderView, p => {
                SetParameterFromView(p, senderView);

                SetParameterFromValue(p, GetSpeed((BindableObject)sender), nameof(ParallaxElementParam.Speed));
                SetParameterFromValue(p, GetStickOnY((BindableObject)sender), nameof(ParallaxElementParam.StickOnY));
            });

            // Manage scroll for children
            if (senderView is Layout layout && layout.Children.Any())
            {
                foreach (var child in layout.Children)
                {
                    OnViewParentChanged(child, e);
                }
            }

            senderView.ParentChanged -= OnViewParentChanged;
        }
    }

    private static void SetParameterFromValue(ParallaxElementParam param, object value, string propertyName)
    {
        switch (propertyName)
        {
            case nameof(ParallaxElementParam.Speed):
                param.Speed = Convert.ToDouble(value);
                break;
            case nameof(ParallaxElementParam.StickOnY):
                param.StickOnY = Convert.ToDouble(value);
                break;
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

    public static double GetStickOnY(BindableObject view)
    {
        return (double)view.GetValue(StickOnYProperty);
    }

    public static void SetStickOnY(BindableObject view, double value)
    {
        view.SetValue(StickOnYProperty, value);
    }
}
