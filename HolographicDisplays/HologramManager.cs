using System.Collections.Generic;
using System.IO;
using System.Linq;
using Exiled.API.Features;
using UnityEngine;
using YamlDotNet.Serialization;

namespace HolographicDisplays
{
    public static class HologramManager
    {
        public static List<Hologram> Holograms { get; } = new();
        private static readonly string FilePath = Path.Combine(Paths.Configs, "holograms.yml");

        public static void Load()
        {
            if (!File.Exists(FilePath))
            {
                Save();
                return;
            }

            var yaml = File.ReadAllText(FilePath);
            var deserializer = new DeserializerBuilder().Build();
            var list = deserializer.Deserialize<List<HoloData>>(yaml);

            if (list == null)
                return;

            foreach (var holo in list)
            {
                var h = new Hologram
                {
                    Name = holo.Name,
                    Content = holo.Text,
                    RoomType = holo.RoomType,
                    LocalPosition = new Vector3(holo.X, holo.Y, holo.Z),
                    SyncDistance = holo.SyncDistance > 0 ? holo.SyncDistance : 32f
                };
                h.Spawn();
                Holograms.Add(h);
            }
        }

        public static void Save()
        {
            var list = Holograms.Select(h => new HoloData
            {
                Name = h.Name,
                Text = h.Content,
                RoomType = h.RoomType,
                X = h.LocalPosition.x,
                Y = h.LocalPosition.y,
                Z = h.LocalPosition.z,
                SyncDistance = h.SyncDistance
            }).ToList();
            var serializer = new SerializerBuilder().Build();
            File.WriteAllText(FilePath, serializer.Serialize(list));
        }

        public static void Create(Player player, string name, string text)
        {
            if (Holograms.Any(h => h.Name == name)) return;
            var room = player.CurrentRoom;
            if (room == null) return;

            Vector3 local = room.Transform.InverseTransformPoint(player.Position);

            var hologram = new Hologram
            {
                Name = name,
                Content = text,
                RoomType = room.Type.ToString(),
                LocalPosition = local
            };
            hologram.Spawn();
            Holograms.Add(hologram);
            Save();
        }

        public static void Delete(string name)
        {
            var holo = Holograms.FirstOrDefault(h => h.Name == name);
            if (holo == null) return;
            holo.Destroy();
            Holograms.Remove(holo);
            Save();
        }

        public static void Edit(string name, string newText)
        {
            var holo = Holograms.FirstOrDefault(h => h.Name == name);
            if (holo == null) return;
            holo.Content = newText;
            holo.Destroy();
            holo.Spawn();
            Save();
        }

        public static void DestroyAll()
        {
            foreach (var h in Holograms)
                h.Destroy();

            Holograms.Clear();
        }

        private class HoloData
        {
            public string Name { get; set; }
            public string Text { get; set; }
            public string RoomType { get; set; }
            public float X { get; set; }
            public float Y { get; set; }
            public float Z { get; set; }
            public float SyncDistance { get; set; } = 32f;
        }
    }
}