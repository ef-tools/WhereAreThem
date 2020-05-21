using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhereAreThem.Model.Models;

namespace WhereAreThem.Model.Plugins {
    public class PluginManager {
        [ImportMany]
        private readonly IEnumerable<Lazy<IPlugin, IPluginMetadata>> _importedPlugins;

        private Dictionary<string, List<Lazy<IPlugin, IPluginMetadata>>> _plugins =
            new Dictionary<string, List<Lazy<IPlugin, IPluginMetadata>>>(StringComparer.OrdinalIgnoreCase);

        public PluginManager() {
            CompositionContainer container = new CompositionContainer(new DirectoryCatalog("."));
            container.ComposeParts(this);

            foreach (var p in _importedPlugins) {
                foreach (string ext in p.Metadata.Extensions) {
                    if (!_plugins.ContainsKey(ext))
                        _plugins.Add(ext, new List<Lazy<IPlugin, IPluginMetadata>>());
                    _plugins[ext].Add(p);
                }
            }
        }

        public Dictionary<string, string> GetDescriptions(string path, bool hasChanged, Dictionary<string, string> descriptions) {
            string ext = Path.GetExtension(path);

            if (!_plugins.ContainsKey(ext))
                return null;

            Dictionary<string, string> newDescriptions = new Dictionary<string, string>();
            foreach (var p in _plugins[ext]) {
                if (hasChanged || !descriptions.ContainsKey(p.Metadata.Id))
                    newDescriptions.Add(p.Metadata.Id, p.Value.GetDescription(path));
                else
                    newDescriptions.Add(p.Metadata.Id, descriptions[p.Metadata.Id]);
            }

            return newDescriptions;
        }
    }
}
