using Bloop.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;

namespace Bloop.GitHub
{
    public class GitHubPlugin : IPlugin, ISettingProvider
    {
        public Control CreateSettingPanel()
        {
            return new GitHubPluginSettings();
        }

        public void Init(PluginInitContext context)
        {
            PluginSettings.LoadSettings(Path.Combine(context.CurrentPluginMetadata.PluginDirectory, "settings.json"));
            // PluginSettings.Instance.SettingsChanged = OnSettingsChanged;
        }

        public List<Result> Query(Query query)
        {
            throw new NotImplementedException();
        }
    }
}
