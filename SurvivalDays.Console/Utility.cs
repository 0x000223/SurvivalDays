using MemoryTool;
using System.Numerics;
using System.Text;

namespace SurvivalDays.Console
{
    public static class Utility
    {
        public static Vector2 WorldToScreen(Vector3 position)
        {
            var temp = position - GetViewTranslation();

            var x = Vector3.Dot(temp, GetViewRight());
            var y = Vector3.Dot(temp, GetViewUp());
            var z = Vector3.Dot(temp, GetViewForward());

            return new Vector2()
            {
                X = GetViewportSize().X * (1 + (x / GetProjectionD1().X / z)),
                Y = GetViewportSize().Y * (1 - (y / GetProjectionD2().Y / z))
            };
        }

        private static ulong GetCamera()
        {
            if (Program.pWorld == 0)
            {
                return 0;
            }

            return Driver.Read<ulong>(Program.pWorld + Offset.CameraOn);
        }

        private static Vector3 GetViewTranslation()
        {
            return Driver.Read<Vector3>(GetCamera() + Offset.ViewTranslation);
        }

        private static Vector3 GetViewRight()
        {
            return Driver.Read<Vector3>(GetCamera() + Offset.ViewRight);
        }

        private static Vector3 GetViewUp()
        {
            return Driver.Read<Vector3>(GetCamera() + Offset.ViewUp);
        }

        private static Vector3 GetViewForward()
        {
            return Driver.Read<Vector3>(GetCamera() + Offset.ViewForward);
        }

        private static Vector3 GetViewportSize()
        {
            return Driver.Read<Vector3>(GetCamera() + Offset.ViewportSize);
        }

        private static Vector3 GetProjectionD1()
        {
            return Driver.Read<Vector3>(GetCamera() + Offset.ProjectionD1);
        }

        private static Vector3 GetProjectionD2()
        {
            return Driver.Read<Vector3>(GetCamera() + Offset.ProjectionD2);
        }

        public static string ReadGameString(ulong pString)
        {
            var count = Driver.Read<int>(pString + Offset.StringLength);

            var bytes = Driver.ReadBytes(pString + Offset.StringData, count);

            return Encoding.UTF8.GetString(bytes);
        }
    }
}
