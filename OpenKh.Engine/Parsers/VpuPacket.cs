﻿using OpenKh.Common;
using System.IO;
using System.Linq;
using Xe.BinaryMapper;

namespace OpenKh.Engine.Parsers
{
    public class VpuPacket
    {
        public enum VertexFunction
        {
            None = 0x00,
            Stock = 0x10,
            DrawTriangle = 0x20,
            DrawTriangleInverse = 0x30
        }

        public class VpuHeader
        {
            [Data] public int Type { get; set; }
            [Data] public int Unknown04 { get; set; }
            [Data] public int Unknown08 { get; set; }
            [Data] public int Unknown0c { get; set; }
            [Data] public int IndexCount { get; set; }
            [Data] public int IndexLocation { get; set; }
            [Data] public int UnkBoxLocation { get; set; }
            [Data] public int Unknown1cLocation { get; set; }
            [Data] public int ColorCount { get; set; }
            [Data] public int ColorLocation { get; set; }
            [Data] public int Unknown28 { get; set; }
            [Data] public int Unknown2c { get; set; }
            [Data] public int VertexCount { get; set; }
            [Data] public int VertexLocation { get; set; }
            [Data] public int Unknown38 { get; set; }
            [Data] public int UnkBoxCount { get; set; }
        }

        public class VertexIndex
        {
            [Data] public int U { get; set; }
            [Data] public int V { get; set; }
            [Data] public int Index { get; set; }
            [Data] public VertexFunction Function { get; set; }

            public override string ToString() =>
                $"{U / 4096.0f:F}, {V / 4096.0f:F}, {Index:X}, {Function}";
        }

        public class VertexColor
        {
            [Data] public int R { get; set; }
            [Data] public int G { get; set; }
            [Data] public int B { get; set; }
            [Data] public int A { get; set; }

            public override string ToString() =>
                $"{R:X}, {G:X}, {B:X}, {A:X}";
        }

        public class VertexCoord
        {
            [Data] public float X { get; set; }
            [Data] public float Y { get; set; }
            [Data] public float Z { get; set; }
            [Data] public float W { get; set; }

            public override string ToString() =>
                $"{X:F}, {Y:F}, {Z:F}, {W:F}";
        }

        public VertexIndex[] Indices { get; }
        public VertexColor[] Colors { get; }
        public VertexCoord[] Vertices { get; }
        public int[] alalni { get; }

        private VpuPacket(Stream stream)
        {
            var vpu = BinaryMapping.ReadObject<VpuHeader>(stream);

            Indices = Read<VertexIndex>(stream, vpu.IndexLocation, vpu.IndexCount);
            Colors = Read<VertexColor>(stream, vpu.ColorLocation, vpu.ColorCount);
            Vertices = Read<VertexCoord>(stream, vpu.VertexLocation, vpu.VertexCount);
            alalni = Enumerable.Range(0, Indices.Length).ToArray();
        }

        private static T[] Read<T>(Stream stream, int offset, int count)
            where T : class
        {
            stream.SetPosition(offset * 0x10);
            return Enumerable.Range(0, count)
                .Select(x => BinaryMapping.ReadObject<T>(stream))
                .ToArray();
        }

        public static VpuPacket Read(Stream stream) =>
            new VpuPacket(stream);

        public static VpuHeader Header(Stream stream) =>
            BinaryMapping.ReadObject<VpuHeader>(stream);
    }
}
