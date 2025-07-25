﻿using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Mirror;
using UnityEngine;
using System.Linq;
using Exiled.API.Extensions;
using PlayerRoles;
using AdminToys;
using Placeholder = HolographicDisplays.Placeholders.Placeholders;

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
            var room = Room.List.FirstOrDefault(r => r.Type.ToString() == RoomType);

            if (room == null)
            {
                return DefaultRotation;
            }

            return room.Transform.rotation * DefaultRotation;
        }

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
            Quaternion rot = GetWorldRotation();

            Toy = Object.Instantiate(prefab, pos, rot);
            Toy.TextFormat = Placeholder.Replace(Content);
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

        public void SyncRotationPerPlayer()
        {
            if (Toy == null)
                return;

            Vector3 pos = GetWorldPosition();

            foreach (var player in Player.List)
            {
                if (!player.IsConnected || player.Role.Type == RoleTypeId.Spectator)
                    continue;

                if (Vector3.Distance(player.Position, pos) > SyncDistance)
                    continue;

                Vector3 dir = player.CameraTransform.position - pos;
                if (dir.sqrMagnitude < 0.01f) continue;

                Quaternion rot = Quaternion.LookRotation(dir) * Quaternion.Euler(0, 180f, 0);

                player.SendFakeSyncVar(Toy.netIdentity, typeof(TextToy), nameof(TextToy.NetworkRotation), rot);
            }
        }
    }
}