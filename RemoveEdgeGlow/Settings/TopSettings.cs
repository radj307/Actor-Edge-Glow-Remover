using System.Collections.Generic;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.WPF.Reflection.Attributes;
using RemoveEdgeGlow.Settings.RecordSettings;

namespace RemoveEdgeGlow.Settings
{
    public class TopSettings
    {

        [SettingName("Plugin Blacklist")]
        public List<ModKey> ModKeys = new()
        { // Default Mod Key List
        };

        [SettingName(nameof(Constants.EffectShaderSectionName))]
        public SettingsEffectShader EffectShader = new();

        [SettingName(nameof(Constants.ArtObjectSectionName))]
        public SettingsArtObject ArtObject = new();

        public bool IsBlacklisted(ModKey key)
        {
            return ModKeys.Contains(key);
        }
        public bool IsBlacklisted(IArtObjectGetter arto)
        {
            return IsBlacklisted(arto.FormKey.ModKey) || ArtObject.IsBlacklisted(arto);
        }
        public bool IsBlacklisted(IEffectShaderGetter efsh)
        {
            return IsBlacklisted(efsh.FormKey.ModKey) || EffectShader.IsBlacklisted(efsh);
        }

        public bool ApplySettingsTo(ref ArtObject arto)
        {
            if (arto.Model == null || arto.Model.File == null)
                return false;
            arto.Model.File = ArtObject.GetModelFile(arto.Model.File, out var changed);
            return changed;
        }
        public bool ApplySettingsTo(ref EffectShader efsh, out int changes)
        {
            changes = EffectShader.ApplySettingsTo(ref efsh);
            return changes > 0;
        }
    }
}
