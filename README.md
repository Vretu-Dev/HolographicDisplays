# HolographicDisplays

## Features

- Create, edit, and delete holograms in-game via RemoteAdmin commands.
- Holograms are saved per-room, with position and rotation relative to the room, so they work regardless of seed/layout.
- Holograms are automatically loaded on round start and can be refreshed without restarting the server.

## Commands
Permission required: `hd.manage`.

```
hd create [name] [text]       - Creates a hologram at your position, visible in your current room.
hd delete [name]              - Deletes the hologram with the given name.
hd edit [name] [new text]     - Changes the text of the hologram.
hd movehere [name]            - Moves the hologram to your position.
hd copy [from] [to]           - Copies the contents of one hologram to another.
hd teleport [name]            - Teleports you to the specified hologram.
hd list                       - Lists all holograms with their names, contents, and positions.
hd reload                     - Reloads all holograms from the config file.
```

Example usage:
```
hd create Entrance "Welcome to the facility!"
hd edit Entrance "New message"
hd delete Entrance
hd reload
hd list
```