using CommandSystem;
using HolographicDisplays.Holograms;
using System;

namespace HolographicDisplays.Commands
{
    public class Delete : ICommand, IUsageProvider
    {
        public string Command => "delete";
        public string[] Aliases => new string[] { "del" };
        public string Description => "Deletes the hologram";
        public string[] Usage => new[] { "[name]" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!HDParent.TryGetPlayer(sender, out _, out response))
                return false;

            if (arguments.Count < 1)
            {
                response = "Using: hd delete [name]";
                return false;
            }
            if (!Manager.Delete(arguments.At(0)))
            {
                response = $"Hologram '{arguments.At(0)}' does not exist!";
                return false;
            }
            response = $"Hologram deleted '{arguments.At(0)}'";
            return true;
        }
    }
}