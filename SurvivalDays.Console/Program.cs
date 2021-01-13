using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DrawingFramework;
using MemoryTool;


namespace SurvivalDays.Console
{
    class Program
    {
        public static ulong pBase { get; set; }

        public static ulong pWorld { get; set; }

        public static ulong pNetworkClient { get; set; }

        static void Main(string[] args)
        {
            System.Console.Title = "Survival Days";

            var processId = User.GetProcessId("Dayz_x64");

            var driver = new Driver(processId);

            var renderer = new Render(1080, 1920);

            pBase = Driver.GetModuleAddress("Dayz_x64.exe");

            pWorld = Driver.Read<ulong>(pBase + Offset.World);

            pNetworkClient = Driver.Read<ulong>(pBase + Offset.NetworkManager + Offset.Client);

            var pScoreboard = Driver.Read<ulong>(pNetworkClient + Offset.Scoreboard);

            var playerCount = Driver.Read<uint>(pNetworkClient + Offset.PlayerCount);

            var pServerName = Driver.Read<ulong>(pNetworkClient + Offset.ServerName);

            var serverName = Utility.ReadGameString(pServerName);

            System.Console.WriteLine($"Dayz_x64 Base: 0x{pBase:X}");

            System.Console.WriteLine($"Server Name: {serverName}");

            var items = Core.GetItems();

            renderer.Draw(() =>
            {
                try
                {
                    var nearEntities = Driver.Read<uint>(pWorld + Offset.NearEntityCount);

                    var farEntites = Driver.Read<uint>(pWorld + Offset.FarEntityCount);

                    renderer.DrawText(200, 185, $"{serverName}", Render.Brushes.Red);

                    renderer.DrawText(200, 200, $"Player Count: {playerCount}", Render.Brushes.Green);

                    renderer.DrawText(200, 215, $"Near Etities: {nearEntities}", Render.Brushes.Green);

                    renderer.DrawText(200, 230, $"Far Entities: {farEntites}", Render.Brushes.Green);

                    foreach (var item in Core.GetItems())
                    {
                        var pos = Utility.WorldToScreen(item.Position);

                        if (pos == Vector2.Zero)
                        {
                            continue;
                        }

                        renderer.DrawText(pos.X, pos.Y, item.Name, Render.Brushes.White);
                    }
                }
                catch { }
            });
        }
    }
}