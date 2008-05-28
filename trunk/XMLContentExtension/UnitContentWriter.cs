using System;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using XMLContentShared;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace XMLContentExtension
{
    [ContentTypeWriter]
    public class UnitContentWriter : ContentTypeWriter<Units>
    {
        protected override void Write(ContentWriter output, Units value)
        {
            output.Write(value.HumanOidList.Count);
            for (int i = 0; i < value.HumanOidList.Count; i++)
            {
                output.Write(value.HumanOidList[i].AttackPower);
                output.Write(value.HumanOidList[i].AttackRadius);
                output.Write(value.HumanOidList[i].DefenseRadius);
                output.Write(value.HumanOidList[i].Health);
                output.Write(value.HumanOidList[i].Name);
                output.Write(value.HumanOidList[i].Offset);
                output.Write(value.HumanOidList[i].Position);
                output.Write(value.HumanOidList[i].Rotation);
                output.Write(value.HumanOidList[i].Scale);
                output.Write(value.HumanOidList[i].Size);
                output.Write(value.HumanOidList[i].Speed);
                //output.Write(value.HumanOidList[i].Texture);
                output.Write(value.HumanOidList[i].TextureAsset);
                output.Write(value.HumanOidList[i].CreditsCost);
            }

            output.Write(value.VehicleList.Count);
            for (int i = 0; i < value.VehicleList.Count; i++)
            {
                output.Write(value.VehicleList[i].AttackPower);
                output.Write(value.VehicleList[i].AttackRadius);
                output.Write(value.VehicleList[i].DefenseRadius);
                output.Write(value.VehicleList[i].Health);
                output.Write(value.VehicleList[i].Name);
                output.Write(value.VehicleList[i].Offset);
                output.Write(value.VehicleList[i].Position);
                output.Write(value.VehicleList[i].Rotation);
                output.Write(value.VehicleList[i].Scale);
                output.Write(value.VehicleList[i].Size);
                output.Write(value.VehicleList[i].Speed);
                //output.Write(value.VehicleList[i].Texture);
                output.Write(value.VehicleList[i].TextureAsset);
                output.Write(value.VehicleList[i].CreditsCost);
            }

            output.Write(value.BuildingList.Count);
            for (int i = 0; i < value.BuildingList.Count; i++)
            {
                output.Write(value.BuildingList[i].AttackPower);
                output.Write(value.BuildingList[i].AttackRadius);
                output.Write(value.BuildingList[i].DefenseRadius);
                output.Write(value.BuildingList[i].Health);
                output.Write(value.BuildingList[i].Name);
                output.Write(value.BuildingList[i].Offset);
                output.Write(value.BuildingList[i].Position);
                output.Write(value.BuildingList[i].Rotation);
                output.Write(value.BuildingList[i].Scale);
                output.Write(value.BuildingList[i].Size);
                output.Write(value.BuildingList[i].Speed);
                //output.Write(value.BuildingList[i].Texture);
                output.Write(value.BuildingList[i].TextureAsset);
                output.Write(value.BuildingList[i].CreditsCost);
                output.Write(value.BuildingList[i].ProductionUnit);                
            }

        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return typeof(UnitContentReader).AssemblyQualifiedName;
        }
    }
}
