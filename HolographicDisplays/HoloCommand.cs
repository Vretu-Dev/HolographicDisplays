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
                response = "Commands: \n" +
                           "<color=yellow>hd create [name] [text]</color> - Creates a hologram at your position\n" +
                           "<color=yellow>hd delete [name]</color> - Deletes the hologram\n" +
                           "<color=yellow>hd edit [name] [new text]</color> - Changes the text of the hologram\n" +
                           "<color=yellow>hd movehere [name]</color> - Moves the hologram to your position\n" +
                           "<color=yellow>hd copy [fromHologram] [toHologram]</color> - Copies the contents of one hologram to another\n" +
                           "<color=yellow>hd teleport [name]</color> - Teleports you to the specified hologram\n" +
                           "<color=yellow>hd list</color> - Displays a list of holograms\n" +
                           "<color=yellow>hd reload</color> - Reloads all holograms\n";
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
                    if (!HologramManager.Delete(arguments.At(1)))
                    {
                        response = $"Hologram '{arguments.At(1)}' does not exist!";
                        return false;
                    }
                    response = $"Hologram deleted {arguments.At(1)}";
                    return true;

                case "edit":
                    if (arguments.Count < 3)
                    {
                        response = "Using: hd edit [name] [new text]";
                        return false;
                    }
                    if (!HologramManager.Edit(arguments.At(1), string.Join(" ", arguments.Skip(2))))
                    {
                        response = $"Hologram '{arguments.At(1)}' does not exist!";
                        return false;
                    }
                    response = $"Hologram edited {arguments.At(1)}";
                    return true;

                case "movehere":
                    if (arguments.Count < 2)
                    {
                        response = "Using: hd movehere [name]";
                        return false;
                    }
                    if (!HologramManager.MoveHere(player, arguments.At(1)))
                    {
                        response = $"Hologram '{arguments.At(1)}' does not exist.";
                        return false;
                    }
                    response = $"Hologram '{arguments.At(1)}' moved here.";
                    return true;

                case "copy":
                    if (arguments.Count < 3)
                    {
                        response = "Using: hd copy [fromHologram] [toHologram]";
                        return false;
                    }
                    if (!HologramManager.CopyContent(arguments.At(1), arguments.At(2)))
                    {
                        response = $"One of the holograms '{arguments.At(1)}' or '{arguments.At(2)}' does not exist!";
                        return false;
                    }
                    response = $"Copied text from '{arguments.At(1)}' to '{arguments.At(2)}'.";
                    return true;

                case "teleport":
                    if (arguments.Count < 2)
                    {
                        response = "Using: hd teleport [name]";
                        return false;
                    }
                    if (!HologramManager.TeleportTo(player, arguments.At(1)))
                    {
                        response = $"Hologram '{arguments.At(1)}' does not exist!";
                        return false;
                    }
                    response = $"Teleportowano do hologramu '{arguments.At(1)}'.";
                    return true;

                case "list":
                    var sb = new StringBuilder("\nHologram List:\n");
                    foreach (var holo in HologramManager.Holograms)
                        sb.AppendLine($"{holo.Name}: {holo.RoomType} - {holo.LocalPosition}");
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