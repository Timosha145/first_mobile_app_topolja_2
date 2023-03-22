using first_mobile_app_topolja;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Button = Xamarin.Forms.Button;
using Slider = Xamarin.Forms.Slider;

namespace first_mobile_app_topolja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RGBPage : ContentPage
    {
        private RGBSlider _rSlider = new RGBSlider("Red");
        private RGBSlider _gSlider = new RGBSlider("Green");
        private RGBSlider _bSlider = new RGBSlider("Blue");

        private List<Object> _objects = new List<Object>();
        private List<RGBSlider> _rgbSliders;
        private Button _button;

        public static Stepper Stepper;
        public static BoxView ColorBox;
        public static Label ColorBoxValueLabel;

        public RGBPage()
        {
            AbsoluteLayout abs = new AbsoluteLayout();

            _rgbSliders = new List<RGBSlider>() { _rSlider, _gSlider, _bSlider };

            ColorBox = new BoxView
            {
                WidthRequest = 200,
                HeightRequest = 200,
                Color = new Color(0, 0, 0),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
            };

            ColorBoxValueLabel = new Label
            {
                Text = $"R: {ColorBox.Color.R}, G: {ColorBox.Color.G}, B: {ColorBox.Color.B}, Opacity: 100%"
            };

            Stepper = new Stepper
            {
                Minimum = 0,
                Maximum = 255,
                Increment = 15,
                Value = 255
            };

            _button = new Button()
            {
                Text = "Random Color",
                BackgroundColor = Color.DarkGray,
                TextColor = Color.WhiteSmoke,
            };

            _button.Clicked += _button_Clicked;
            Stepper.ValueChanged += Stepper_ValueChanged;

            _objects.Add(ColorBox);
            _objects.Add(ColorBoxValueLabel);
            _objects.Add(Stepper);
            _objects.Add(_button);

            PlaceObjects();

            void PlaceSliders(double xOffset, double yOffset, ref double yPos)
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

        private void _button_Clicked(object sender, EventArgs e)
        {
            Color color = ColorBox.Color;
            Random random = new Random();

            int currentR = random.Next(0, 255);
            int currentG = random.Next(0, 255);
            int currentB = random.Next(0, 255);

            _rSlider.ColorSlider.Value = currentR;
            _gSlider.ColorSlider.Value = currentG;
            _bSlider.ColorSlider.Value = currentB;


            ColorBox.Color = Color.FromRgba(currentR, currentG, currentB, (int)Stepper.Value);

            ColorBoxValueLabel.Text = $"R: {String.Format("{0:X2}", (int)(color.R * 255))}, G: {String.Format("{0:X2}", (int)(color.G * 255))}" +
                 $", B: {String.Format("{0:X2}", (int)(color.B * 255))}, Opacity: {Math.Round(Stepper.Value / 255 * 100, 0)}%";
        }

        private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Stepper stepper = (Stepper)sender;
            Color color = ColorBox.Color;
            double colorValue = stepper.Value;

            int currentR = (int)(color.R * 255);
            int currentG = (int)(color.G * 255);
            int currentB = (int)(color.B * 255);

            ColorBox.Color = Color.FromRgba(currentR, currentG, currentB, (int)colorValue);

            ColorBoxValueLabel.Text = $"R: {String.Format("{0:X2}", (int)(color.R * 255))}, G: {String.Format("{0:X2}", (int)(color.G * 255))}" +
                 $", B: {String.Format("{0:X2}", (int)(color.B * 255))}, Opacity: {Math.Round(colorValue / 255 * 100, 0)}%";
        }
    }
}

public class RGBSlider
{
    public RGBSlider(string title)
    {
        Title = title;

        Label.Text = $"{Title}: {(int)ColorSlider.Value}";

        ColorSlider.ValueChanged += ColorSlider_ValueChanged;
    }

    public string Title { get; private set; }

    public Slider ColorSlider = new Slider
    {
        Maximum = 255,
        Minimum = 0,
        MinimumTrackColor = Color.CornflowerBlue,
        MaximumTrackColor = Color.DarkBlue,
        ThumbColor = Color.DarkCyan,
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

        int currentR = (int)(color.R * 255);
        int currentG = (int)(color.G * 255);
        int currentB = (int)(color.B * 255);

        if (Title == "Red")
            RGBPage.ColorBox.Color = Color.FromRgb(colorValue, currentG, currentB);
        else if (Title == "Green")
            RGBPage.ColorBox.Color = Color.FromRgb(currentR, colorValue, currentB);
        else if (Title == "Blue")
            RGBPage.ColorBox.Color = Color.FromRgb(currentR, currentG, colorValue);

        RGBPage.ColorBoxValueLabel.Text = $"R: {String.Format("{0:X2}", (int)(color.R * 255))}, G: {String.Format("{0:X2}", (int)(color.G * 255))}" +
            $", B: {String.Format("{0:X2}", (int)(color.B * 255))}, Opacity: {Math.Round(RGBPage.Stepper.Value / 255 * 100, 0)}%";
    }
}
