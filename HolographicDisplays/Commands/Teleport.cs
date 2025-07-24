using CommandSystem;
using HolographicDisplays.Holograms;
using System;

namespace HolographicDisplays.Commands
{
    public class Teleport : ICommand, IUsageProvider
    {
        public string Command => "teleport";
        public string[] Aliases => new string[] { "tp" };
        public string Description => "Teleports you to the specified hologram";
        public string[] Usage => new[] { "[name]" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!HDParent.TryGetPlayer(sender, out var player, out response))
                return false;

            if (arguments.Count < 1)
            {
                response = "Using: hd teleport [name]";
                return false;
            }
            if (!Manager.TeleportTo(player, arguments.At(0)))
            {
                response = $"Hologram '{arguments.At(0)}' does not exist!";
                return false;
            }
            response = $"Teleported to hologram '{arguments.At(0)}'.";
            return true;
        }
    }
}