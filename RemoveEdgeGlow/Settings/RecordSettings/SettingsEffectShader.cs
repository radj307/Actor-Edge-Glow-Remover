using Mutagen.Bethesda.Plugins;
using System.Collections.Generic;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.WPF.Reflection.Attributes;
using RemoveEdgeGlow.Settings.RecordSettings.EFSH;
using RemoveEdgeGlow.Settings.GenericUtils;

namespace RemoveEdgeGlow.Settings.RecordSettings
{
    public class SettingsEffectShader : MatchEditorID
    {
        [MaintainOrder]
        [Tooltip("If Enable Whitelist is unchecked, ALL effect shaders except those on the blacklist will be patched.")]
        public bool EnableWhitelist = false;
        public List<FormLink<IEffectShaderGetter>> Whitelist = new()
        { // EFSH Whitelist
        };
        [Tooltip("The blacklist takes priority over the whitelist, if the same effect shader is specified in both it will not be patched.")]
        public List<FormLink<IEffectShaderGetter>> Blacklist = new()
        { // EFSH Blacklist
        };
        [SettingName("Edge Effect - Persistent Alpha Ratio")]
        public float EdgeEffectPersistentAlphaRatio = 0.0f;
        [SettingName("Edge Effect - Full Alpha Ratio")]
        public float EdgeEffectFullAlphaRatio = 0.0f;
        public EFSHColorKey ColorKey1 = new()
        {
            Enabled = false
        };
        public EFSHColorKey ColorKey2 = new()
        {
            Enabled = true
        };
        public EFSHColorKey ColorKey3 = new()
        {
            Enabled = true
        };
        public float ColorScale = 1.0f;
        [SettingName("Particle Animation Settings")]
        public EFSHParticleShader ParticleSettings = new();
        public float FillTextureScaleU = 1.0f;
        public float FillTextureScaleV = 1.0f;

        public bool IsBlacklisted(IEffectShaderGetter efsh)
        {
            bool onBlacklist = Blacklist.Contains(efsh.FormKey); // query blacklist
            if (!EnableWhitelist) // whitelist is disabled
                return onBlacklist; // return true if on blacklist
            bool onWhitelist = Whitelist.Contains(efsh.FormKey) || HasMatch(efsh); // whitelist is enabled, query it
            return !onWhitelist || onBlacklist; // return true if on not whitelist or on blacklist
        }

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
            efsh.EdgeEffectFullAlphaRatio = ApplySettingsToValue(efsh.EdgeEffectFullAlphaRatio, EdgeEffectFullAlphaRatio, ref changed);

            // Color Key 1
            efsh.ColorKey1 = ApplySettingsToValue(efsh.ColorKey1, ColorKey1, ref changed);
            efsh.ColorKey1Time = ColorKey1.GetTime(efsh.ColorKey1Time, out var changedKey1Time);

            // Color Key 2
            efsh.ColorKey2 = ApplySettingsToValue(efsh.ColorKey2, ColorKey2, ref changed);
            efsh.ColorKey2Time = ColorKey2.GetTime(efsh.ColorKey2Time, out var changedKey2Time);

            // Color Key 3
            efsh.ColorKey3 = ApplySettingsToValue(efsh.ColorKey3, ColorKey3, ref changed);
            efsh.ColorKey3Time = ColorKey3.GetTime(efsh.ColorKey3Time, out var changedKey3Time);

            // Particle Shader Animated
            efsh.ParticleAnimatedStartFrame = ApplySettingsToValue(efsh.ParticleAnimatedStartFrame, ParticleSettings.StartFrame, ref changed);
            efsh.ParticleAnimatedStartFrameVariation = ApplySettingsToValue(efsh.ParticleAnimatedStartFrameVariation, ParticleSettings.StartFrameVariation, ref changed);
            efsh.ParticleAnimatedEndFrame = ApplySettingsToValue(efsh.ParticleAnimatedEndFrame, ParticleSettings.EndFrame, ref changed);
            efsh.ParticleAnimatedLoopStartFrame = ApplySettingsToValue(efsh.ParticleAnimatedLoopStartFrame, ParticleSettings.LoopStartFrame, ref changed);
            efsh.ParticleAnimatedLoopStartVariation = ApplySettingsToValue(efsh.ParticleAnimatedLoopStartVariation, ParticleSettings.LoopStartVariation, ref changed);
            efsh.ParticleAnimatedFrameCount = ApplySettingsToValue(efsh.ParticleAnimatedFrameCount, ParticleSettings.FrameCount, ref changed);
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
