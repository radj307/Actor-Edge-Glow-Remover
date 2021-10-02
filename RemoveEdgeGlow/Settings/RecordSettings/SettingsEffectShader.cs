using System.Collections.Generic;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.WPF.Reflection.Attributes;
using RemoveEdgeGlow.Settings.GenericUtils;
using RemoveEdgeGlow.Settings.RecordSettings.EFSH;

namespace RemoveEdgeGlow.Settings.RecordSettings
{
    /// <summary>
    /// Contains all Effect-Shader-Specific Settings.
    /// </summary>
    public class SettingsEffectShader : MatchEditorID
    {
        public SettingsEffectShader() : base(
            whitelist: null,
            blacklist: new() {
                "azura",
                "detect",
                "greybeard",
                "standingstone",
                "puzzle",
                "alduin",
                "miraak",
                "summon",
                "shadowmere",
                "riekling",
                "lurker",
                "apocrypha",
                "icewraith",
                "ghost",
                "ethereal",
                "illusion",
                "muffle",
                "absorb",
                "kyne",
                "animalally",
                "disintegrate",
                "dragon",
                "lightbeam",
                "whirlwind",
                "dlc2expspider",
                "vampire",
            })
        { }
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
        [SettingName("ColorKey1")]
        public EFSHColorKey ColorKey1 = new()
        {
            Enabled = false
        };
        [SettingName("ColorKey2")]
        public EFSHColorKey ColorKey2 = new()
        {
            Enabled = true
        };
        [SettingName("ColorKey3")]
        public EFSHColorKey ColorKey3 = new()
        {
            Enabled = true
        };
        [SettingName("ColorScale")]
        public float ColorScale = 1.0f;
        [SettingName("Particle Animation Settings")]
        public EFSHParticleShader ParticleSettings = new();
        [SettingName("FillTextureScaleU")]
        public float FillTextureScaleU = 1.0f;
        [SettingName("FillTextureScaleV")]
        public float FillTextureScaleV = 1.0f;

        /// <summary>
        /// Check if a given Effect Shader is present on the whitelist, or if the whitelist is disabled, if it is not on the blacklist
        /// Returns true when the given effect shader's formkey IS a valid target for the patcher.
        /// </summary>
        /// <param name="efsh">Effect Shader Getter Interface</param>
        /// <returns>bool</returns>
        public bool IsValidPatchTarget(IEffectShaderGetter efsh)
        {
            if ( efsh.EditorID == null )
                return false;
            bool onBlacklist = Blacklist.Contains(efsh.FormKey) || IsBlacklistedEditorID(efsh.EditorID); // query blacklist
            if ( !EnableWhitelist )
                return !onBlacklist; // return true if not blacklisted
            bool onWhitelist = Whitelist.Contains(efsh.FormKey) || IsWhitelistedEditorID(efsh.EditorID); // whitelist is enabled, query it
            return !onBlacklist && onWhitelist; // return true if not on blacklist or is on whitelist
        }

        /// <summary>
        /// Apply current settings to a given value.
        /// </summary>
        /// <typeparam name="T">Variable Type</typeparam>
        /// <param name="value">The subrecord's current unmodified value.</param>
        /// <param name="setting">The current setting's value.</param>
        /// <param name="changed">Number of changed records.</param>
        /// <returns>T</returns>
        private static T ApplySettingsToValue<T>(T value, T setting, ref int changed)
        {
            bool changedThis = !value!.Equals(setting!);
            changed += changedThis ? 1 : 0;
            return changedThis ? setting : value;
        }

        /// <summary>
        /// Apply current settings to an EffectShader object reference.
        /// Returns the number of modified subrecords.
        /// </summary>
        /// <param name="efsh"></param>
        /// <returns>int</returns>
        public int ApplySettingsTo(ref EffectShader efsh)
        {
            int changed = 0;
            // Edge Effect Alpha Ratios
            efsh.EdgeEffectPersistentAlphaRatio = ApplySettingsToValue(efsh.EdgeEffectPersistentAlphaRatio, EdgeEffectPersistentAlphaRatio, ref changed);
            efsh.EdgeEffectFullAlphaRatio = ApplySettingsToValue(efsh.EdgeEffectFullAlphaRatio, EdgeEffectFullAlphaRatio, ref changed);

            // Color Key 1
            efsh.ColorKey1 = ApplySettingsToValue(efsh.ColorKey1, ColorKey1, ref changed);
            efsh.ColorKey1Time = ColorKey1.GetTime(efsh.ColorKey1Time, out bool changedKey1Time);

            // Color Key 2
            efsh.ColorKey2 = ApplySettingsToValue(efsh.ColorKey2, ColorKey2, ref changed);
            efsh.ColorKey2Time = ColorKey2.GetTime(efsh.ColorKey2Time, out bool changedKey2Time);

            // Color Key 3
            efsh.ColorKey3 = ApplySettingsToValue(efsh.ColorKey3, ColorKey3, ref changed);
            efsh.ColorKey3Time = ColorKey3.GetTime(efsh.ColorKey3Time, out bool changedKey3Time);

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
