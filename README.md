# Trombone Champ Discord Rich Presence (TCDRP)

The TCDRP is a simple wrapper that allows other mods to access the Discord Rich Presence API through a simple API.

## API Documentation

On your mod's initialization (`Awake` function):

```cs
TCDRP.API.InitRPC(clientId);
```

Setting an activity

```cs
TCDRP.API.SetActivity(Plugin.clientId, state:"Choosing a save");
```

> Note: You can set variables as mentioned in [Discord documentation](https://discord.com/developers/docs/game-sdk/activities).

If you need more help, read through the API implementation in `TCDRP/API.cs` or read through the `TestTCMod` implementation.
