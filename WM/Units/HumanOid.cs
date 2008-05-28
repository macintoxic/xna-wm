using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Net;

namespace WM.Units
{
    public class HumanOid : UnitBase
    {
        public HumanOid()
            : base(new Vector2(0, 0), 0, new Vector2(0, 0), 0, 0, "", new Vector2(0, 0), new Vector2(0, 0))
        {
        }

        public HumanOid(Vector2 position, float rotation, Vector2 scale, float targetRadius, float speed, string textureAsset, Vector2 offset, Vector2 size)
            : base(position, rotation, scale, targetRadius, speed, textureAsset, offset, size)
        {   
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch batch, float time)
        {
            // Draw a spinning cat sprite, looking it up from the sprite sheet by name.
            batch.Draw(spriteSheetUnit.Texture, Position,
                             spriteSheetUnit.SourceRectangle("cat"), Color.White,
                             time, new Vector2(50, 50), Scale.X, SpriteEffects.None, 0);

            // Draw an animating glow effect, by rapidly cycling
            // through 7 slightly different sprite images.
            const int animationFramesPerSecond = 20;
            const int animationFrameCount = 7;

            // Look up the index of the first glow sprite.
            int glowIndex = spriteSheetUnit.GetIndex("glow1");

            // Modify the index to select the current frame of the animation.
            glowIndex += (int)(time * animationFramesPerSecond) % animationFrameCount;

            // Draw the current glow sprite.
            batch.Draw(spriteSheetUnit.Texture, new Rectangle(100, 150, 200, 200),
                             spriteSheetUnit.SourceRectangle(glowIndex), Color.White);

            base.Draw(batch, time);
        }

        public void Move(GameTime gameTime)
        {                    
        }

        public void Attack(GameTime gameTime, UnitBase OtherTarget)
        {
        }

        public UnitBase FindTargetWithinRadius()
        {
            return null;
        }

        public override void UpdateNetworkReader(PacketReader reader)
        {
            base.UpdateNetworkReader(reader);
        }

        public override void UpdateNetworkWriter(PacketWriter writer)
        {
            base.UpdateNetworkWriter(writer);
        }
    }
}
