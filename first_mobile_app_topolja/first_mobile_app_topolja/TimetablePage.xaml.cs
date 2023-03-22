using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace first_mobile_app_topolja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimetablePage : ContentPage
    {
        private Label _headerLbl;
        private Image _image;
        private TimePicker _timePicker;
        private SimpleTask _currentTask;

        private List<SimpleTask> _tasks = new List<SimpleTask>() 
        {  
            new SimpleTask("Toidu valmistma", new TimeSpan(9, 0, 0),new TimeSpan(9, 30, 0), "makeFood.png"),
            new SimpleTask("Sööma", new TimeSpan(9, 30, 0),new TimeSpan(10, 0, 0), "eat.png"),
            new SimpleTask("Õppima", new TimeSpan(10, 0, 0),new TimeSpan(15, 0, 0), "study.png"),
            new SimpleTask("Treenima", new TimeSpan(15, 0, 0),new TimeSpan(17, 0, 0), "gym.png"),
            new SimpleTask("Jalutama", new TimeSpan(17, 0, 0),new TimeSpan(18, 0, 0), "goWalk.png"),
            new SimpleTask("Töötama", new TimeSpan(18, 0, 0),new TimeSpan(20, 0, 0), "work.png"),
            new SimpleTask("Rääkida", new TimeSpan(20, 0, 0),new TimeSpan(20, 15, 0), "talk.png"),
            new SimpleTask("Programmerima", new TimeSpan(20, 15, 0),new TimeSpan(22, 0, 0), "coding.png"),
            new SimpleTask("Mängima", new TimeSpan(22, 0, 0),new TimeSpan(23, 0, 0), "playGames.png"),
            new SimpleTask("Puhkama", new TimeSpan(23, 0, 0),new TimeSpan(23, 30, 0), "chill.png"),
            new SimpleTask("Vaatama filmi", new TimeSpan(23, 30, 0),new TimeSpan(24, 0, 0), "film.png"),
            new SimpleTask("Magama", new TimeSpan(0, 0, 0),new TimeSpan(9, 0, 0), "sleep.png"),
        };

        public TimetablePage()
        {
            _headerLbl = new Label
            {
                Text = "Tunniplaan",
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center
            };
            _image = new Image
            {
                Source = "tunniplaan.png"
            };
            _timePicker = new TimePicker
            {
                Format = "HH:mm"

            };

            StackLayout stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.White,
                Children = { _headerLbl, _image, _timePicker }
            };

            Content= stackLayout;

            _timePicker.PropertyChanged += _timePicker_PropertyChanged;

            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += Tap_Tapped;

            _image.GestureRecognizers.Add(tap);

        }

        private void Tap_Tapped(object sender, EventArgs e)
        {
            if (_currentTask!=null)
                DisplayAlert("Ülesanne", $"Praegu teile on vaja {_currentTask.Name}!", "Ok");
        }

        private void _timePicker_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            int tasks = 0;
            foreach (var task in _tasks)
            {
                if (_timePicker.Time >= task.StartTime && _timePicker.Time < task.EndTime)
                {
                    _headerLbl.Text = $"Ülessane: {task.Name}";
                    _image.Source = task.ImageDir;
                    _currentTask= task;
                    tasks++;
                }
            }
            if (tasks==0)
            {
                _headerLbl.Text = "Tunniplaan";
                _image.Source = "tunniplaan.png";
                _currentTask = null;
            }
        }
    }

    class SimpleTask
    {
        public SimpleTask(string name, TimeSpan startTime, TimeSpan endTime, string imageDir)
        {
            Name=name;
            StartTime=startTime;
            EndTime=endTime;
            ImageDir=imageDir;
        }
        public string Name { get; private set; }
        public TimeSpan StartTime { get; private set; }
        public TimeSpan EndTime { get; private set; }
        public string ImageDir { get; private set; }
    }
}