using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.WPF.Reflection.Attributes;

namespace RemoveEdgeGlow.SettingsObjects
{
    public class EffectShaderSettings
    {
        public EffectShaderSettings()
        {
            EdgeEffectPersistentAlphaRatio = 0.0f;
            EdgeEffectFullAlphaRatio = 0.0f;
            ColorKey1 = new();
            ColorKey2 = new()
            {
                Enabled = true
            };
            ColorKey3 = new()
            {
                Enabled = true
            };
            ColorScale = 1.0f;
            ParticleSettings = new();
            FillTextureScaleU = 1.0f;
            FillTextureScaleV = 1.0f;
        }

        [SettingName("Edge Effect - Persistent Alpha Ratio")]
        public float EdgeEffectPersistentAlphaRatio;
        [SettingName("Edge Effect - Full Alpha Ratio")]
        public float EdgeEffectFullAlphaRatio;
        public ColorKeySettings ColorKey1;
        public ColorKeySettings ColorKey2;
        public ColorKeySettings ColorKey3;
        public float ColorScale;
        [SettingName("Particle Animation Settings")]
        public ParticleShaderSettings ParticleSettings;
        public float FillTextureScaleU;
        public float FillTextureScaleV;

        // DO NOT PASS NULL VALUES!
        private static T ApplySettingsToValue<T>(T value, T setting, ref int changed)
        {
            var changedThis = !value!.Equals(setting!);
            changed += changedThis ? 1 : 0;
            return changedThis ? setting : value;
        }

        public int ApplySettingsTo(ref EffectShader efsh)
        {
            var changed = 0;
            // Edge Effect Alpha Ratios
            efsh.EdgeEffectPersistentAlphaRatio = ApplySettingsToValue(efsh.EdgeEffectPersistentAlphaRatio, EdgeEffectPersistentAlphaRatio, ref changed);
            efsh.EdgeEffectFullAlphaRatio       = ApplySettingsToValue(efsh.EdgeEffectFullAlphaRatio, EdgeEffectFullAlphaRatio, ref changed);

            // Color Key 1
            efsh.ColorKey1     = ApplySettingsToValue(efsh.ColorKey1, ColorKey1, ref changed);
            efsh.ColorKey1Time = ColorKey1.GetTime(efsh.ColorKey1Time, out var changedKey1Time);

            // Color Key 2
            efsh.ColorKey2     = ApplySettingsToValue(efsh.ColorKey2, ColorKey2, ref changed);
            efsh.ColorKey2Time = ColorKey2.GetTime(efsh.ColorKey2Time, out var changedKey2Time);

            // Color Key 3
            efsh.ColorKey3     = ApplySettingsToValue(efsh.ColorKey3, ColorKey3, ref changed);
            efsh.ColorKey3Time = ColorKey3.GetTime(efsh.ColorKey3Time, out var changedKey3Time);

            // Particle Shader Animated
            efsh.ParticleAnimatedStartFrame          = ApplySettingsToValue(efsh.ParticleAnimatedStartFrame, ParticleSettings.StartFrame, ref changed);
            efsh.ParticleAnimatedStartFrameVariation = ApplySettingsToValue(efsh.ParticleAnimatedStartFrameVariation, ParticleSettings.StartFrameVariation, ref changed);
            efsh.ParticleAnimatedEndFrame            = ApplySettingsToValue(efsh.ParticleAnimatedEndFrame, ParticleSettings.EndFrame, ref changed);
            efsh.ParticleAnimatedLoopStartFrame      = ApplySettingsToValue(efsh.ParticleAnimatedLoopStartFrame, ParticleSettings.LoopStartFrame, ref changed);
            efsh.ParticleAnimatedLoopStartVariation  = ApplySettingsToValue(efsh.ParticleAnimatedLoopStartVariation, ParticleSettings.LoopStartVariation, ref changed);
            efsh.ParticleAnimatedFrameCount          = ApplySettingsToValue(efsh.ParticleAnimatedFrameCount, ParticleSettings.FrameCount, ref changed);
            efsh.ParticleAnimatedFrameCountVariation = ApplySettingsToValue(efsh.ParticleAnimatedFrameCountVariation, ParticleSettings.FrameCountVariation, ref changed);

            // Fill Texture Scale
            efsh.FillTextureScaleU = ApplySettingsToValue(efsh.FillTextureScaleU, FillTextureScaleU, ref changed);
            efsh.FillTextureScaleV = ApplySettingsToValue(efsh.FillTextureScaleV, FillTextureScaleV, ref changed);

            changed += changedKey1Time ? 1 : 0;
            changed += changedKey2Time ? 1 : 0;
            changed += changedKey3Time ? 1 : 0;

            return changed;
        }
    }
}
