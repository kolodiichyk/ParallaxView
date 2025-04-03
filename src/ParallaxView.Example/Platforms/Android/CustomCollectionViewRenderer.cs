using AndroidX.RecyclerView.Widget;
using Microsoft.Maui.Controls.Handlers.Items;
using Microsoft.Maui.Platform;

namespace ParallaxView.Example;

public class CustomCollectionViewHandler : CollectionViewHandler
{
	protected override RecyclerView CreatePlatformView()
	{
		var recyclerView = base.CreatePlatformView();

		recyclerView.SetClipToPadding(false);
		recyclerView.SetClipChildren(false);
		recyclerView.SetClipToOutline(false);

		return recyclerView;
	}

	protected override ReorderableItemsViewAdapter<ReorderableItemsView, IGroupableItemsViewSource> CreateAdapter()
	{
		var t =  base.CreateAdapter();
		return t;
	}
}