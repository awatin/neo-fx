﻿using System;
using System.Collections.Immutable;
using DevHawk.Buffers;
using NeoFx.Models;
using NeoFx.Storage;

namespace NeoFx.P2P.Messages
{
    public readonly struct HeadersPayload : IWritable<HeadersPayload>
    {
        public readonly ImmutableArray<BlockHeader> Headers;

        public int Size => Headers.GetVarSize(h => h.Size);

        public HeadersPayload(ImmutableArray<BlockHeader> headers)
        {
            Headers = headers;
        }

        static bool TryReadHeaderWithZeroTransactions(ref BufferReader<byte> reader, out BlockHeader header)
        {
            if (BlockHeader.TryRead(ref reader, out header)
                && reader.TryReadVarInt(out var txCount)
                && txCount == 0)
            {
                return true;
            }

            header = default;
            return false;
        }

        public static bool TryRead(ref BufferReader<byte> reader, out HeadersPayload payload)
        {
            if (reader.TryReadVarArray<BlockHeader>(TryReadHeaderWithZeroTransactions, out var headers))
            {
                payload = new HeadersPayload(headers);
                return true;
            }

            payload = default;
            return false;
        }

        public void WriteTo(ref BufferWriter<byte> writer)
        {
            writer.WriteVarArray(Headers);
        }
    }
}
