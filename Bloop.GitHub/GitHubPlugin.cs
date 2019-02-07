using Bloop.Plugin;
using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Bloop.GitHub
{
    public class GitHubPlugin : IPlugin, ISettingProvider
    {
        private IReadOnlyList<Repository> _repos;

        public Control CreateSettingPanel()
        {
            return new GitHubPluginSettings();
        }

        public void Init(PluginInitContext context)
        {
            PluginSettings.LoadSettings(Path.Combine(context.CurrentPluginMetadata.PluginDirectory, "settings.json"));
            // PluginSettings.Instance.SettingsChanged = OnSettingsChanged;
            Task.Run((Action)RefreshRepos);
        }

        public List<Result> Query(Query query)
        {
            return null;
        }

        private void RefreshRepos()
        {
            var client = new GitHubClient(new ProductHeaderValue("Bloop.GitHub"));
            client.Credentials = new Credentials("");
            _repos = client.Repository.GetAllForCurrent().Result;
        }
    }
}
