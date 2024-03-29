#region File Description
//-----------------------------------------------------------------------------
// SpriteSheetReader.cs
//
// Microsoft Game Technology Group
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework.Content;
#endregion

namespace SpriteSheetRuntime
{
    /// <summary>
    /// Content pipeline support class for reading sprite sheet data from XNB format.
    /// </summary>
    public class SpriteSheetReader : ContentTypeReader<SpriteSheetBase>
    {
        /// <summary>
        /// Loads sprite sheet data from an XNB file.
        /// </summary>
        protected override SpriteSheetBase Read(ContentReader input,
                                            SpriteSheetBase existingInstance)
        {
            return new SpriteSheetBase(input);
        }
    }
}
