namespace TCDRP
{
    public static class API
    {
        public static void InitRPC(long clientId)
        {
            // This is simply a shorthand for the function in the Plugin.
            Plugin.Instance.InitRPC(clientId);
        }

        public static void SetActivity(long clientId, string name = "", string state = "", string detail = "", string largeImage = "", string largeText = "", string smallImage = "", string smallText = "", long startTime = 0, long endTime = 0, string partyId = "", int partyCurrentSize = 0, int partyMaxSize = 0, int partyPrivacy = 1, string secretsMatch = "", string secretsJoin = "", string secretsSpectate = "")
        {
            if (partyPrivacy > 1)
                partyPrivacy = 1;
            if (partyPrivacy < 0)
                partyPrivacy = 0;
            // Shorthand for setting simple activities
            var activity = new Discord.Activity {
                Name = name,
                State = state,
                Details = detail,
                Timestamps = {
                    Start = startTime,
                    End = endTime,
                },
                Assets = {
                    LargeImage = largeImage,
                    LargeText = largeText,
                    SmallImage = smallImage,
                    SmallText = smallText,
                },
                Party = {
                    Id = partyId,
                    Size = {
                        CurrentSize = partyCurrentSize,
                        MaxSize = partyMaxSize,
                    },
                    Privacy = (Discord.ActivityPartyPrivacy) partyPrivacy,
                },
                Secrets = {
                    Match = secretsMatch,
                    Join = secretsJoin,
                    Spectate = secretsSpectate,
                }};
            Plugin.Instance.SetActivity(clientId, activity);
        }

        public static void SetActivity(long clientId, Discord.Activity activity)
        {
            Plugin.Instance.SetActivity(clientId, activity);
        }
    }
}