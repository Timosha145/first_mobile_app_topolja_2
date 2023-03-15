using first_mobile_app_topolja;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace first_mobile_app_topolja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RGBPage : ContentPage
    {
        private List<Object> _objects= new List<Object>();
        private List<RGBSlider> _rgbSliders = new List<RGBSlider>() { new RGBSlider("Red"), new RGBSlider("Green"), new RGBSlider("Blue") };
        public static BoxView ColorBox;
        public static Label ColorBoxValueLabel;

        public RGBPage()
        {
            AbsoluteLayout abs = new AbsoluteLayout();

            ColorBox = new BoxView
            {
                WidthRequest= 200,
                HeightRequest= 200,
                Color= new Color(0,0,0),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions= LayoutOptions.Center,
            };

            ColorBoxValueLabel = new Label
            {
                Text = $"R: {ColorBox.Color.R}, G: {ColorBox.Color.G}, B: {ColorBox.Color.B},"
            };

            _objects.Add(ColorBox);
            _objects.Add(ColorBoxValueLabel);

            PlaceObjects();

            void PlaceSliders(double xOffset, double yOffset , ref double yPos)
            {
                foreach (var item in _rgbSliders)
                {
                    double x = 0;

                    AbsoluteLayout.SetLayoutBounds(item.Label, new Rectangle(x, yPos, 300, 50));
                    abs.Children.Add(item.Label);

                    x = item.Label.Width + xOffset;

                    AbsoluteLayout.SetLayoutBounds(item.ColorSlider, new Rectangle(x, yPos, 300, 50));
                    abs.Children.Add(item.ColorSlider);

                    yPos += item.ColorSlider.Height + yOffset;
                }
            }

            void PlaceObjects()
            {
                double y = 0, xOffset = 50, yOffset = 100;

                foreach (var obj in _objects)
                {
                    int Width = 300, Height = 50;

                    if (obj is BoxView)
                    {
                        Width = 300;
                        Height = 300;
                    }

                    Rectangle rectangle = new Rectangle(0, y, Width, Height);

                    AbsoluteLayout.SetLayoutBounds((BindableObject)obj, rectangle);
                    y += rectangle.Height;
                    abs.Children.Add((View)obj);
                }

                PlaceSliders(xOffset, yOffset, ref y);

                Content = abs;
            }
        }
    }
}

public class RGBSlider
{
    public RGBSlider(string title)
    {
        Title= title;

        Label.Text = $"{Title}: {(int)ColorSlider.Value}";

        ColorSlider.ValueChanged += ColorSlider_ValueChanged;
    }

    public string Title { get; private set; }

    public Slider ColorSlider = new Slider
    {
        Maximum = 255,
        Minimum = 0,
        MinimumTrackColor= Color.CornflowerBlue,
        MaximumTrackColor = Color.DarkBlue,
        ThumbColor= Color.DarkCyan
    };

    public Label Label = new Label
    {
        Text = "Color"
    };
    private void ColorSlider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        Slider slider = (Slider)sender;
        Color color = RGBPage.ColorBox.Color;
        int colorValue = (int)slider.Value;

        Label.Text = $"{Title}: {colorValue}";

        double currentR = color.R;
        double currentG = color.G;
        double currentB = color.B;

        if (Title=="Red")
            RGBPage.ColorBox.Color = Color.FromRgba(colorValue, currentG, currentB, 1);
        else if (Title == "Green")
            RGBPage.ColorBox.Color = Color.FromRgba(currentR, colorValue, currentB, 1);
        else if(Title == "Blue")
            RGBPage.ColorBox.Color = Color.FromRgba(currentR, currentG, colorValue, 1);

        RGBPage.ColorBoxValueLabel.Text = $"R: {color.R}, G: {color.G}, B: {color.B},";
    }
}