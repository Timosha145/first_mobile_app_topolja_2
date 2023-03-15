using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace first_mobile_app_topolja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BoxPage : ContentPage
    {
        private BoxView _box;
        private bool _isClicked;
        public BoxPage()
        {
            _box = new BoxView()
            {
                Color = Color.Bisque,
                CornerRadius = 100,
                WidthRequest = 200,
                HeightRequest = 300,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            TapGestureRecognizer tap=new TapGestureRecognizer();
            tap.Tapped += Tap_Tapped;
            _box.GestureRecognizers.Add(tap);

            Content = _box;
        }


        private async void Tap_Tapped(object sender, EventArgs e)
        {
            BoxView box = (BoxView)sender;
            Random random = new Random();

            Color randomColor = Color.FromRgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));

            if (!_isClicked)
            {
                _isClicked = true;
                for (int i = 1; i < 20; i++)
                {
                    box.Rotation += i;
                    box.Color = randomColor;

                    try
                    {
                        Vibration.Vibrate();
                        var a = TimeSpan.FromSeconds(0.5f);
                        Vibration.Vibrate(a);
                    }
                    catch (Exception)
                    {
                    }

                    await Task.Delay(100);
                }



                _isClicked = false;
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
            await Navigation.PushAsync(new MainPage());
        }
    }
}