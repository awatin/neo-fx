﻿using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Text;

namespace NeoFx.Models
{
    public readonly struct UInt256 : IEquatable<UInt256>, IComparable<UInt256>
    {
        public static readonly UInt256 Zero = new UInt256(0, 0, 0, 0);

        private readonly ulong data1;
        private readonly ulong data2;
        private readonly ulong data3;
        private readonly ulong data4;

        private UInt256(ulong data1, ulong data2, ulong data3, ulong data4)
        {
            this.data1 = data1;
            this.data2 = data2;
            this.data3 = data3;
            this.data4 = data4;
        }

        public UInt256(ReadOnlySpan<byte> span)
        {
            if (!TryReadBytes(span, out this))
            {
                throw new ArgumentException(nameof(span));
            }
        }

        public static bool TryReadBytes(ReadOnlySpan<byte> buffer, out UInt256 result)
        {
            if (buffer.Length >= 32
                && BinaryPrimitives.TryReadUInt64LittleEndian(buffer, out var data1)
                && BinaryPrimitives.TryReadUInt64LittleEndian(buffer.Slice(8), out var data2)
                && BinaryPrimitives.TryReadUInt64LittleEndian(buffer.Slice(16), out var data3)
                && BinaryPrimitives.TryReadUInt64LittleEndian(buffer.Slice(24), out var data4))
            {
                result = new UInt256(data1, data2, data3, data4);
                return true;
            }

            result = default;
            return false;
        }

        // TODO:
        //      public bool TryWriteBytes(Span<byte> buffer)
        //      public static bool TryParse(ReadOnlySpan<char> @string, out UInt256 result)
        //      public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format = default, IFormatProvider provider = null)
        //      public override string ToString()


        public override bool Equals(object obj)
        {
            return (obj is UInt256 value) && (this.Equals(value));
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(data1, data2, data3, data4);
        }

        public bool Equals(in UInt256 other)
        {
            return (this.data1 == other.data1)
                && (this.data2 == other.data2)
                && (this.data3 == other.data3)
                && (this.data4 == other.data4);
        }

        public int CompareTo(in UInt256 other)
        {
            var result = data1.CompareTo(other.data1);
            if (result != 0)
                return result;

            result = data2.CompareTo(other.data2);
            if (result != 0)
                return result;

            result = data3.CompareTo(other.data3);
            if (result != 0)
                return result;

            return data4.CompareTo(other.data4);
        }

        int IComparable<UInt256>.CompareTo(UInt256 other)
        {
            return this.CompareTo(other);
        }

        bool IEquatable<UInt256>.Equals(UInt256 other)
        {
            return this.Equals(other);
        }

        public static bool operator ==(in UInt256 left, in UInt256 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(in UInt256 left, in UInt256 right)
        {
            return !left.Equals(right);
        }

        public static bool operator >(in UInt256 left, in UInt256 right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(in UInt256 left, in UInt256 right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator <(in UInt256 left, in UInt256 right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(in UInt256 left, in UInt256 right)
        {
            return left.CompareTo(right) <= 0;
        }
    }
}
