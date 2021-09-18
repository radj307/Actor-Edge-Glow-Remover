using System.Collections.Generic;
using System.Linq;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.WPF.Reflection.Attributes;
using RemoveEdgeGlow.SettingsObjects;

namespace RemoveEdgeGlow
{
    public class Settings
    {
        [Tooltip("Add mods you don't want to patch here.")]
        public List<ModKey> ModBlacklist = new();
        public List<FormLink<EffectShader>> Blacklist = new()
        {

        };

        [SettingName("Shader Overrides")]
        public List<EffectShaderSettingsOverride> Overrides = new() { };

        [SettingName("Default Shader Override")]
        [Tooltip("These are the settings applied when there are no overrides specified for an effect.")]
        public EffectShaderSettings DefaultShaderSettings = new();

        private EffectShaderSettingsOverride GetHighestPriorityOverride(ref EffectShader efsh)
        {
            var highest = 0;
            var highestSettings = new EffectShaderSettingsOverride();
            foreach(var settings in Overrides.Where(s => !s.ShouldSkip()))
            {
                var priority = settings.GetPriority(efsh);
                if (priority <= highest)
                    continue;
                highestSettings = settings;
                highest = settings.Priority;
            }
            return highestSettings;
        }

        public bool ApplyChanges(ref EffectShader efsh, out int changeCount)
        {
            if (Overrides.Count != 0)
            {
                var settings = GetHighestPriorityOverride(ref efsh);
                if (!settings.ShouldSkip())
                {
                    changeCount = settings.ApplySettingsTo(ref efsh);
                    return changeCount > 0;
                }
            }
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
