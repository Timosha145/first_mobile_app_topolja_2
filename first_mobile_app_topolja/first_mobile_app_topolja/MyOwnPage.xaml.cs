using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace first_mobile_app_topolja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyOwnPage : ContentPage
    {
        private TimePicker _timePicker;
        private Button _addTimerBtn;
        private StackLayout _stackLayoutMain;
        private List<CostumTimer> _costumTimers = new List<CostumTimer>();

        public MyOwnPage()
        {
            _timePicker = new TimePicker
            {
                Format = "HH:mm",
                TextColor = Color.Black
            };

            _addTimerBtn = new Button
            {
                Text = "Lisa",
                FontSize = 20,
                BackgroundColor = Color.DarkGray,
                TextColor = Color.WhiteSmoke,
            };

            StackLayout stackLayoutEditor = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = Color.White,
                Children = { _timePicker, _addTimerBtn }
            };

            _stackLayoutMain = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.White,
                Children = { stackLayoutEditor, stackLayoutEditor }
            };

            Content = _stackLayoutMain;
            _addTimerBtn.Clicked += _addTimerBtn_Clicked;
        }

        private void _addTimerBtn_Clicked(object sender, EventArgs e)
        {
            Label timerLbl = new Label
            {
                Text = _timePicker.Time.ToString("T"),
                FontSize = 30,
                TextColor = Color.DarkGray
            };

            Button startTimerBtn = new Button
            {
                Text = "▶️",
                WidthRequest = 50,
                BackgroundColor = Color.DarkGray,
                TextColor = Color.WhiteSmoke,
            };

            Button stopTimerBtn = new Button
            {
                Text = "⏸",
                WidthRequest = 50,
                BackgroundColor = Color.DarkGray,
                TextColor = Color.WhiteSmoke,
                IsEnabled = false
            };

            Button restartTimerBtn = new Button
            {
                Text = "↻",
                WidthRequest = 50,
                BackgroundColor = Color.DarkGray,
                TextColor = Color.WhiteSmoke,
            };

            Button deleteTimerBtn = new Button
            {
                Text = "✖",
                WidthRequest = 50,
                BackgroundColor = Color.DarkGray,
                TextColor = Color.WhiteSmoke,
            };

            StackLayout stackLayoutTimer = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = Color.White,
                Children = { timerLbl, startTimerBtn, stopTimerBtn, restartTimerBtn, deleteTimerBtn }
            };

            _costumTimers.Add(new CostumTimer(timerLbl, startTimerBtn, stopTimerBtn, restartTimerBtn, deleteTimerBtn,  stackLayoutTimer, _timePicker.Time, false));
            CostumTimer costumTimer = _costumTimers[_costumTimers.Count - 1];

            costumTimer.StartTimerBtn.TabIndex = _costumTimers.Count - 1;
            costumTimer.StopTimerBtn.TabIndex = _costumTimers.Count - 1;
            costumTimer.RestartTimerBtn.TabIndex = _costumTimers.Count - 1;
            costumTimer.DeleteTimerBtn.TabIndex = _costumTimers.Count - 1;

            startTimerBtn.Clicked += _startTimerBtn_Clicked;
            stopTimerBtn.Clicked += _stopTimerBtn_Clicked;
            restartTimerBtn.Clicked += RestartBtn_Clicked;
            deleteTimerBtn.Clicked += DeleteTimerBtn_Clicked;

            _stackLayoutMain.Children.Add(stackLayoutTimer);
        }

        private void DeleteTimerBtn_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            CostumTimer costumTimer = _costumTimers[btn.TabIndex];

            costumTimer.Stop = true;
            _stackLayoutMain.Children.Remove(costumTimer.StackLayoutTimer);
        }

        private void RestartBtn_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            CostumTimer costumTimer = _costumTimers[btn.TabIndex];

            costumTimer.Stop = true;
            costumTimer.Timer = costumTimer.DefaultTimer;
            costumTimer.TimerLbl.Text = costumTimer.Timer.ToString("T");
            costumTimer.TimerLbl.TextColor = Color.DarkGray;
        }

        private void _stopTimerBtn_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            CostumTimer costumTimer = _costumTimers[btn.TabIndex];

            costumTimer.Stop = true;
        }

        private async void _startTimerBtn_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            CostumTimer costumTimer = _costumTimers[btn.TabIndex];

            costumTimer.Stop = false;
            costumTimer.StopTimerBtn.IsEnabled = true;
            costumTimer.StartTimerBtn.IsEnabled = false;
            costumTimer.TimerLbl.TextColor = Color.LimeGreen;

            double totalSec = costumTimer.DefaultTimer.TotalSeconds;

            while (costumTimer.Timer >= new TimeSpan(0,0,0) && !costumTimer.Stop)
            {
                costumTimer.TimerLbl.Text = costumTimer.Timer.ToString("T");
                await Task.Delay(100);
                costumTimer.Timer -= new TimeSpan(0, 0, 1);

                double totalNowSeconds = costumTimer.Timer.Seconds;

                if ((totalSec * 0.75f) > totalNowSeconds && (totalSec * 0.5f) < totalNowSeconds)
                {
                    costumTimer.TimerLbl.TextColor = Color.DarkGreen;
                }
                else if ((totalSec * 0.5f) > totalNowSeconds && (totalSec * 0.25f) < totalNowSeconds)
                {
                    costumTimer.TimerLbl.TextColor = Color.Orange;
                }
                else if ((totalSec * 0.25f) > totalNowSeconds)
                {
                    costumTimer.TimerLbl.TextColor = Color.Red;
                }

                if (totalNowSeconds==0)
                {
                    try
                    {
                        // Or use specified time
                        var duration = TimeSpan.FromSeconds(5);
                        Vibration.Vibrate(duration);
                        await DisplayAlert("Timer", "Aeg on läbinud!", "Ok");
                    }
                    catch (Exception)
                    {
                        // Other error has occurred.
                    }
                }
            }

            costumTimer.StopTimerBtn.IsEnabled = false;
            costumTimer.StartTimerBtn.IsEnabled = true;
        }
    }
}

class CostumTimer
{
    public CostumTimer(Label timerLbl, Button startTimerBtn, Button stopTimerBtn, Button restartTimerBtn, Button deleteTimerbtn, 
        StackLayout stackLayoutTimer, TimeSpan timer, bool stop)
    {
        TimerLbl = timerLbl;
        StartTimerBtn = startTimerBtn;
        StopTimerBtn = stopTimerBtn;
        RestartTimerBtn = restartTimerBtn;
        DeleteTimerBtn = deleteTimerbtn;
        StackLayoutTimer = stackLayoutTimer;
        Timer = timer;
        DefaultTimer = timer;
        Stop = stop;

    }

    public Label TimerLbl { get; set; }
    public Button StartTimerBtn { get; set; }
    public Button StopTimerBtn { get; set; }
    public Button RestartTimerBtn { get; set; }
    public Button DeleteTimerBtn { get; set; }
    public StackLayout StackLayoutTimer { get; set; }
    public TimeSpan Timer { get; set; }
    public TimeSpan DefaultTimer { get; private set; }
    public bool Stop { get; set; }
}