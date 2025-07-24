using CommandSystem;
using HolographicDisplays.Holograms;
using System;

namespace HolographicDisplays.Commands
{
    public class MoveHere : ICommand, IUsageProvider
    {
        public string Command => "movehere";
        public string[] Aliases => new string[] { "mv" };
        public string Description => "Moves the hologram to your position";
        public string[] Usage => new[] { "[name]" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!HDParent.TryGetPlayer(sender, out var player, out response))
                return false;

            if (arguments.Count < 1)
            {
                response = "Using: hd movehere [name]";
                return false;
            }
            if (!Manager.MoveHere(player, arguments.At(0)))
            {
                response = $"Hologram '{arguments.At(0)}' does not exist.";
                return false;
            }
            response = $"Hologram '{arguments.At(0)}' moved here.";
            return true;
        }
    }
}