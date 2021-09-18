using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;

namespace RemoveEdgeGlow.SettingsObjects
{
    public class EffectShaderSettingsOverride : EffectShaderSettings
    {
        public EffectShaderSettingsOverride()
        {
            EffectShader = new();
            EffectShader.SetToNull();
            Enabled = false;
            Priority = Constants.DefaultPriority;
        }
        public EffectShaderSettingsOverride(FormLink<EffectShader> target, bool enabled, int priority, float persistentAlphaRatio, float fullAlphaRatio, ColorKeySettings colorKey1, ColorKeySettings colorKey2, ColorKeySettings colorKey3, ParticleShaderSettings particles, float texScaleU, float texScaleV)
        {
            EffectShader = target;
            Enabled = enabled;
            Priority = priority;
            EdgeEffectPersistentAlphaRatio = persistentAlphaRatio;
            EdgeEffectFullAlphaRatio = fullAlphaRatio;
            ColorKey1 = colorKey1;
            ColorKey2 = colorKey2;
            ColorKey3 = colorKey3;
            ParticleSettings = particles;
            FillTextureScaleU = texScaleU;
            FillTextureScaleV = texScaleV;
        }

        public FormLink<EffectShader> EffectShader;
        public bool Enabled;
        public int Priority;

        public bool ShouldSkip()
        {
            return EffectShader.IsNull || !Enabled || Priority.Equals(Constants.DefaultPriority);
        }

        public bool HasValidPriority()
        {
            return !Priority.Equals(Constants.DefaultPriority);
        }

        public int GetPriority(IEffectShaderGetter efsh)
        {
            return !ShouldSkip() && efsh.FormKey == EffectShader.FormKey
                ? Priority
                : Constants.DefaultPriority;
        }
    }
}
