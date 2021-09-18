using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.WPF.Reflection.Attributes;

namespace RemoveEdgeGlow.SettingsObjects
{
    [ObjectNameMember(nameof(EffectShader))]
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

        [Tooltip("The effect formlink to apply these settings to.")]
        public FormLink<EffectShader> EffectShader;
        [Tooltip("Disabling an override will prevent it from being applied without deleting it from the list. Default settings will take priority.")]
        public bool Enabled;
        [Tooltip("The override with the highest priority wins if multiple overrides could be applied to the same effect.")]
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
