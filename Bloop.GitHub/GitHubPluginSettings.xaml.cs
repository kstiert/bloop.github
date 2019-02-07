using System.Windows.Controls;


namespace Bloop.GitHub
{
    /// <summary>
    /// Interaction logic for GitHubPluginSettings.xaml
    /// </summary>
    public partial class GitHubPluginSettings : UserControl
    {
        public GitHubPluginSettings()
        {
            InitializeComponent();
            Token.Text = PluginSettings.Instance.Token;
        }

        private void Token_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            PluginSettings.Instance.Token = textBox.Text;
            PluginSettings.Instance.Save();
        }
    }
}
