using System.Drawing;

namespace RemoveEdgeGlow.Settings.RecordSettings.EFSH
{
    public class EFSHColorKey
    {
        public EFSHColorKey(bool enabled = true, int red = 0, int green = 0, int blue = 0, float time = 1.0f)
        {
            Enabled = enabled;
            Red = red;
            Green = green;
            Blue = blue;
            Time = time;
        }

        public bool Enabled;
        public int Red;
        public int Green;
        public int Blue;
        public float Time;

        private static float GetFloat(float current, float setting, out bool changed)
        {
            changed = !current.Equals(setting);
            return changed ? setting : current;
        }

        public float GetTime(float current, out bool changed)
        {
            return GetFloat(current, Time, out changed);
        }

        public static implicit operator Color(EFSHColorKey obj) => Color.FromArgb(obj.Red, obj.Green, obj.Blue);
    }
}
