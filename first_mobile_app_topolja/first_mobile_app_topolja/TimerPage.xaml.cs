using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace first_mobile_app_topolja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimerPage : ContentPage
    {
        private bool _timerIsWorking;
        private Color _defaultLblColor;
        public TimerPage()
        {
            InitializeComponent();
            Start();
        }

        private void Start()
        {
            _defaultLblColor = lbl.BackgroundColor;

            ShowTimer();
        }

        private async void ShowTimer()
        {
            if (!_timerIsWorking)
            {
                _timerIsWorking = true;
                lbl.BackgroundColor= _defaultLblColor;
            }
            else
            {
                _timerIsWorking = false;
                lbl.BackgroundColor= Color.IndianRed;
            }

            while (_timerIsWorking)
            {
                lbl.Text = DateTime.Now.ToString("T");
                await Task.Delay(1000);
            }
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            ShowTimer();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
            await Navigation.PushAsync(new MainPage());
        }
    }
}