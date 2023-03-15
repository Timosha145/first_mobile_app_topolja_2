using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace first_mobile_app_topolja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StepperSlider_Page : ContentPage
    {
        private Stepper _stepper;
        private Slider _slider;
        private Label _label;

        private List<Object> _objects;

        public StepperSlider_Page()
        {
            _stepper = new Stepper
            {
                Minimum = 0,
                Margin = 100,
                Value = 20,
                Increment = 5
            };

            _label = new Label
            {
                Text = "TTHK",
                FontSize = _stepper.Value
            };

            _slider = new Slider
            {
                Minimum=_stepper.Minimum,
                Maximum=_stepper.Maximum,
                Value=_stepper.Value,
                MinimumTrackColor=Color.IndianRed,
                MaximumTrackColor=Color.CornflowerBlue
            };

            _stepper.ValueChanged += _stepper_ValueChanged;
            _slider.ValueChanged += _stepper_ValueChanged;

            _objects = new List<object>() { _stepper, _label, _slider };

            AbsoluteLayout abs = new AbsoluteLayout();
            double y = 0;

            foreach (var item in _objects)
            {
                y += 0.2f;
                AbsoluteLayout.SetLayoutBounds((BindableObject)item, new Rectangle(0.1, y, 300, 100));
                AbsoluteLayout.SetLayoutFlags((BindableObject)item, AbsoluteLayoutFlags.PositionProportional);
                abs.Children.Add((View)item);
            }

            Content = abs;
        }

        private void _stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            _label.FontSize = e.NewValue;
        }
    }
}