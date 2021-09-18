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
        public EffectShaderSettings DefaultShaderSettings = new();
        public List<EffectShaderSettingsOverride> Overrides = new() {};

        [Tooltip("Add mods you don't want to patch here.")]
        public List<ModKey> ModBlacklist = new();
        public List<FormLink<EffectShader>> Blacklist = new()
        {

        };

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
            }
            return highestSettings;
        }

        public bool ApplyChanges(ref EffectShader efsh)
        {
            if (Overrides.Count != 0)
            {
                var settings = GetHighestPriorityOverride(ref efsh);

                if (!settings.ShouldSkip()) // if overrides return null settings, apply defaults
                    return settings.ApplySettingsTo(efsh);
            }
            return DefaultShaderSettings.ApplySettingsTo(efsh);
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
