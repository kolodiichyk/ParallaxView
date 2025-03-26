using Android.Views;
using AndroidX.RecyclerView.Widget;
using Microsoft.Maui.Controls.Handlers.Items;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Platform;

namespace ParallaxView.Example
{
    public partial class CustomCollectionViewHandler : CollectionViewHandler
    {
        protected override RecyclerView CreatePlatformView()
        {
            var recyclerView = base.CreatePlatformView();

            recyclerView.SetClipToPadding(false);
            recyclerView.SetClipChildren(false);
            recyclerView.SetClipToOutline(false);

            return recyclerView;
        }

        protected override void ConnectHandler(RecyclerView platformView)
        {
            base.ConnectHandler(platformView);
        }

        protected override ReorderableItemsViewAdapter<ReorderableItemsView, IGroupableItemsViewSource> CreateAdapter()
        {
            var t =  base.CreateAdapter();
            return t;
        }
    }
}
