using System.Collections.Generic;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.WPF.Reflection.Attributes;
using RemoveEdgeGlow.SettingsObjects;

namespace RemoveEdgeGlow
{
    public class Settings
    {
        [Tooltip("Add mods you don't want to patch here.")]
        public List<ModKey> ModBlacklist = new()
        { // default mod blacklist
        };
        public List<FormLink<EffectShader>> Blacklist = new()
        { // default formlink blacklist
        };

        [SettingName("Default Shader Override")]
        [Tooltip("These are the settings applied when there are no overrides specified for an effect.")]
        public EffectShaderSettings DefaultShaderSettings = new();

        public bool ApplyChanges(ref EffectShader efsh, out int changeCount)
        {
            changeCount = DefaultShaderSettings.ApplySettingsTo(ref efsh);
            return changeCount > 0;
        }

        private bool IsBlacklisted(ModKey modkey)
        {
            return ModBlacklist.Contains(modkey);
        }

        public bool IsBlacklisted(FormLink<EffectShader> formlink)
        {
            return IsBlacklisted(formlink.FormKey.ModKey) || Blacklist.Contains(formlink);
        }
    }
}
