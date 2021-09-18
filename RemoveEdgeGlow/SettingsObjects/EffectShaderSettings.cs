using Mutagen.Bethesda.Skyrim;
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

        public float EdgeEffectPersistentAlphaRatio;
        public float EdgeEffectFullAlphaRatio;
        public ColorKeySettings ColorKey1;
        public ColorKeySettings ColorKey2;
        public ColorKeySettings ColorKey3;
        public float ColorScale;
        public ParticleShaderSettings ParticleSettings;
        public float FillTextureScaleU;
        public float FillTextureScaleV;

        public bool ApplySettingsTo(EffectShader efsh)
        {
            // Edge Effect Alpha Ratios
            efsh.EdgeEffectPersistentAlphaRatio = EdgeEffectPersistentAlphaRatio;
            efsh.EdgeEffectFullAlphaRatio = EdgeEffectFullAlphaRatio;

            // Color Key 1
            efsh.ColorKey1 = ColorKey1;
            efsh.ColorKey1Time = ColorKey1.GetTime(efsh.ColorKey1Time, out var changedKey1Time);

            // Color Key 2
            efsh.ColorKey2 = ColorKey2;
            efsh.ColorKey2Time = ColorKey2.GetTime(efsh.ColorKey2Time, out var changedKey2Time);

            // Color Key 3
            efsh.ColorKey3 = ColorKey3;
            efsh.ColorKey3Time = ColorKey3.GetTime(efsh.ColorKey3Time, out var changedKey3Time);

            // Particle Shader Animated
            efsh.ParticleAnimatedStartFrame = ParticleSettings.StartFrame;
            efsh.ParticleAnimatedStartFrameVariation = ParticleSettings.StartFrameVariation;
            efsh.ParticleAnimatedEndFrame = ParticleSettings.EndFrame;
            efsh.ParticleAnimatedLoopStartFrame = ParticleSettings.LoopStartFrame;
            efsh.ParticleAnimatedLoopStartVariation = ParticleSettings.LoopStartVariation;
            efsh.ParticleAnimatedFrameCount = ParticleSettings.FrameCount;
            efsh.ParticleAnimatedFrameCountVariation = ParticleSettings.FrameCountVariation;

            // Fill Texture Scale
            efsh.FillTextureScaleU = FillTextureScaleU;
            efsh.FillTextureScaleV = FillTextureScaleV;

            return changedKey1Time || changedKey2Time || changedKey3Time; // TODO: Add more conditions here? Totally optional tbh
        }
    }
}
