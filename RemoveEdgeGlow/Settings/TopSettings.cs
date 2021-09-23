using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        [SettingName("Edge Glow Settings (EFSH - Effect Shader)")]
        public SettingsEffectShader EffectShader = new();

        [SettingName("Visual Effect Settings (ARTO - Art Object)")]
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

        public bool ApplySettings(ref ArtObject arto)
        {
            if (arto.Model == null || arto.Model.File == null)
                return false;
            arto.Model.File = ArtObject.GetModelFile(arto.Model.File, out var changed);
            return changed;
        }
        public bool ApplySettings(ref EffectShader efsh, out int changes)
        {
            changes = EffectShader.ApplySettingsTo(ref efsh);
            return changes > 0;
        }
    }
}