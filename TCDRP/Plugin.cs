using System;
using System.Collections;
using System.Collections.Generic;
using BepInEx;
using HarmonyLib;

namespace TCDRP
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance;
        public static void LogDebug(string msg) => Instance.Logger.LogDebug(msg);
        public static void LogInfo(string msg) => Instance.Logger.LogInfo(msg);
        public static void LogError(string msg) => Instance.Logger.LogError(msg);
        public static void LogWarning(string msg) => Instance.Logger.LogWarning(msg);

        private Dictionary<long, DiscordHandler> discordHandlers = new Dictionary<long, DiscordHandler>();
        private Dictionary<long, Discord.Activity> activities = new Dictionary<long, Discord.Activity>();

        public void InitRPC(long clientId)
        {
            try
            {
                var discord = new Discord.Discord(clientId, (ulong)Discord.CreateFlags.NoRequireDiscord);
                discord.SetLogHook(Discord.LogLevel.Error, (level, message) => LogError($"[{level.ToString()}] {message}"));
                var discordHandler = new DiscordHandler(discord, discord.GetActivityManager());
                discordHandlers.Add(clientId, discordHandler);
            }
            catch (Exception) {}
        }

        public void SetActivity(long clientId, Discord.Activity activity)
        {
            if (activities.ContainsKey(clientId))
            {
                activities[clientId] = activity;
            }
            else
            {
                activities.Add(clientId, activity);
            }
        }

        private void Awake()
        {
            if (Instance != null) return;
            
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            Instance = this;
        }

        private void Update()
        {
            if (discordHandlers.Count > 0 && activities.Count > 0)
            {
                foreach (var handler in discordHandlers)
                {
                    if (activities.ContainsKey(handler.Key))
                    {
                        Discord.Activity activity = activities[handler.Key];
                        handler.Value.activityManager.UpdateActivity(activity, (result) =>
                        {
                            if (result != Discord.Result.Ok) LogInfo("Discord: Something went wrong: " + result.ToString());
                        });
                        try
                        {
                            handler.Value.discord.RunCallbacks();
                        }
                        catch (Exception e)
                        {
                            LogError(e.ToString());
                            handler.Value.discord.Dispose();
                            discordHandlers.Remove(handler.Key);
                        }
                    }
                }
            }
        }
    }

    public struct DiscordHandler
    {
        public Discord.Discord discord;
        public Discord.ActivityManager activityManager;
        public DiscordHandler(Discord.Discord discord, Discord.ActivityManager activityManager)
        {
            this.discord = discord;
            this.activityManager = activityManager;
        }
    }
}
