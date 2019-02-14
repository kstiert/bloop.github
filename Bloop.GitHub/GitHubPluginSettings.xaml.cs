using System.Collections.Generic;
using System.Windows.Controls;
using Bloop.GitHub.Shell;

namespace Bloop.GitHub
{
    /// <summary>
    /// Interaction logic for GitHubPluginSettings.xaml
    /// </summary>
    public partial class GitHubPluginSettings : UserControl
    {
        public GitHubPluginSettings(IDictionary<string, IShell> shells)
        {
            InitializeComponent();
            Token.Text = PluginSettings.Instance.Token;
            RepoRoot.Text = PluginSettings.Instance.RepoRoot;
            foreach (var shell in shells.Values)
            {
                var radio = new RadioButton
                {
                    GroupName = "Shells",
                    Content = shell.Name,
                    IsChecked = PluginSettings.Instance.Shell == shell.Name,
                    IsEnabled = shell.Avalible
                };
                radio.Checked += Radio_Checked;
                Shells.Children.Add(radio);
            }
        }

        private void Token_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            PluginSettings.Instance.Token = textBox.Text;
            PluginSettings.Instance.Save();
        }

        private void RepoRoot_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            PluginSettings.Instance.RepoRoot = textBox.Text;
            PluginSettings.Instance.Save();
        }

        private void Browse_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            if (dialog.ShowDialog() ?? false)
            {
                RepoRoot.Text = dialog.SelectedPath;
            }
        }

        private void Radio_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            var radio = (RadioButton)sender;
            PluginSettings.Instance.Shell = radio.Content.ToString();
            PluginSettings.Instance.Save();
        }
    }
}
