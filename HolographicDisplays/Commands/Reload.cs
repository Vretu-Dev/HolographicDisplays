using CommandSystem;
using HolographicDisplays.Holograms;
using System;

namespace HolographicDisplays.Commands
{
    public class Reload : ICommand
    {
        public string Command => "reload";
        public string[] Aliases => Array.Empty<string>();
        public string Description => "Reloads all holograms";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!HDParent.TryGetPlayer(sender, out _, out response))
                return false;

            Manager.DestroyAll();
            Manager.Load();
            response = "Holograms reloaded";
            return true;
        }
    }
}
