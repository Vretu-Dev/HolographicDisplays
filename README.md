![HOlo](https://github.com/user-attachments/assets/65be6663-97d4-4af3-b1de-10e9d68a49e9)<br><br><br>
[![downloads](https://img.shields.io/github/downloads/Vretu-Dev/HolographicDisplays/total?style=for-the-badge&logo=icloud&color=%233A6D8C)](https://github.com/Vretu-Dev/UltimateHUD/releases/latest)
![Latest](https://img.shields.io/github/v/release/Vretu-Dev/HolographicDisplays?style=for-the-badge&label=Latest%20Release&color=%23D91656)


## Downloads:
| Framework | Version    |  Release                                                              |
|:---------:|:----------:|:----------------------------------------------------------------------:|
| Exiled    | ≥ 9.6.0    | [⬇️](https://github.com/Vretu-Dev/HolographicDisplays/releases/latest) |

## Features:

- 🛠️ Create, edit, and delete holograms via RemoteAdmin commands.
- 🧭 Saved per-room with relative rotation/position, seed-independent.
- 🔁 Auto-loads on round start, reloadable without restart.

## Commands:

> [!IMPORTANT]
> **Permission required**: `hd.manage`

```bash
hd create [name] [text]       # Create hologram at your position
hd delete [name]              # Delete hologram
hd edit [name] [new text]     # Edit hologram text
hd movehere [name]            # Move hologram to your position
hd copy [from] [to]           # Copy hologram content
hd teleport [name]            # Teleport to hologram
hd list                       # List all holograms
hd reload                     # Reload all from config
```

<details>
<summary><strong>📌 Example usage (click to expand)</strong></summary>

```bash
hd create Entrance "Welcome to the facility!"
hd edit Entrance "New message"
hd delete Entrance
hd reload
hd list
```
</details>

---

## Placeholders:

You can use the following placeholders in hologram text:

| Placeholder           | Description                          |
|-----------------------|--------------------------------------|
| `{server_name}`       | Name of the server                   |
| `{players}`           | Connected players                    |
| `{max_players}`       | Maximum number of players            |
| `{server_tps}`        | Current TPS (ticks/sec)              |
| `{server_maxtps}`     | Max possible TPS (usually 60)        |
| `{round_time}`        | Round duration (`mm:ss`)             |
| `{time}`              | System time (`HH:mm`)                |
| `{total_escaped}`     | Total escaped players                |
| `{classd_escaped}`    | Escaped Class-D players              |
| `{scientist_escaped}` | Escaped scientists                   |

---

## Config:
```yaml
is_enabled: true
debug: false
# Interval for updating hologram placeholders in seconds.
placeholder_update_interval: 2
# Interval for updating hologram rotation (player-facing) in milliseconds.
rotation_update_interval: 10
# UTC Time Zone | 2 = UTC+2
time_zone: 2
```
