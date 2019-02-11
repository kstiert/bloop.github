using Bloop.GitHub.Shell;
using Bloop.Plugin;
using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Bloop.GitHub
{
    public class GitHubPlugin : IPlugin, ISettingProvider
    {
        private readonly IDictionary<string, IShell> _shells = new Dictionary<string, IShell>();
        private IReadOnlyList<Repository> _repos;
        private PluginInitContext _context;

        public Control CreateSettingPanel()
        {
            return new GitHubPluginSettings(_shells);
        }

        public void Init(PluginInitContext context)
        {
            _context = context;
            PluginSettings.LoadSettings(Path.Combine(context.CurrentPluginMetadata.PluginDirectory, "settings.json"));
            PluginSettings.Instance.SettingsChanged = RefreshRepos;
            RefreshRepos();

            foreach (var shellType in Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(IShell).IsAssignableFrom(t) && !t.IsInterface))
            {
                var shell = (IShell)Activator.CreateInstance(shellType);
                _shells[shell.Name] = shell;
            }
        }

        public List<Result> Query(Query query)
        {
            if(_repos == null)
            {
                return OpenSettingsResult();
            }

            var exact = _repos.FirstOrDefault(r => r.FullName == query.Search.Trim(' ') && query.Search.EndsWith(" "));

            if (exact != null)
            {
                return RepoMatchResults(exact);
            }
            else
            {
                return RepoSearchResults(query);
            }
        }

        public async void RefreshRepos()
        {
            try
            {
                var client = new GitHubClient(new ProductHeaderValue("Bloop.GitHub"));
                client.Credentials = new Credentials(PluginSettings.Instance.Token);
                _repos = await client.Repository.GetAllForCurrent();
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
                    Action = ctx => { _context.API.OpenSettingDialog("plugins"); return true; },
                    Title = "Open Settings",
                    SubTitle = "Configure GitHub Plugin",
                    IcoPath = "images\\icon.png"
                }}.ToList();
        }

        private List<Result> RepoSearchResults(Query query)
        {
            var match = _repos.Where(r => r.FullName.Contains(query.Search));
            var prefix = _context.CurrentPluginMetadata.ActionKeyword == "*" ? string.Empty : _context.CurrentPluginMetadata.ActionKeyword + " ";
            return match.Select(r => new Result
            {
                Action = ctx =>
                {
                    _context.API.ChangeQuery($"{prefix}{r.FullName} ", true);
                    return false;
                },
                Title = r.Name,
                SubTitle = r.Owner.Login,
                IcoPath = "Images\\repo.png"
            }).ToList();
        }

        private List<Result> RepoMatchResults(Repository repo)
        {
            var results = new List<Result>();

            results.Add(new Result
            {
                Action = ctx =>
                {
                    System.Diagnostics.Process.Start(repo.HtmlUrl);
                    return true;
                },
                Title = "Go",
                SubTitle = repo.HtmlUrl,
                IcoPath = "Images\\globe.png"

            });

            var localPath = Path.Combine(PluginSettings.Instance.RepoRoot, repo.Name);
            if (Directory.Exists(localPath))
            {
                results.Add(new Result
                {
                    Action = ctx =>
                    {
                        System.Diagnostics.Process.Start(localPath);
                        return true;
                    },
                    Title = "Open Folder",
                    SubTitle = localPath,
                    IcoPath = "Images\\folder.png"
                });

                results.Add(new Result
                {
                    Action = ctx =>
                    {
                        _shells[PluginSettings.Instance.Shell].Launch(localPath);
                        return true;
                    },
                    Title = "Open Shell",
                    SubTitle = localPath,
                    IcoPath = "Images\\shell.png"
                });
            }
            else
            {
                results.Add(new Result
                {
                    Action = ctx =>
                    {
                        _shells[PluginSettings.Instance.Shell].Launch(PluginSettings.Instance.RepoRoot, $"git clone {repo.CloneUrl};cd {repo.Name}");
                        return true;
                    },
                    Title = "Clone",
                    SubTitle = localPath,
                    IcoPath = "Images\\clone.png"
                });
            }

            return results;
        }
    }
}
