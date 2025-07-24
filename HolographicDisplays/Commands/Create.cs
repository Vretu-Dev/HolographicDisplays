using CommandSystem;
using HolographicDisplays.Holograms;
using System;
using System.Linq;

namespace HolographicDisplays.Commands
{
    public class Create : ICommand, IUsageProvider
    {
        public string Command => "create";
        public string[] Aliases => new string[] { "cr" };
        public string Description => "Creates a hologram at your position";
        public string[] Usage => new[] { "[name] [text]" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!HDParent.TryGetPlayer(sender, out var player, out response))
                return false;

            if (arguments.Count < 2)
            {
                response = "Using: hd create [name] [text]";
                return false;
            }
            if (!Manager.Create(player, arguments.At(0), string.Join(" ", arguments.Skip(1))))
            {
                response = $"Cannot create hologram '{arguments.At(0)}'! It may already exist or room is invalid.";
                return false;
            }
            response = $"Hologram created '{arguments.At(0)}'";
            return true;
        }
    }
}
