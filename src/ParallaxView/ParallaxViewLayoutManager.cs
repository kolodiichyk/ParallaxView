using Microsoft.Maui.Layouts;

namespace ParallaxView;

public class ParallaxViewLayoutManager(IList<IView> view) : ILayoutManager
{
    public Size Measure(double widthConstraint, double heightConstraint)
    {
        return ProcessChildren(view, child => child.Measure(widthConstraint, heightConstraint));
    }

    public Size ArrangeChildren(Rect bounds)
    {
        return ProcessChildren(view, child => child.Arrange(bounds));
    }

	Size ProcessChildren(IEnumerable<IView> children, Func<IView, Size> processFunc)
    {
        double width = 0;
        double height = 0;

        foreach (var child in children.Reverse())
        {
            if (child.Visibility == Visibility.Collapsed)
                continue;

            var childSize = processFunc(child);
            width = Math.Max(width, childSize.Width);
            height = Math.Max(height, childSize.Height);
        }

        return new Size(width, height);
    }
}