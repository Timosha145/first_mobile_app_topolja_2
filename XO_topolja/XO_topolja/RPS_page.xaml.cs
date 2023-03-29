using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XO_topolja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TTT_page : ContentPage
    {
        private string _beginner = "❌";
        private string[] _variants = new string[] { "❌", "⭕" };
        private List<Label> _variantsLbl = new List<Label>();
        private int _player = 1;
        private bool playable;

        private Button _beginBtn, _newGamebtn;
        private List<Label> _labels = new List<Label>();
        private Grid _grid;
        private CheckBox _withBot;

        public TTT_page()
        {
            Game();
        }

        private void Game()
        {
            StackLayout stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                BackgroundColor = Color.White
            };

            StackLayout stackLayoutTurn = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = Color.White
            };

            StackLayout stackLayoutBot = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                BackgroundColor = Color.White
            };

            for (int i = 0; i < 2; i++)
            {
                Label label = new Label
                {
                    Text = _variants[i],
                    FontSize = 55,
                    WidthRequest = 200,
                    HorizontalTextAlignment = TextAlignment.Center,
                    IsEnabled = false
                };

                _variantsLbl.Add(label);

                stackLayoutTurn.Children.Add(label);
            }

            _newGamebtn = new Button
            {
                Text = "Uus Mäng",
                BackgroundColor = Color.DarkGray,
                TextColor = Color.WhiteSmoke,
                IsEnabled = false
            };

            _beginBtn = new Button
            {
                Text = "Alusta",
                BackgroundColor = Color.DarkGray,
                TextColor = Color.WhiteSmoke,
            };

            _grid = new Grid
            {
                WidthRequest = 400,
                HeightRequest = 400,
                BackgroundColor = Color.Gainsboro,
                Padding = 10
            };

            Label labelForWithBot = new Label
            {
                Text = "Mängin bottiga: ",
                FontSize = 25,
                WidthRequest = 200,
                HorizontalTextAlignment = TextAlignment.Center,
            };
            _withBot = new CheckBox();

            stackLayoutBot.Children.Add(labelForWithBot);
            stackLayoutBot.Children.Add(_withBot);

            _newGamebtn.Clicked += NewGamebtn_Clicked;
            _beginBtn.Clicked += beginBtn_Clicked;

            Content = stackLayout;

            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += Tap_Tapped;

            int index=0;
            for (int i = 0; i < 3; i++)
            {
                _grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                _grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                for (int ii = 0; ii < 3; ii++)
                {
                    index++;

                    Frame frame = new Frame
                    {
                        BackgroundColor = Color.Peru,
                        CornerRadius = 10,
                        WidthRequest = 70,
                        HeightRequest = 70,
                    };

                    Label label = new Label
                    {
                        Text = "",
                        FontSize = 55,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
                        TabIndex=index
                    };

                    _labels.Add(label);
                    frame.Content = label;
                    _grid.Children.Add(frame, i, ii);


                    label.GestureRecognizers.Add(tap);
                }
            }

            stackLayout.Children.Add(_grid);
            stackLayout.Children.Add(stackLayoutTurn);
            stackLayout.Children.Add(_newGamebtn);
            stackLayout.Children.Add(_beginBtn);
            stackLayout.Children.Add(stackLayoutBot);
        }

        private async void Tap_Tapped(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;

            if (lbl.Text != "" || !playable)
                return;

            _player = _player==1 ? 0 : 1;

            string sym = _variants[_player];
            lbl.Text = sym;

            _variantsLbl[1-_player].IsEnabled = true;
            _variantsLbl[_player].IsEnabled = false;


            if (CheckWin(0, 1, 2, sym) || CheckWin(3, 4, 5, sym) || CheckWin(6, 7, 8, sym) ||
                CheckWin(0, 3, 6, sym) || CheckWin(1, 4, 7, sym) || CheckWin(2, 5, 8, sym) ||
                CheckWin(0, 4, 8, sym) || CheckWin(2, 4, 6, sym))
            {
                playable = false;
                await DisplayAlert("Võitja", $"Võitja on {sym}!", "OK");
            }
        }

        private bool CheckWin(int a, int b, int c, string sym)
        {
            return (_labels[a].Text == sym && _labels[b].Text == sym && _labels[c].Text == sym);
        }

        private async void beginBtn_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Küsimus", "Kas te soovite juhuslikult valida algijat?", "Jah", "Ei");

            if (!answer)
            {
                await DisplayAlert("Algija", "Reeglite järgi algab mängija ❌.", "OK");
            }
            else if (answer)
            {
                Random r = new Random();
                int i = r.Next(0, 2);
                _beginner = _variants[i];

                await DisplayAlert("Algija", $"Algab mängija {_beginner}.", "OK");

                if (_beginner == "⭕")
                    _player = 0;
                else
                    _player = 1;

                _variantsLbl[i].IsEnabled = true;
            }

            if (!_variantsLbl[1].IsEnabled)
                _variantsLbl[0].IsEnabled = true;

            _beginBtn.IsEnabled = false;
            _withBot.IsEnabled = false;
            _newGamebtn.IsEnabled = true;
            playable = true;
        }

        private void NewGamebtn_Clicked(object sender, EventArgs e)
        {
            _beginBtn.IsEnabled = true;
            _withBot.IsEnabled = true;

            _variants = new string[] { "❌", "⭕" };

            foreach (Label label in _labels)
            {
                label.Text = "";
            }

            foreach (Label label in _variantsLbl)
            {
                label.IsEnabled = false;
            }

            _newGamebtn.IsEnabled = false;
        }
    }
}