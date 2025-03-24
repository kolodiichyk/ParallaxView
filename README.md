# ParallaxView [![NuGet](https://img.shields.io/nuget/v/ParallaxView.svg?label=NuGet)](https://www.nuget.org/packages/ParallaxView/)

.net MAUI control for creating parallax effects to parent controls with scroll (ScrollView and CollectionView)
## FireWatch Parallax Example

The FireWatch parallax implementation creates a beautiful depth effect using multiple image layers with different parallax speeds. Here's how to implement it:

```xaml
<parallaxView:ParallaxView 
    xmlns:parallaxView="clr-namespace:ParallaxView;assembly=ParallaxView"
    Source="{Binding Source={x:Reference ParentScrollView}}"
    BackgroundColor="#F8DDCB"
    HeightRequest="300"
    IsClippedToBounds="True">
    
    <!-- Add multiple image layers with decreasing parallax speeds -->
    <Image 
        parallaxView:ParallaxView.Speed="0.8"
        Source="layer_1.png"
        Aspect="AspectFill"
        HeightRequest="300" />
    
    <!-- Additional layers... -->
    
    <!-- Center logo/text with minimal parallax -->
    <Image 
        parallaxView:ParallaxView.Speed="0.3"
        Source="firewatch.png"
        HeightRequest="250"
        HorizontalOptions="Center" />
        
    <!-- Static front layer -->
    <Image 
        parallaxView:ParallaxView.Speed="0"
        Source="layer_6.png"
        HeightRequest="300" />
</parallaxView:ParallaxView>
```

### Key Features:
- Multiple image layers with different parallax speeds (where 1 - normal scroll speed)
- Higher speed values (0.8) create stronger parallax effects
- Speed 0 for static front layer
- All images use AspectFill to maintain proper scaling
- Container clips to bounds for clean edges
- Source is ScrollView or CollectionView

### Required Resources:
- Background layers (layer_1.png through layer_6.png)
- Center logo/text image (firewatch.png)
- Set appropriate background color (#F8DDCB)

