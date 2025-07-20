using Exiled.API.Features.Core.UserSettings;
using Exiled.Events.EventArgs.Player;
using Exiled.Permissions.Extensions;
using HolographicDisplays.Holograms;
using System.Linq;

namespace HolographicDisplays.Extensions
{
    public static class ServerSettings
    {
        public static UserTextInputSetting Hologram { get; private set; }
        public static ButtonSetting Create { get; private set; }
        public static ButtonSetting Edit { get; private set; }
        public static ButtonSetting Delete { get; private set; }
        public static ButtonSetting Teleport { get; private set; }
        public static ButtonSetting MoveHere { get; private set; }
        public static ButtonSetting Copy { get; private set; }
        public static ButtonSetting Reload { get; private set; }

        public static void RegisterSettings()
        {
            Hologram = new UserTextInputSetting(
                id: 9955,
                label: "Holograms Input",
                hintDescription: "Use: (Name | Content), (Name | Name), (Name)"
            );

            Create = new ButtonSetting(
                id: 9956,
                label: "Create (Name|Content)",
                buttonText: "Create",
                hintDescription: "Use: (Name | Content)",
                onChanged: (player, setting) =>
                {
                    var settings = SettingBase.SyncedList[player];
                    var inputInstance = settings.OfType<UserTextInputSetting>().FirstOrDefault(s => s.Id == 9955);

                    string combined = inputInstance?.Text?.Trim() ?? "";
                    var parts = combined.Split(new[] { '|' }, 2);

                    string name = parts.Length > 0 ? parts[0].Trim() : "";
                    string text = parts.Length > 1 ? parts[1].Trim() : "";

                    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(text))
                    {
                        player.SendConsoleMessage("Enter the name and content separated by |", "red");
                        return;
                    }

                    if (!Manager.Create(player, name, text))
                    {
                        player.SendConsoleMessage($"Cannot create hologram '{name}'! It may already exist or room is invalid.", "yellow");
                        return;
                    }

                    player.SendConsoleMessage($"Hologram created '{name}'", "green");
                }
            );

            Edit = new ButtonSetting(
                id: 9957,
                label: "Edit (Name|Content)",
                buttonText: "Edit",
                hintDescription: "Use: (Name | Content)",
                onChanged: (player, setting) =>
                {
                    var settings = SettingBase.SyncedList[player];
                    var inputInstance = settings.OfType<UserTextInputSetting>().FirstOrDefault(s => s.Id == 9955);

                    string combined = inputInstance?.Text?.Trim() ?? "";
                    var parts = combined.Split(new[] { '|' }, 2);

                    string name = parts.Length > 0 ? parts[0].Trim() : "";
                    string text = parts.Length > 1 ? parts[1].Trim() : "";

                    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(text))
                    {
                        player.SendConsoleMessage("Enter the name and content separated by |", "red");
                        return;
                    }

                    if (!Manager.Edit(name, text))
                    {
                        player.SendConsoleMessage($"Hologram '{name}' does not exist!", "red");
                        return;
                    }

                    player.SendConsoleMessage($"The content of the hologram '{name}' has been changed.", "green");
                }
            );

            Delete = new ButtonSetting(
                id: 9958,
                label: "Delete (Name)",
                buttonText: "Delete",
                hintDescription: "Use: (Name)",
                onChanged: (player, setting) =>
                {
                    var settings = SettingBase.SyncedList[player];
                    var inputInstance = settings.OfType<UserTextInputSetting>().FirstOrDefault(s => s.Id == 9955);

                    string combined = inputInstance?.Text?.Trim() ?? "";
                    var parts = combined.Split(new[] { '|' }, 2);
                    string name = parts.Length > 0 ? parts[0].Trim() : "";

                    if (string.IsNullOrWhiteSpace(name))
                    {
                        player.SendConsoleMessage("Enter the name (before |) to delete!", "red");
                        return;
                    }

                    if (!Manager.Delete(name))
                    {
                        player.SendConsoleMessage($"Hologram '{name}' does not exist!", "red");
                        return;
                    }

                    player.SendConsoleMessage($"Hologram '{name}' has been deleted.", "green");
                }
            );

