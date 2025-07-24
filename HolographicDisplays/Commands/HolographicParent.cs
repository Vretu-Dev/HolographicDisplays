using CommandSystem;
using Exiled.Permissions.Extensions;
using Exiled.API.Features;
using RemoteAdmin;
using System;

namespace HolographicDisplays.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class HDParent : ParentCommand, IUsageProvider
    {
        public HDParent() => LoadGeneratedCommands();
        public override string Command => "HolographicDisplays";
        public override string[] Aliases => new string[] { "hd" };
        public override string Description => "Holograms Managment";
        public string[] Usage => new[] { "create / delete / edit / movehere / copy / teleport / list / reload" };

        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new Create());
            RegisterCommand(new Delete());
            RegisterCommand(new Edit());
            RegisterCommand(new MoveHere());
            RegisterCommand(new Copy());
            RegisterCommand(new Teleport());
            RegisterCommand(new List());
            RegisterCommand(new Reload());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response =
                "Commands: \n" +
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

        public static bool TryGetPlayer(ICommandSender sender, out Player player, out string response)
        {
            player = null;
            response = null;

            if (sender is not PlayerCommandSender plSender || !Player.TryGet(plSender, out player))
            {
                response = "Command only for player!";
                return false;
            }

            if (!player.CheckPermission("hd.manage"))
            {
                response = "You don't have permision: hd.manage!";
                player = null;
                return false;
            }

            return true;
        }
    }
}