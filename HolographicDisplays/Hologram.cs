using AdminToys;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Mirror;
using UnityEngine;
using System.Linq;

namespace HolographicDisplays
{
    public class Hologram
    {
        public string Name;
        public string Content;
        public string RoomType;
        public Vector3 LocalPosition;
        public TextToy Toy { get; private set; }

        public Vector3 GetWorldPosition()
        {
            var room = Room.List.FirstOrDefault(r => r.Type.ToString() == RoomType);

            if (room == null)
            {
                return LocalPosition;
            }

            return room.Transform.TransformPoint(LocalPosition);
        }

        public void Spawn(Vector2? size = null)
        {
            Destroy();

            var prefab = Text.Prefab;
            Vector3 pos = GetWorldPosition();

            Toy = Object.Instantiate(prefab, pos, Quaternion.identity);
            Toy.TextFormat = Content;
            Toy.Scale = size ?? new Vector2(0.15f, 0.05f);

            NetworkServer.Spawn(Toy.gameObject);
        }

        public void Destroy()
        {
            if (Toy != null)
            {
                NetworkServer.Destroy(Toy.gameObject);
                Toy = null;
            }
        }

        public void SyncRotationToNearest()
        {
            if (Toy == null)
                return;

            Vector3 pos = GetWorldPosition();

            Player nearest = null;
            float minDist = float.MaxValue;

            foreach (var player in Player.List)
            {
                float dist = Vector3.Distance(pos, player.Position);
                if (dist < minDist && dist <= 32f)
                {
                    minDist = dist;
                    nearest = player;
                }
            }

            if (nearest == null)
                return;

            Vector3 dir = nearest.CameraTransform.position - pos;
            if (dir.sqrMagnitude < 0.01f) return;

            Quaternion rot = Quaternion.LookRotation(dir);
            Toy.transform.rotation = rot * Quaternion.Euler(0, 180f, 0);
        }
    }
}