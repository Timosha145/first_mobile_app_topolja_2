using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace first_mobile_app_topolja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class MainPage : ContentPage
    {
        private List<CPage> _cpages = new List<CPage>() {
            new CPage("text page", new TextPage()),
            new CPage("timer page", new TimerPage()),
            new CPage("box page", new BoxPage()),
            new CPage("RGB Page", new RGBPage()),
            new CPage("Horoscope Page", new HoroscopePage()),
            new CPage("TimeTable Page", new TimetablePage()),
            new CPage("MyOwn Page", new MyOwnPage())
        };

        public MainPage()
        {
            StackLayout stackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.White
            };

            for (int i = 0; i < _cpages.Count; i++)
            {
                Button button = new Button()
                {
                    Text = _cpages[i].Title,
                    TabIndex = i,
                    BackgroundColor = Color.DarkGray,
                    TextColor = Color.WhiteSmoke,
                };

                button.Clicked += Button_Clicked;
                stackLayout.Children.Add(button);
            }

            Content = stackLayout;
        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            Button btn = (Button)sender ;

            await Navigation.PopAsync();
            await Navigation.PushAsync(_cpages[btn.TabIndex].Page);
        }
    }
}

public class CPage
{
    public CPage(string title, ContentPage page)
    {
        Title = title;
        Page = page;
    }

    public string Title { get; private set; }
    public ContentPage Page { get; private set; }
}