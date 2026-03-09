using System.Text;
using Dalamud.Hooking;
using Dalamud.Plugin.SelfTest;
using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using FFXIVClientStructs.FFXIV.Client.System.String;
using FFXIVClientStructs.FFXIV.Client.UI.Agent;
using FFXIVClientStructs.FFXIV.Client.UI.Misc;
using InteropGenerator.Runtime;


namespace BilkDebug;

public unsafe class Hooks : IDisposable
{
    private Hook<CharaCard.Delegates.HandleCurrentCharaCardDataPacket> hook = null!;

    public Hooks()
    {
        hook = Hook.HookFromAddress(CharaCard.Addresses.HandleCurrentCharaCardDataPacket.Value,
            new CharaCard.Delegates.HandleCurrentCharaCardDataPacket(Detour));
        hook.Enable();
    }

    private void Detour(CharaCard* test, AgentCharaCard.CharaCardPacket* packet)
    {
        Log.Debug($"HIT! {packet->AccountId}");
        Log.Debug($"{test->TempBannerData.EyeDirectionX}");

        hook.Original(test, packet);
    }

    public void Dispose()
    {
        hook.Dispose();
    }
}