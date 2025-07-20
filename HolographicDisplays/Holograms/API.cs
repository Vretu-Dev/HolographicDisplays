namespace HolographicDisplays.Holograms
{
    public class HoloData
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string RoomType { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float SyncDistance { get; set; } = 32f;
        public float Yaw { get; set; } = 0f;
    }
}
