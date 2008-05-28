using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XMLContentShared
{
    public class UnitItem
    {
        /// <summary>
        /// The name of this layer.
        /// </summary>
        private string name;

        /// <summary>
        /// The name of the texture that this layer uses.
        /// </summary>
        private string textureAsset;

        /// <summary>
        /// The texture that is used by this layer.
        /// </summary>
        private Texture2D texture;

        private Vector2 position;
        private float rotation;
        private Vector2 scale;
        private Vector2 offset;
        private Vector2 size;

        private float speed;
        private float health;
        private float attackPower;
        private float attackRadius;
        private float defenseRadius;
        private int creditsCost;

        /// <summary>
        /// Gets or sets the name of this tile layer.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets the texture that is used by this tile layer.
        /// </summary>
        public string TextureAsset
        {
            get { return textureAsset; }
            set { textureAsset = value; }
        }

        [ContentSerializerIgnore]
        public Texture2D Texture
        {
            get { return texture; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public Vector2 Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public Vector2 Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        public Vector2 Size
        {
            get { return size; }
            set { size = value; }
        }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public float Health
        {
            get { return health; }
            set { health = value; }
        }

        public float AttackPower
        {
            get { return attackPower; }
            set { attackPower = value; }
        }

        public float AttackRadius
        {
            get { return attackRadius; }
            set { attackRadius = value; }
        }

        public float DefenseRadius
        {
            get { return defenseRadius; }
            set { defenseRadius = value; }
        }

        public int CreditsCost
        {
            get { return creditsCost; }
            set { creditsCost = value; }
        }
       
        public void Load(ContentManager content)
        {
            texture = content.Load<Texture2D>(textureAsset);
        }

        public void LoadContent(ContentManager content)
        {
            Load(content);            
        }

        public void UnLoadContent(ContentManager content)
        {
            
        }
    }
}
