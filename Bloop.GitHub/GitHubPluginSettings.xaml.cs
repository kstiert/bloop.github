using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
