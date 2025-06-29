using CommandSystem;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;
using System.Text;
using System.Linq;
using RemoteAdmin;
using System;

namespace HolographicDisplays.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class HoloCommand : ICommand
    {
        public string Command => "HolographicDisplays";
        public string[] Aliases => new string[] { "hd" };
        public string Description => "Holograms Managment";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender is not PlayerCommandSender plSender || !Player.TryGet(plSender, out var player))
            {
                response = "Command only for player!";
                return false;
            }

            if (!player.CheckPermission("hd.manage"))
            {
                response = "You don't have permision: hd.manage!";
                return false;
            }

            if (arguments.Count == 0)
            {
                response = "Using: create [name] [text] | delete [name] | edit [name] [new text] | list";
                return false;
            }

            switch (arguments.At(0).ToLower())
            {
                case "create":
                    if (arguments.Count < 3)
                    {
                        response = "Using: hd create [name] [text]";
                        return false;
                    }
                    HologramManager.Create(player, arguments.At(1), string.Join(" ", arguments.Skip(2)));
                    response = $"Hologram created {arguments.At(1)}";
                    return true;

                case "delete":
                    if (arguments.Count < 2)
                    {
                        response = "Using: hd delete [name]";
                        return false;
                    }
                    HologramManager.Delete(arguments.At(1));
                    response = $"Hologram deleted {arguments.At(1)}";
                    return true;

                case "edit":
                    if (arguments.Count < 3)
                    {
                        response = "Using: hd edit [name] [new text]";
                        return false;
                    }
                    HologramManager.Edit(arguments.At(1), string.Join(" ", arguments.Skip(2)));
                    response = $"Hologram edited {arguments.At(1)}";
                    return true;

                case "list":
                    var sb = new StringBuilder("Hologram List:\n");
                    foreach (var holo in HologramManager.Holograms)
                        sb.AppendLine($"{holo.Name}: {holo.Content} [{holo.LocalPosition}]");
                    response = sb.ToString();
                    return true;

                case "reload":
                    HologramManager.DestroyAll();
                    HologramManager.Load();
                    response = "Holograms reloaded";
                    return true;

                default:
                    response = "Uknown subcommand.";
                    return false;
            }
        }
    }
}