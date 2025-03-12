namespace ParallaxView;

internal record ParallaxElementParam
{
    public double Speed { get; set; }
    public double Y { get; set; }
    public double StickOnY { get; set; }
    public bool IsZoomed { get; set; }
    public double ZoomScale { get; set; }
}
