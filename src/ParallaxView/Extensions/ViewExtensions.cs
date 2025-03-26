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

    public static object FindParent<T1, T2>(this Element element)
    {
        var parent = element.Parent;

        if (parent == null)
            return null;

        if (parent is T1 || parent is T2)
            return parent;

        return parent.FindParent<T1, T2>();
    }
}
