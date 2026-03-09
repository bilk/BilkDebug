using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using BilkDebug.UI;
using System.IO;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Client.Game;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using FFXIVClientStructs.FFXIV.Client.Game.UI;

namespace BilkDebug;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ITextureProvider TextureProvider { get; private set; } = null!;
    [PluginService] internal static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] internal static IClientState ClientState { get; private set; } = null!;
    [PluginService] internal static IPlayerState PlayerState { get; private set; } = null!;
    [PluginService] internal static IDataManager DataManager { get; private set; } = null!;
    [PluginService] internal static IPluginLog Log { get; private set; } = null!;
    [PluginService] internal static ISigScanner SigScanner { get; private set; } = null!;
    [PluginService] internal static IGameInteropProvider Hook { get; private set; } = null!;

    internal static Plugin P = null!;

    public Configuration Configuration { get; init; }

    public Hooks hooks;

    public readonly WindowSystem WindowSystem = new("BilkDebug");
    private MainWindow MainWindow { get; init; }

    public Plugin(IDalamudPluginInterface pi)
    {
        P = this;

        InteropGenerator.Runtime.Resolver.GetInstance.Setup(
            SigScanner.SearchBase,
            DataManager.GameData.Repositories["ffxiv"].Version,
            new FileInfo( Path.Join( PluginInterface.ConfigDirectory.FullName, "SigCache.json" ) ) );
        FFXIVClientStructs.Interop.Generated.Addresses.Register();
        InteropGenerator.Runtime.Resolver.GetInstance.Resolve();

        hooks = new();

        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();

        MainWindow = new MainWindow(this);

        WindowSystem.AddWindow(MainWindow);

        CommandManager.AddHandler("/bdebug", new CommandInfo(OnCommand)
        {
            HelpMessage = "A useful message to display in /xlhelp"
        });

        PluginInterface.UiBuilder.Draw += WindowSystem.Draw;

        PluginInterface.UiBuilder.OpenConfigUi += ToggleMainUi;

        PluginInterface.UiBuilder.OpenMainUi += ToggleMainUi;
    }

    public void Dispose()
    {
        PluginInterface.UiBuilder.Draw -= WindowSystem.Draw;
        PluginInterface.UiBuilder.OpenConfigUi -= ToggleMainUi;
        PluginInterface.UiBuilder.OpenMainUi -= ToggleMainUi;
        hooks.Dispose();
        WindowSystem.RemoveAllWindows();

        MainWindow.Dispose();

        CommandManager.RemoveHandler("/bdebug");
    }

    private void OnCommand(string command, string args)
    {
        if (args == "go")
        {
            Test();
        }
        else
        {
            MainWindow.Toggle();
        }
    }
    public void ToggleMainUi() => MainWindow.Toggle();

    private unsafe void Test()
    {
        var instance = CharaCard.Instance();
        
    }
}
