using MemoryTool;
using System.Collections.Generic;

namespace SurvivalDays.Console
{
    public static class Core
    {
        public static List<Item> GetItems()
        {
            var itemTable = Driver.Read<ulong>(Program.pWorld + Offset.ItemTable);
            var itemCount = Driver.Read<uint>(Program.pWorld + Offset.ItemCount);

            var itemList = new List<Item>();

            for (uint i = 0; i < itemCount; i++)
            {
                var flag = Driver.Read<uint>(itemTable + i * 0x18);

                if (flag > 0)
                {
                    var pItem = Driver.Read<ulong>(itemTable + (i * 0x18) + 0x8);

                    itemList.Add(new Item(pItem));
                }
            }

            return itemList;
        }
    }
}
