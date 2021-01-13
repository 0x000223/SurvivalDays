using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MemoryTool;

namespace SurvivalDays.Console
{
    public class Item
    {
        public Item(ulong address)
        {
            Address = address;

            Name = GetName();

            Position = GetPosition();
        }

        public ulong Address { get; }

        public string Name { get; }

        public Vector3 Position { get; }

        private string GetName()
        {
            var itemType = Driver.Read<ulong>(Address + Offset.ItemType);

            var pDisplayName = Driver.Read<ulong>(itemType + Offset.DisplayName);

            return Utility.ReadGameString(pDisplayName);
        }

        private Vector3 GetPosition()
        {
            var renderVisualState = Driver.Read<ulong>(Address + Offset.RenderVisualState);

            return Driver.Read<Vector3>(renderVisualState + Offset.Position);
        }
    }
}