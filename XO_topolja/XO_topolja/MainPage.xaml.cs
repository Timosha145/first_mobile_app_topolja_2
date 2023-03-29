using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XO_topolja
{
    public partial class MainPage : ContentPage
    {
        private List<CPage> _cpages = new List<CPage>() {
            new CPage("Tic-Tac-Toe", new TTT_page())
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
            Button btn = (Button)sender;

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