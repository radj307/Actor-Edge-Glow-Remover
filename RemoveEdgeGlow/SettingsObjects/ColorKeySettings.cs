using System.Drawing;

namespace RemoveEdgeGlow.SettingsObjects
{
    public class ColorKeySettings
    {
        public ColorKeySettings(int red = 0, int green = 0, int blue = 0, float scale = 1.0f, float time = 1.0f)
        {
            Enabled = true;
            Red = red;
            Green = green;
            Blue = blue;
            Scale = scale;
            Time = time;
        }

        public bool Enabled = false;
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

        public static implicit operator Color(ColorKeySettings obj) => Color.FromArgb(obj.Red, obj.Green, obj.Blue);
    }
}
