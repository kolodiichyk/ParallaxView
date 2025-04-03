using Microsoft.Maui.Layouts;
using ParallaxView.Extensions;

namespace ParallaxView;

[ContentProperty(nameof(Children))]
public partial class ParallaxView : Layout, IDisposable
{
	bool isDisposed;

	public static BindableProperty SourceProperty = BindableProperty.Create(nameof(Source),
		typeof(View), typeof(ParallaxView), propertyChanged: OnSourcePropertyChanged);

	readonly Dictionary<VisualElement, ParallaxElementParam> _viewSpeedDictionary = new();

	public View Source
	{
		get => (View)GetValue(SourceProperty);
		set => SetValue(SourceProperty, value);
	}

	public void Dispose()
	{
		Dispose(true);
	}

	protected virtual void Dispose(bool isDisposing)
	{
		if (isDisposed)
		{
			return;
		}

		if (isDisposing)
		{
			UnSubscribeToSource(Source);
		}

		isDisposed = true;
	}

	protected override ILayoutManager CreateLayoutManager()
	{
		return new ParallaxViewLayoutManager(this);
	}

	static void OnSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		if (bindable is ParallaxView parallaxView)
		{
			parallaxView.UnSubscribeToSource(oldValue);
			parallaxView.SubscribeToSource(newValue);
		}
	}

	protected override void OnParentChanged()
	{
		base.OnParentChanged();

		if (Parent is null)
		{
			return;
		}

		if (Parent is ScrollView or CollectionView)
		{
			UnSubscribeToSource(Source);
			SubscribeToSource(Parent);
		}
		else
		{
			var source = Parent.FindParent<ScrollView, CollectionView>();
			if (source is not null)
			{
				UnSubscribeToSource(Source);
				SubscribeToSource(source);
			}
			else
			{
				Parent.ParentChanged += OnParentParentChanged;
			}
		}
	}

	void OnParentParentChanged(object sender, EventArgs e)
	{
		Parent.ParentChanged -= OnParentParentChanged;
		OnParentChanged();
	}

	void SubscribeToSource(object source)
	{
		ManageScrollEventSubscription(source, true);
	}

	void UnSubscribeToSource(object source)
	{
		ManageScrollEventSubscription(source, false);
	}

	void ManageScrollEventSubscription(object source, bool subscribe)
	{
		if (source == null)
		{
			return;
		}

		switch (source)
		{
			case ScrollView scrollView:
				if (subscribe)
				{
					scrollView.Scrolled += ScrollViewOnScrolled;
				}
				else
				{
					scrollView.Scrolled -= ScrollViewOnScrolled;
				}

				break;
			case CollectionView collectionView:
				if (subscribe)
				{
					collectionView.Scrolled += CollectionViewOnScrolled;
				}
				else
				{
					collectionView.Scrolled -= CollectionViewOnScrolled;
				}

				break;
		}
	}

	void CollectionViewOnScrolled(object sender, ItemsViewScrolledEventArgs e)
	{
		ManageScrolled(e.VerticalOffset /*+ e.VerticalDelta*/, this);
	}

	void ScrollViewOnScrolled(object sender, ScrolledEventArgs e)
	{
		ManageScrolled(e.ScrollY, this);
	}

	void ManageScrolled(double scrollY, Layout view)
	{
		double parentTranslationY = 0;
		if (view.Parent is not ParallaxView)
		{
			parentTranslationY = view.TranslationY;
		}

		foreach (var child in view.Children.Cast<VisualElement>())
		{
			child.BatchBegin();
			if (!_viewSpeedDictionary.TryGetValue(child, out var param))
			{
				return;
			}

			child.TranslationY = parentTranslationY + param.Y + scrollY * param.Speed;

			ManageZoom(child, param, scrollY);

			// Manage scroll for children
			if (child is Layout layout && layout.Children.Any())
			{
				ManageScrolled(scrollY, layout);
			}

			child.BatchCommit();
		}
	}

	void ManageZoom(VisualElement view, ParallaxElementParam param, double scrollY)
	{
		if (!param.IsZoomed)
		{
			return;
		}

		view.Scale = 1 + Math.Abs(param.ZoomScale * scrollY);
	}
}