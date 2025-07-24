using CommandSystem;
using HolographicDisplays.Holograms;
using System;

namespace HolographicDisplays.Commands
{
    public class Copy : ICommand, IUsageProvider
    {
        public string Command => "copy";
        public string[] Aliases => new string[] { "cp" };
        public string Description => "Copies the contents of one hologram to another";
        public string[] Usage => new[] { "[fromHologram] [toHologram]" };

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!HDParent.TryGetPlayer(sender, out _, out response))
                return false;

            if (arguments.Count < 2)
            {
                response = "Using: hd copy [fromHologram] [toHologram]";
                return false;
            }
            if (!Manager.CopyContent(arguments.At(0), arguments.At(1)))
            {
                response = $"One of the holograms '{arguments.At(0)}' or '{arguments.At(1)}' does not exist!";
                return false;
            }
            response = $"Copied text from '{arguments.At(0)}' to '{arguments.At(1)}'.";
            return true;
        }
    }
}