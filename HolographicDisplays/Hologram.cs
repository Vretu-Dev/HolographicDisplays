using UnityEngine;
using System.Linq;
using PlayerRoles;
using LabApi.Features.Wrappers;

namespace HolographicDisplays
{
    public class Hologram
    {
        public string Name;
        public string Content;
        public string RoomType;
        public Vector3 LocalPosition;
        public float SyncDistance = 32f;
        public Quaternion DefaultRotation = Quaternion.identity;
        public TextToy Toy { get; private set; }

        public Quaternion GetWorldRotation()
        {
            var room = Room.List.FirstOrDefault(r => r.Name.ToString() == RoomType);

            if (room == null)
            {
                return DefaultRotation;
            }

            return room.Transform.rotation * DefaultRotation;
        }

        public Vector3 GetWorldPosition()
        {
            var room = Room.List.FirstOrDefault(r => r.Name.ToString() == RoomType);

            if (room == null)
            {
                return LocalPosition;
            }

            return room.Transform.TransformPoint(LocalPosition);
        }

        public void Spawn()
        {
            Destroy();

            Vector3 pos = GetWorldPosition();
            Quaternion rot = GetWorldRotation();

            Toy = TextToy.Create(pos, rot, null, true);
            Toy.TextFormat = Placeholders.Replace(Content);
        }

        public void Destroy()
        {
            if (Toy != null)
            {
                Toy.Destroy();
                Toy = null;
            }
        }

        public void SyncRotationPerPlayer()
        {
            if (Toy == null)
                return;

            Vector3 pos = GetWorldPosition();

            foreach (var player in Player.List)
            {
                if (player.Role == RoleTypeId.Spectator)
                    continue;

                if (Vector3.Distance(player.Position, pos) > SyncDistance)
                    continue;

                Vector3 dir = player.Camera.position - pos;
                if (dir.sqrMagnitude < 0.01f) continue;

                Quaternion rot = Quaternion.LookRotation(dir) * Quaternion.Euler(0f, 180f, 0f);

                player.SendFakeSyncVar(Toy.Base, 2, rot);
            }
        }
    }
}