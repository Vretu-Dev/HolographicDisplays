using CommandSystem;
using HolographicDisplays.Holograms;
using System;
using System.Linq;

namespace HolographicDisplays.Commands
{
    public class Edit : ICommand, IUsageProvider
    {
        public string Command => "edit";
        public string[] Aliases => new string[] { "ed" };
        public string Description => "Changes the text of the hologram";
        public string[] Usage => new[] { "[name] [new text]" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!HDParent.TryGetPlayer(sender, out _, out response))
                return false;

            if (arguments.Count < 2)
            {
                response = "Using: hd edit [name] [new text]";
                return false;
            }
            if (!Manager.Edit(arguments.At(0), string.Join(" ", arguments.Skip(1))))
            {
                response = $"Hologram '{arguments.At(0)}' does not exist!";
                return false;
            }
            response = $"Hologram edited '{arguments.At(0)}'";
            return true;
        }
    }
}
