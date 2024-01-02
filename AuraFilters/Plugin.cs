using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using AuraFilters.Windows;

namespace AuraFilters
{
    public sealed class Plugin : IDalamudPlugin
    {
        public Configuration Configuration { get; init; }

        private DalamudPluginInterface _pluginInterface { get; init; }
        private ICommandManager _commandManager { get; init; }
        private readonly WindowSystem _windowSystem = new("AuraFilters");

        private ConfigWindow _configWindow { get; init; }

        private const string COMMAND_NAME = "/paf";

        public Plugin(
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] ICommandManager commandManager)
        {
            _pluginInterface = pluginInterface;
            _commandManager = commandManager;

            Configuration = this._pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            Configuration.Initialize(this._pluginInterface);

            _configWindow = new ConfigWindow(this);
            
            _windowSystem.AddWindow(_configWindow);

            _commandManager.AddHandler(COMMAND_NAME, new CommandInfo(OnCommand)
            {
                HelpMessage = "Opens configuration for Aura Filters plugin"
            });

            _pluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
        }

        public void Dispose()
        {
            _windowSystem.RemoveAllWindows();
            
            _configWindow.Dispose();
            
            _commandManager.RemoveHandler(COMMAND_NAME);
        }

        private void OnCommand(string command, string args) => DrawConfigUI();

        public void DrawConfigUI() => _configWindow.IsOpen = true;
    }
}
