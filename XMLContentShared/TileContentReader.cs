using System;
using Microsoft.Xna.Framework.Content;

namespace XMLContentShared
{

    public class TileContentReader : ContentTypeReader<Tile>
    {
        protected override Tile Read(ContentReader input, Tile existingInstance)
        {
            Tile sprite = new Tile();

            sprite.Position = input.ReadVector2();
            sprite.Rotation = input.ReadSingle();
            sprite.Scale = input.ReadVector2();
            sprite.TextureAsset = input.ReadString();

            sprite.Load(input.ContentManager);

            return sprite;
        }
    }
}
