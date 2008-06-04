using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Net;
using XMLContentShared;

namespace WM.Units
{
    public class Vehicle : HumanOid
    {
        public Vehicle()
        {
        }

        public Vehicle(UnitItem unitDefinition, MatchInfo.MatchInfo matchInfo)
            : base(unitDefinition.Name, unitDefinition.Position, unitDefinition.Rotation, unitDefinition.Scale, unitDefinition.AttackRadius, unitDefinition.Speed, unitDefinition.TextureAsset, unitDefinition.Offset, unitDefinition.Size, matchInfo, unitDefinition.AttackPower)
        {
            AttackPower = unitDefinition.AttackPower;
        }

        public Vehicle(string name, Vector2 position, float rotation, Vector2 scale, float targetRadius, float speed, string textureAsset, Vector2 Offset, Vector2 size, MatchInfo.MatchInfo matchInfo, float attackPower)
            : base(name, position, rotation, scale, targetRadius, speed, textureAsset, Offset, size, matchInfo, attackPower)
        {
            AttackPower = attackPower;
        }


        public override void UpdateNetworkReader(PacketReader reader)
        {
            base.UpdateNetworkReader(reader);
        }

        public override void UpdateNetworkWriter(PacketWriter writer)
        {
            base.UpdateNetworkWriter(writer);
        }

        public override void TakeHit(float damageAmount)
        {
            // todo ..import Health variable from ItemDefinition/UnitItem/BuildingItem (XML) so we can substract
            Health -= damageAmount;
            if (Health <= 0)
            {
                // todo ..Destroy UntiBase from level

                // todo check object type, humanoid or building.

                // Remove it from the lists
                MatchInfo.GameInfo.MyPlayer.RemoveVehicleUnit(this);
            }
        }
    }
}
