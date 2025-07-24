using CommandSystem;
using HolographicDisplays.Holograms;
using System;
using System.Text;

namespace HolographicDisplays.Commands
{
    public class List : ICommand
    {
        public string Command => "list";
        public string[] Aliases => new string[] { "li" };
        public string Description => "Displays a list of holograms";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!HDParent.TryGetPlayer(sender, out _, out response))
                return false;

            var sb = new StringBuilder("\nHologram List:\n");

            foreach (var holo in Manager.Holograms)
                sb.AppendLine($"{holo.Name}: {holo.RoomType} - {holo.LocalPosition}");

            response = sb.ToString();
            return true;
        }
    }
}