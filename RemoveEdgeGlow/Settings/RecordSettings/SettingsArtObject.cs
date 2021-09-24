using System;
using System.Collections.Generic;
using Mutagen.Bethesda.FormKeys.SkyrimSE;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.WPF.Reflection.Attributes;

namespace RemoveEdgeGlow.Settings.RecordSettings
{
    [ObjectNameMember(nameof(EmptyEffectModel))]
    public class SettingsArtObject : MatchEditorID
    {
        public SettingsArtObject() : base(new()
        {
            "cloak"
        }) { }
        [SettingName("Model Filepath")]
        [Tooltip("Absolute or relative filepath to a .nif containing a blank Art Object.")]
        public string EmptyEffectModel = Constants.EmptyArtObjectModel;

        [Tooltip("If Enable Whitelist is unchecked, ALL art objects except those on the blacklist will be patched.")]
        public bool EnableWhitelist = true;
        public List<FormLink<IArtObjectGetter>> Whitelist = new()
        { // ARTO Whitelist
            Skyrim.ArtObject.HealTargetFX,
            Skyrim.ArtObject.HealConTargetFX,
            Skyrim.ArtObject.HealMystTargetFX,
            Skyrim.ArtObject.HealMystFXHand01,
            Skyrim.ArtObject.HealMystConTargetFX01,
            Skyrim.ArtObject.HealRitualHandEffects,
            Skyrim.ArtObject.HealRitualCastBodyFX
        };
        [Tooltip("The blacklist takes priority over the whitelist, if the same art object is specified in both it will not be patched.")]
        public List<FormLink<IArtObjectGetter>> Blacklist = new()
        { // ARTO Blacklist
        };

        public string GetModelFile(string current, out bool changed)
        {
            changed = !current.Equals(EmptyEffectModel, StringComparison.OrdinalIgnoreCase) && EmptyEffectModel.Length > 0;
            return changed ? EmptyEffectModel : current;
        }

        public bool IsBlacklisted(IArtObjectGetter arto)
        {
            bool onBlacklist = Blacklist.Contains(arto.FormKey); // query blacklist
            if (!EnableWhitelist) // whitelist is disabled
                return onBlacklist; // return true if on blacklist
            bool onWhitelist = Whitelist.Contains(arto.FormKey) || HasMatch(arto); // whitelist is enabled, query it
            return !onWhitelist || onBlacklist; // return true if not on whitelist or on blacklist
        }
    }
}
