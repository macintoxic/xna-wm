using System;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using XMLContentShared;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace XMLContentExtension
{
    [ContentTypeWriter]
    public class LevelContentWriter : ContentTypeWriter<Level>
    {
        protected override void Write(ContentWriter output, Level value)
        {
            output.Write(value.Name);
            output.Write(value.MapSize);

            WriteTileLayerList(output, value.LayerList);
        }

        private void WriteTileLayerList(ContentWriter output, List<TileLayer> list)
        {
            output.Write(list.Count);

            for (int index = 0; index < list.Count; ++index)
                WriteTileLayer(output, list[index]);
        }

        private void WriteTileLayer(ContentWriter output, TileLayer layer)
        {
            //output.Write(layer.Color.ToVector4());
            output.Write(layer.Depth);
            output.Write(layer.Name);
            output.Write(layer.TextureAsset);

            WriteTileList(output, layer.TileList);
        }

        private void WriteTileList(ContentWriter output, List<Tile> list)
        {
            output.Write(list.Count);

            for (int index = 0; index < list.Count; ++index)
                WriteTile(output, list[index]);
        }

        private void WriteTile(ContentWriter output, Tile tile)
        {
            output.Write(tile.Offset);
            output.Write(tile.Position);
            output.Write(tile.Rotation);
            output.Write(tile.Scale);
            output.Write(tile.Size);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(LevelContentReader).AssemblyQualifiedName;
        }
    }
}
