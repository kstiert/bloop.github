using Bloop.Plugin;
using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Bloop.GitHub
{
    public class GitHubPlugin : IPlugin, ISettingProvider
    {
        private IReadOnlyList<Repository> _repos;
        private IPublicAPI _api;

        public Control CreateSettingPanel()
        {
            return new GitHubPluginSettings();
        }

        public void Init(PluginInitContext context)
        {
            _api = context.API;
            PluginSettings.LoadSettings(Path.Combine(context.CurrentPluginMetadata.PluginDirectory, "settings.json"));
            PluginSettings.Instance.SettingsChanged = RefreshRepos;
            Task.Run((Action)RefreshRepos);
        }

        public List<Result> Query(Query query)
        {
            if(_repos == null)
            {
                return OpenSettingsResult();
            }

            var match = _repos.Where(r => r.FullName.Contains(query.Search));

            if(match.Count() == 1)
            {
                var repo = match.Single();
                return new List<Result>
                {
                    new Result
                    {
                        Action = ctx =>
                        {
                            System.Diagnostics.Process.Start(repo.HtmlUrl);
                            return true;
                        },
                        Title = "Go",
                        SubTitle = "Web"
                    }
                };
            }
            else
            {
                return match.Select(r => new Result
                {
                    Action = ctx =>
                    {
                        _api.ChangeQuery(r.FullName, true);
                        return false;
                    },
                    Title = r.Name,
                    SubTitle = r.Owner.Name,
                    IcoPath = "Images\\repo.png"
                }).ToList();
            }
        }

        private void RefreshRepos()
        {
            try
            {
                var client = new GitHubClient(new ProductHeaderValue("Bloop.GitHub"));
                client.Credentials = new Credentials(PluginSettings.Instance.Token);
                _repos = client.Repository.GetAllForCurrent().Result;
            }
            catch
            {
                _repos = null;
            }

        }

        private List<Result> OpenSettingsResult()
        {
            return new List<Result> { new Result
                {
                    Action = ctx => { _api.OpenSettingDialog(); return true; },
                    Title = "Open Settings",
                    SubTitle = "Configure GitHub Plugin",
                    IcoPath = "images\\icon.png"
                }}.ToList();
        }
    }
}
