using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.WPF.Reflection.Attributes;
using RemoveEdgeGlow.Settings.PluginSettings;
using RemoveEdgeGlow.Settings.RecordSettings;

namespace RemoveEdgeGlow.Settings
{
    public class TopSettings
    {
        // Plugin-Specific Section
        [SettingName(nameof(Constants.PluginSectionName))]
        public SettingsModKey Plugin = new();
        // Effect Shader Section
        [SettingName(nameof(Constants.EffectShaderSectionName))]
        public SettingsEffectShader EffectShader = new();
        // Art Object Section
        [SettingName(nameof(Constants.ArtObjectSectionName))]
        public SettingsArtObject ArtObject = new();

        public bool PrintSettings;

        /// <summary>
        /// Check if the given Art Object record is a valid patcher target with the current settings.
        /// </summary>
        /// <param name="arto">Art Object Record</param>
        /// <returns>bool</returns>
        public bool IsValidPatcherTarget(IArtObjectGetter arto)
        {
            return !Plugin.IsBlacklistedPlugin(arto.FormKey.ModKey) && ( Plugin.IsWhitelistedPlugin(arto.FormKey.ModKey) || ArtObject.IsValidPatchTarget(arto) );
        }
        /// <summary>
        /// Check if the given Effect Shader record is a valid patcher target with the current settings.
        /// </summary>
        /// <param name="efsh">Effect Shader Record</param>
        /// <returns>bool</returns>
        public bool IsValidPatcherTarget(IEffectShaderGetter efsh)
        {
            return !Plugin.IsBlacklistedPlugin(efsh.FormKey.ModKey) && ( Plugin.IsWhitelistedPlugin(efsh.FormKey.ModKey) || EffectShader.IsValidPatchTarget(efsh) );
        }
        /// <summary>
        /// Apply the current Art Object Settings to a given ArtObject ref.
        /// Returns true when a value was modified.
        /// </summary>
        /// <param name="arto">ArtObject reference to modify</param>
        /// <returns>bool</returns>
        public bool ApplySettingsTo(ref ArtObject arto)
        {
            if ( arto.Model == null || arto.Model.File == null )
                return false;
            arto.Model.File = ArtObject.GetModelFile(arto.Model.File, out bool changed);
            return changed;
        }
        /// <summary>
        /// Apply the current Effect Shader Settings to a given EffectShader ref.
        /// Returns true when at least one value was modified.
        /// </summary>
        /// <param name="efsh">EffectShader reference to modify</param>
        /// <param name="changes">Total number of modified values</param>
        /// <returns>bool</returns>
        public bool ApplySettingsTo(ref EffectShader efsh, out int changes)
        {
            changes = EffectShader.ApplySettingsTo(ref efsh);
            return changes > 0;
        }
    }
}
