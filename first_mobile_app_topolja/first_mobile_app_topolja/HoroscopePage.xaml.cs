using first_mobile_app_topolja;
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
    public partial class HoroscopePage : ContentPage
    {
        private DatePicker _calendar;
        private Image _horoscopeImage;
        private Label _inputLbl, _editorLbl, _horoscopeLbl;
        private Editor _horoscopeInput;
        private List<Horoscope> _horoscopeList = new List<Horoscope>()
        {
            new Horoscope("Veevalaja", "veevalaja.png", new DateTime(2000, 1, 21), new DateTime(2000, 2, 18)),
            new Horoscope("Kalad", "kalad.png", new DateTime(2000, 2, 19), new DateTime(2000, 3, 20)),
            new Horoscope("Jäär", "jaar.png", new DateTime(2000, 3, 21), new DateTime(2000, 4, 20)),
            new Horoscope("Sõnn", "sonn.png", new DateTime(2000, 4, 21), new DateTime(2000, 5, 20)),
            new Horoscope("Kaksikud", "kaksikud.png", new DateTime(2000, 5, 21), new DateTime(2000, 6, 21)),
            new Horoscope("Vähk", "vahk.png", new DateTime(2000, 6, 22), new DateTime(2000, 7, 22)),
            new Horoscope("Lõvi", "lovi.png", new DateTime(2000, 7, 23), new DateTime(2000, 8, 22)),
            new Horoscope("Neitsi", "neitsi.png", new DateTime(2000, 8, 23), new DateTime(2000, 9, 22)),
            new Horoscope("Kaalud", "kaalud.png", new DateTime(2000, 9, 23), new DateTime(2000, 10, 23)),
            new Horoscope("Skorpion", "skorpion.png", new DateTime(2000, 10, 24), new DateTime(2000, 11, 22)),
            new Horoscope("Ambur", "ambur.png", new DateTime(2000, 11, 23), new DateTime(2000, 12, 21)),
            new Horoscope("Kaljukits", "kaljukits.png", new DateTime(2000, 12, 22), new DateTime(2000, 1, 20))
        };

        public Label test;

        public HoroscopePage()
        {
            _horoscopeImage = new Image
            {
                Source = ImageSource.FromFile("horoscopeDefault.jpg")
            };

            _inputLbl = new Label
            {
                Text = "Teie Sünnipäev: "
            };
            _calendar = new DatePicker
            {
                TextColor = Color.WhiteSmoke,
                BackgroundColor = Color.DarkGray,
                WidthRequest = 200
            };

            _editorLbl = new Label
            {
                Text= "Teie Horoskoop: "
            };

            _horoscopeLbl = new Label
            {
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center
            };

            _horoscopeInput = new Editor
            {
                Placeholder = "Kirjuta tekst",
                BackgroundColor = Color.DarkGray,
                TextColor = Color.WhiteSmoke,
                PlaceholderColor = Color.WhiteSmoke,
                WidthRequest = 200
            };

            StackLayout stackLayoutCalendar = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = Color.White,
                Children = { _inputLbl, _calendar }
            };

            StackLayout stackLayoutEditor = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = Color.White,
                Children = { _editorLbl, _horoscopeInput }
            };

            StackLayout stackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.White,
                Children = { _horoscopeLbl, _horoscopeImage, stackLayoutCalendar, stackLayoutEditor}
            };

            Content = stackLayout;

            _calendar.DateSelected += _calendar_DateSelected;
            _horoscopeInput.TextChanged += _horoscopeInput_TextChanged;
        }

        private void _horoscopeInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            foreach (var horoscope in _horoscopeList)
            {
                if (_horoscopeInput.Text == horoscope.Name)
                {
                    int stDay = horoscope.StartDate.Day;
                    int stMonth = horoscope.StartDate.Month;
                    int endDay = horoscope.EndDate.Day;
                    int endMonth = horoscope.EndDate.Month;

                    _horoscopeImage.Source = horoscope.ImageDir;
                    _horoscopeLbl.Text = _horoscopeLbl.Text = $"{horoscope.Name}: {stDay}.{stMonth} - {endDay}.{endMonth}";
                    _horoscopeInput.Text = "";
                }
            }
        }

        private void _calendar_DateSelected(object sender, DateChangedEventArgs e)
        {
            foreach (var horoscope in _horoscopeList)
            {
                int cDay = _calendar.Date.Day;
                int cMonth = _calendar.Date.Month;
                int stDay = horoscope.StartDate.Day;
                int stMonth = horoscope.StartDate.Month;
                int endDay = horoscope.EndDate.Day;
                int endMonth = horoscope.EndDate.Month;

                if (cDay>=stDay && cMonth==stMonth || cMonth == endMonth && cDay <= endDay)
                {
                    _horoscopeImage.Source = horoscope.ImageDir;
                    _horoscopeLbl.Text = $"{horoscope.Name}: {stDay}.{stMonth} - {endDay}.{endMonth}";
                    _horoscopeInput.Text = "";
                }
            }
        }
    }
}

class Horoscope
{
    public Horoscope(string name, string imageDir, DateTime startDate, DateTime endDate)
    {
        Name = name;
        ImageDir = imageDir;
        StartDate = startDate;
        EndDate = endDate;
    }

    public string Name { get; private set; }
    public string ImageDir { get; private set; } 
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

}