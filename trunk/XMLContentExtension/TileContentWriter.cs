using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using XMLContentShared;

namespace XMLContentExtension
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// TODO: change the ContentProcessor attribute to specify the correct
    /// display name for this processor.
    /// </summary>
    [ContentTypeWriter]
    public class TileContentWriter : ContentTypeWriter<Tile>
    {
        protected override void Write(ContentWriter output, Tile value)
        {
            output.Write(value.Position);
            output.Write(value.Rotation);
            output.Write(value.Scale);
            output.Write(value.TextureAsset);
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(TileContentReader).AssemblyQualifiedName;
        }
    }
}