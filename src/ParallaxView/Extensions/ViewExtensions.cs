namespace ParallaxView.Extensions;

public static class ViewExtensions
{
    public static T FindParent<T>(this Element element) where T : VisualElement
    {
        var parent = element.Parent;

        if (parent == null)
            return null;

        if (parent is T parentAsT)
            return parentAsT;

        return parent.FindParent<T>();
    }
}
