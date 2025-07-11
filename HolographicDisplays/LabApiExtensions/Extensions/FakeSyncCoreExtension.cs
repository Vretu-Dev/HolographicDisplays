﻿using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using Mirror;
using System;

namespace HolographicDisplays
{
    internal static class FakeSyncCoreExtension
    {
        internal static void SendFakeCore(this Player target, NetworkBehaviour networkBehaviour, Action<NetworkWriterPooled> writeSyncData, Action<NetworkWriterPooled> writeSyncVar)
        {
            if (target.Connection == null)
                return;

            using NetworkWriterPooled writer = NetworkWriterPool.Get();
        
            // gets the dirty mask based on the changed behavior's index
            NetworkBehaviour[] behaviors = networkBehaviour.netIdentity.NetworkBehaviours;
            int index = behaviors == null ? 0 : Array.IndexOf(behaviors, networkBehaviour);
            Compression.CompressVarUInt(writer, 1UL << index);

            // placeholder length
            int headerPosition = writer.Position;
            writer.WriteByte(0);
            int contentPosition = writer.Position;

            // Serialize Object Sync Data.
            writeSyncData.Invoke(writer);

            // Write Object Sync Vars
            writeSyncVar.Invoke(writer);

            // end position safety write
            int endPosition = writer.Position;
            writer.Position = headerPosition;
            int size = endPosition - contentPosition;
            byte safety = (byte)(size & 0xFF);
            writer.WriteByte(safety);
            writer.Position = endPosition;

            // Copy owner to observer
            if (networkBehaviour.syncMode != SyncMode.Observers)
                Logger.Warn("Well snyc mode is not observers, sucks to be you I guess.");

            target.Connection.Send(new EntityStateMessage
            {
                netId = networkBehaviour.netId,
                payload = writer.ToArraySegment(),
            });
        }
    }
}
