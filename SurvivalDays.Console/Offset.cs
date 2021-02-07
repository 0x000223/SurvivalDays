namespace SurvivalDays.Console
{
    public struct Offset
    {
        public const uint World = 0x403EA80;
        public const uint NetworkManager = 0xDEB080;

        // World +
        public const uint CameraOn = 0x1B8;
        public const uint NearEntityTable = 0xE90;
        public const uint NearEntityCount = 0xE98;
        public const uint FarEntityTable = 0xFD8;
        public const uint FarEntityCount = 0xFE0;
        public const uint ItemTable = 0x1F90;
        public const uint ItemCount = 0x1F98;
        public const uint ItemValidCount = 0x1F9C;
        public const uint BulletTable = 0xD48;

        // NetworkManager + 
        public const uint Client = 0x50;
        public const uint PlayerIdentity = 0x158;

        // Client + 
        public const uint Scoreboard = 0x10;
        public const uint ServerName = 0x330;
        public const uint PlayerCount = 0x18;
        public const uint PlayerIdentitySize = 0x158;

        // Identity + 
        public const uint Name = 0xF0;
        public const uint NetworkId = 0x30;

        // Entity +
        public const uint VisualState = 0x130;


        // Item + 
        public const uint ItemType = 0xE0;
        public const uint ModelName = 0x80;
        public const uint DisplayName = 0x4D0;
        public const uint RenderVisualState = 0x130;

        // VisualState + 
        public const uint Position = 0x2C;
        public const uint HeadPosition = 0xF8;

        // Camera
        public const uint ViewRight = 0x8;
        public const uint ViewUp = 0x14;
        public const uint ViewForward = 0x20;
        public const uint ViewTranslation = 0x2C;
        public const uint ViewportSize = 0x58;
        public const uint ProjectionD1 = 0xD0;
        public const uint ProjectionD2 = 0xDC;

        // String
        public const uint StringLength = 0x8;
        public const uint StringData = 0x10;
    }
}