            Teleport = new ButtonSetting(
                id: 9959,
                label: "Teleport (Name)",
                buttonText: "Teleport",
                hintDescription: "Use: (Name)",
                onChanged: (player, setting) =>
                {
                    var settings = SettingBase.SyncedList[player];
                    var inputInstance = settings.OfType<UserTextInputSetting>().FirstOrDefault(s => s.Id == 9955);

                    string combined = inputInstance?.Text?.Trim() ?? "";
                    var parts = combined.Split(new[] { '|' }, 2);
                    string name = parts.Length > 0 ? parts[0].Trim() : "";

                    if (string.IsNullOrWhiteSpace(name))
                    {
                        player.SendConsoleMessage("Enter the hologram name (before |) to teleport!", "red");
                        return;
                    }

                    if (!Manager.TeleportTo(player, name))
                    {
                        player.SendConsoleMessage($"Hologram '{name}' does not exist!", "red");
                        return;
                    }

                    player.SendConsoleMessage($"Teleported to hologram '{name}'.", "green");
                }
            );

            MoveHere = new ButtonSetting(
                id: 9960,
                label: "MoveHere (Name)",
                buttonText: "MoveHere",
                hintDescription: "Use: (Name)",
                onChanged: (player, setting) =>
                {
                    var settings = SettingBase.SyncedList[player];
                    var inputInstance = settings.OfType<UserTextInputSetting>().FirstOrDefault(s => s.Id == 9955);

                    string combined = inputInstance?.Text?.Trim() ?? "";
                    var parts = combined.Split(new[] { '|' }, 2);
                    string name = parts.Length > 0 ? parts[0].Trim() : "";

                    if (string.IsNullOrWhiteSpace(name))
                    {
                        player.SendConsoleMessage("Enter the hologram name (before |) to move here!", "red");
                        return;
                    }

                    if (!Manager.MoveHere(player, name))
                    {
                        player.SendConsoleMessage($"Hologram '{name}' does not exist!", "red");
                        return;
                    }

                    player.SendConsoleMessage($"Hologram '{name}' moved to your position.", "green");
                }
            );

            Copy = new ButtonSetting(
                id: 9961,
                label: "Copy (From|To)",
                buttonText: "Copy",
                hintDescription: "Use: (Name | Name)",
                onChanged: (player, setting) =>
                {
                    var settings = SettingBase.SyncedList[player];
                    var inputInstance = settings.OfType<UserTextInputSetting>().FirstOrDefault(s => s.Id == 9955);

                    string combined = inputInstance?.Text?.Trim() ?? "";
                    var parts = combined.Split(new[] { '|' }, 2);

                    string from = parts.Length > 0 ? parts[0].Trim() : "";
                    string to = parts.Length > 1 ? parts[1].Trim() : "";

                    if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
                    {
                        player.SendConsoleMessage("Enter: HologramFromCopy | HologramToCopy", "red");
                        return;
                    }

                    if (!Manager.CopyContent(from, to))
                    {
                        player.SendConsoleMessage($"One of the holograms '{from}' or '{to}' does not exist!", "red");
                        return;
                    }

                    player.SendConsoleMessage($"Copied content from '{from}' to '{to}'.", "green");
                }
            );

            Reload = new ButtonSetting(
                id: 9962,
                label: "Reload Holograms",
                buttonText: "Reload",
                onChanged: (player, setting) =>
                {
                    Manager.DestroyAll();
                    Manager.Load();
                    player.SendConsoleMessage("All holograms reloaded!", "green");
                }
            );

            Exiled.Events.Handlers.Player.Verified += OnVerified;
        }

        public static void UnregisterSettings()
        {
            SettingBase.Unregister(settings: new[] { Hologram });
            SettingBase.Unregister(settings: new[] { Create });
            SettingBase.Unregister(settings: new[] { Edit });
            SettingBase.Unregister(settings: new[] { Delete });
            SettingBase.Unregister(settings: new[] { Teleport });
            SettingBase.Unregister(settings: new[] { MoveHere });
            SettingBase.Unregister(settings: new[] { Copy });
            SettingBase.Unregister(settings: new[] { Reload });
            Exiled.Events.Handlers.Player.Verified -= OnVerified;
        }

        private static void OnVerified(VerifiedEventArgs ev)
        {
            if (ev.Player.CheckPermission("hd.manage"))
            {
                SettingBase.Register(ev.Player, new SettingBase[]
                {
                    Hologram,
                    Create,
                    Edit,
                    Delete,
                    Teleport,
                    MoveHere,
                    Copy,
                    Reload
                });
            }
        }
    }
}