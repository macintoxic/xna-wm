using System;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace XMLContentShared
{
    public class UnitContentReader : ContentTypeReader<Units>
    {
        protected override Units Read(ContentReader input, Units existingInstance)
        {

            Units u = new Units();

            int count = input.ReadInt32();
            for(int i=0; i< count; i++)
            {
                UnitItem unitItem = new UnitItem();
                unitItem.AttackPower = input.ReadSingle();
                unitItem.AttackRadius = input.ReadSingle();
                unitItem.DefenseRadius = input.ReadSingle();
                unitItem.Health = input.ReadSingle();
                unitItem.Name = input.ReadString();
                unitItem.Offset = input.ReadVector2();
                unitItem.Position = input.ReadVector2();
                unitItem.Rotation = input.ReadSingle();
                unitItem.Scale = input.ReadVector2();
                unitItem.Size = input.ReadVector2();
                unitItem.Speed = input.ReadSingle();
                //unitItem.Texture = input.ReadSingle();
                unitItem.TextureAsset = input.ReadString();
                unitItem.CreditsCost = input.ReadInt32();
                u.HumanOidList.Add(unitItem);
            }

            count = input.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                UnitItem unitItem = new UnitItem();
                unitItem.AttackPower = input.ReadSingle();
                unitItem.AttackRadius = input.ReadSingle();
                unitItem.DefenseRadius = input.ReadSingle();
                unitItem.Health = input.ReadSingle();
                unitItem.Name = input.ReadString();
                unitItem.Offset = input.ReadVector2();
                unitItem.Position = input.ReadVector2();
                unitItem.Rotation = input.ReadSingle();
                unitItem.Scale = input.ReadVector2();
                unitItem.Size = input.ReadVector2();
                unitItem.Speed = input.ReadSingle();
                //unitItem.Texture = input.ReadSingle();
                unitItem.TextureAsset = input.ReadString();
                unitItem.CreditsCost = input.ReadInt32();
                u.VehicleList.Add(unitItem);
            }

            count = input.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                BuildingItem unitItem = new BuildingItem();
                unitItem.AttackPower = input.ReadSingle();
                unitItem.AttackRadius = input.ReadSingle();
                unitItem.DefenseRadius = input.ReadSingle();
                unitItem.Health = input.ReadSingle();
                unitItem.Name = input.ReadString();
                unitItem.Offset = input.ReadVector2();
                unitItem.Position = input.ReadVector2();
                unitItem.Rotation = input.ReadSingle();
                unitItem.Scale = input.ReadVector2();
                unitItem.Size = input.ReadVector2();
                unitItem.Speed = input.ReadSingle();
                //unitItem.Texture = input.ReadSingle();
                unitItem.TextureAsset = input.ReadString();
                unitItem.CreditsCost = input.ReadInt32();
                unitItem.ProductionUnit = input.ReadString();
                u.BuildingList.Add(unitItem);
            }
            

            return u;
        }
    }
}
