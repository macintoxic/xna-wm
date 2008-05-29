using System;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace XMLContentShared
{
    public class LevelContentReader : ContentTypeReader<Level>
    {
        protected override Level Read(ContentReader input, Level existingInstance)
        {
            Level level = new Level();
            
            level.Name = input.ReadString();
            level.MapSize = input.ReadVector2();
            level.LayerList = LoadTileLayerList(input);

            return level;
        }

        private List<TileLayer> LoadTileLayerList(ContentReader input)
        {
            int count = input.ReadInt32();

            List<TileLayer> list = new List<TileLayer>();
            
            for (int index = 0; index < count; ++index)
                list.Add(LoadTileLayer(input));

            return list;
        }

        private TileLayer LoadTileLayer(ContentReader input)
        {
            TileLayer layer = new TileLayer();

            //layer.Color = new Microsoft.Xna.Framework.Graphics.Color(input.ReadVector4());
            layer.Depth = input.ReadSingle();
            layer.Name = input.ReadString();
            layer.TextureAsset = input.ReadString();
            layer.TileList = LoadTileList(input);

            return layer;
        }

        private List<Tile> LoadTileList(ContentReader input)
        {
            int count = input.ReadInt32();

            List<Tile> list = new List<Tile>();

            for (int index = 0; index < count; ++index)
                list.Add(LoadTile(input));

            return list;
        }

        private Tile LoadTile(ContentReader input)
        {
            Tile tile = new Tile();

            tile.Offset = input.ReadVector2();
            tile.Position = input.ReadVector2();
            tile.Rotation = input.ReadSingle();
            tile.Scale = input.ReadVector2();
            tile.Size = input.ReadVector2();

            return tile;
        }
    }
}
