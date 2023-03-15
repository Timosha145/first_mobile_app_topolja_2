using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace first_mobile_app_topolja
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TextPage : ContentPage
    {
        private Label _label;

        public TextPage()
        {
            Button goBackBtn = new Button()
            {
                Text = "Tagasi",
                BackgroundColor = Color.DarkGray,
                TextColor = Color.WhiteSmoke
            };

            Editor editor = new Editor()
            {
                Placeholder = "Kirjuta tekst",
                BackgroundColor = Color.DarkGray,
                TextColor = Color.WhiteSmoke,
                PlaceholderColor = Color.WhiteSmoke
            };

            _label = new Label()
            {
                BackgroundColor = Color.Beige,
                FontSize= 15
            };

            StackLayout stackLayout = new StackLayout()
            {
                Children = { editor, _label, goBackBtn }
            };

            Content = stackLayout;
            goBackBtn.Clicked += GoBackBtn_Clicked;
            editor.TextChanged += Editor_TextChanged;
        }

        private void Editor_TextChanged(object sender, TextChangedEventArgs e)
        {
            Editor Editor = (Editor)sender;
            _label.Text = Editor.Text;
        }

        private async void GoBackBtn_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
            await Navigation.PushAsync(new MainPage());
        }
    }
}