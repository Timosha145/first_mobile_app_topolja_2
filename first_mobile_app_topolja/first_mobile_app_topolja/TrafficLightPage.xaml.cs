using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace first_mobile_app_topolja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrafficLightPage : ContentPage
    {
        private BoxView _circle1, _circle2, _circle3;
        private Label _label1, _label2, _label3;
        private BoxView[] _circles;
        private Label[] _labels;
        private string[] _trafficLightNames = new string[] { "Punane" , "Kollane" , "Roheline" };
        private int _timer = 2;
        private bool _isTrafficLightWorking, _isNightModeWorking;
        private sbyte _stopTimer;

        public TrafficLightPage()
        {
            _circles = new BoxView[] {_circle1, _circle2, _circle3};
            _labels = new Label[] { _label1, _label2, _label3 };

            StackLayout stackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.White,
            };

            StackLayout trafficLightSL = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.Black,
                Padding = 10,
                Margin = new Thickness(40, 0, 40, 0),              
            };


            Label header = new Label()
            {
                TextColor= Color.White,
                BackgroundColor= Color.DarkSlateGray,
                WidthRequest = Application.Current.MainPage.Width,
                Text = "Valgusfoor",
                FontSize = 30,
                FontAttributes= FontAttributes.Bold,
            };

            Editor editor = new Editor()
            {
                Placeholder = "Muuda Kiirus",
                BackgroundColor = Color.LightGray,
                TextColor = Color.White,
                PlaceholderColor = Color.Gray,               
            };

            Button turnOnBtn = new Button()
            {
                Text = "Lülita Sisse",
                BackgroundColor = Color.DarkGray,
                TextColor = Color.WhiteSmoke
            };

            Button nightModeBtn = new Button()
            {
                Text = "Lülita Sisse Öörežiim",
                BackgroundColor = Color.DarkGray,
                TextColor = Color.WhiteSmoke
            };

            for (int i = 0; i < _circles.Length; i++)
            {
                _circles[i] = new BoxView()
                {
                    Color = Color.Gray,
                    CornerRadius = 360,
                    WidthRequest = 125,
                    HeightRequest = 125,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                };


                _labels[i] = new Label()
                {
                    TextColor = Color.White,
                    HorizontalOptions= LayoutOptions.Center,
                    Text = _trafficLightNames[i],
                    FontSize = 15,
                    FontAttributes = FontAttributes.Bold,
                };

                trafficLightSL.Children.Add(_circles[i]);
                trafficLightSL.Children.Add(_labels[i]);

                TapGestureRecognizer tap = new TapGestureRecognizer();
                tap.Tapped += Tap_Tapped;

                _circles[i].GestureRecognizers.Add(tap);
            }

            stackLayout.Children.Add(header);
            stackLayout.Children.Add(editor);
            stackLayout.Children.Add(trafficLightSL);
            stackLayout.Children.Add(nightModeBtn);
            stackLayout.Children.Add(turnOnBtn);
            Content = stackLayout;

            editor.TextChanged += Editor_TextChanged;
            nightModeBtn.Clicked += NightModeBtn_Clicked;
            turnOnBtn.Clicked += TurnOnBtn_Clicked;
        }

        private void Editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            Editor editor = (Editor)sender;

            try
            {
                _timer = int.Parse(editor.Text);
            }
            catch (Exception)
            {
                editor.Placeholder = "Ainult Täisarvud!";
                editor.Text = "";
                _timer = 2;
            }
        }

        private void NightModeBtn_Clicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (!_isNightModeWorking && !_isTrafficLightWorking)
            {
                _isTrafficLightWorking = false;
                _isNightModeWorking = true;
                button.Text = "Lülita Välja Öörežiim";
                button.BackgroundColor = Color.DarkSlateGray;
            }
            else if (!_isTrafficLightWorking)
            {
                _isNightModeWorking = false;
                _stopTimer = 0;
                button.Text = "Lülita Sisse Öörežiim";
                button.BackgroundColor = Color.DarkGray;
            }

            NightMode();
        }

        private async void Tap_Tapped(object sender, EventArgs e)
        {
            BoxView boxView=(BoxView)sender;
            int id = _circles.IndexOf(boxView);
            Label label = _labels[id];
            string defaultText = _trafficLightNames[id];

            if (!_isTrafficLightWorking)
            {
                label.Text="Valgusfoor On Lülitud Välja!";
                await Task.Delay(1000);
                label.Text = defaultText;
            }
            else
            {
                if (boxView.Color==Color.Red)
                    label.Text = "Stop!";
                else if (boxView.Color == Color.Yellow)
                    label.Text = "Oota";
                else if (boxView.Color == Color.Green)
                    label.Text = "Võib Minna";
                await Task.Delay(2000);
                label.Text = defaultText;
            }
        }

        private void TurnOnBtn_Clicked(object sender, EventArgs e)
        {          
            Button button = (Button)sender;

            if (!_isTrafficLightWorking && !_isNightModeWorking)
            {
                _isTrafficLightWorking = true;
                _stopTimer = 1;
                button.Text = "Lülita Välja";
                button.BackgroundColor = Color.DarkSlateGray;
            }
            else
            {
                _isTrafficLightWorking = false;
                _stopTimer = 0;
                button.Text = "Lülita Sisse";
                button.BackgroundColor = Color.DarkGray;
            }

            TrafficLightSystem(_isTrafficLightWorking);
        }

        private async void TextTimer(int labelId, int time)
        {
            while (time>0)
            {
                time-=10;
                await Task.Delay(10);
                _labels[labelId].Text = time.ToString();
            }
            _labels[labelId].Text = _trafficLightNames[labelId];
        }

        private async void TrafficLightSystem(bool isWorking)
        {
            foreach (var circle in _circles)
            {
                circle.Color = Color.Gray;
            }

            if (!isWorking)
            {
                return;
            }

            _circles[2].Color= Color.Green;
            TextTimer(2, _timer * 1000);
            await Task.Delay(_timer*1000);
            for (int i = 0; i < 5; i++)
            {
                _circles[2].Color = Color.Green;
                await Task.Delay(_timer*100 * _stopTimer);
                _circles[2].Color = Color.Gray;
                await Task.Delay(_timer*100 * _stopTimer);
            }

            _circles[1].Color = Color.Yellow;
            await Task.Delay(_timer * 330 * _stopTimer);
            _circles[1].Color = Color.Gray;

            _circles[0].Color = Color.Red;
            await Task.Delay(_timer * 1000 * _stopTimer);
            _circles[1].Color = Color.Yellow;
            await Task.Delay(_timer * 330 * _stopTimer);
            _circles[0].Color = Color.Gray;
            _circles[1].Color = Color.Gray;

            TrafficLightSystem(_isTrafficLightWorking);
        }

        private async void NightMode()
        {
            foreach (var circle in _circles)
            {
                circle.Color = Color.Gray;
            }

            while (_isNightModeWorking)
            {
                _circles[1].Color = Color.Yellow;
                await Task.Delay(1000);
                _circles[1].Color = Color.Gray;
                await Task.Delay(1000);
            }
        }
    }
}