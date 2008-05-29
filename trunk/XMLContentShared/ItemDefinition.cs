using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XMLContentShared
{
    public class ItemDefinition
    {
        /// <summary>
        /// The name of this layer.
        /// </summary>
        protected string name;

        /// <summary>
        /// The name of the texture that this layer uses.
        /// </summary>
        protected string textureAsset;

        /// <summary>
        /// The texture that is used by this layer.
        /// </summary>
        protected Texture2D texture;

        protected Vector2 position;
        protected float rotation;
        protected Vector2 scale;
        protected Vector2 offset;
        protected Vector2 size;

        protected float speed;
        protected float health;
        protected float attackPower;
        protected float attackRadius;
        protected float defenseRadius;
        protected int creditsCost;


        public ItemDefinition()
        {
        }


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

        public virtual void LoadContent(ContentManager content)
        {
        }

        public virtual void UnLoadContent(ContentManager content)
        {
        }

    }
}
