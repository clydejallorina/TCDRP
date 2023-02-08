using BepInEx;
using HarmonyLib;
using TCDRP;

namespace TestTCMod
{
    [BepInDependency("TCDRP", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public const long clientId = 0; // Insert your own Discord Client ID here
        private void Awake()
        {
            // Plugin startup logic
            TCDRP.API.InitRPC(clientId); // Init client RPC
            Harmony.CreateAndPatchAll(typeof(TestHooks));
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }
    }

    public static class TestHooks
    {
        [HarmonyPatch(typeof(SaveSlotController), nameof(SaveSlotController.Start))]
        [HarmonyPostfix]
        public static void SetStatusOnSaveSlot()
        {
            TCDRP.API.SetActivity(Plugin.clientId, state:"Choosing a save");
        }

        [HarmonyPatch(typeof(HomeController), nameof(HomeController.Start))]
        [HarmonyPostfix]
        public static void SetStatusOnHomeScreen()
        {
            TCDRP.API.SetActivity(Plugin.clientId, state:"At the home screen");
        }

        [HarmonyPatch(typeof(LevelSelectController), nameof(LevelSelectController.Start))]
        [HarmonyPostfix]
        public static void SetStatusOnLevelSelect()
        {
            TCDRP.API.SetActivity(Plugin.clientId, state:"Choosing a level");
        }

        [HarmonyPatch(typeof(GameController), nameof(GameController.Start))]
        [HarmonyPostfix]
        public static void SetStatusOnSong()
        {
            TCDRP.API.SetActivity(Plugin.clientId, state:"Playing the game", detail: $"{GlobalVariables.chosen_track_data.artist} - {GlobalVariables.chosen_track_data.trackname_long}");
        }
    }
}
