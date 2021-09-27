using System;
using System.Collections.Generic;
using Mutagen.Bethesda.FormKeys.SkyrimSE;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.WPF.Reflection.Attributes;
using RemoveEdgeGlow.Settings.GenericUtils;

namespace RemoveEdgeGlow.Settings.RecordSettings
{
    /// <summary>
    /// Contains all Art-Object-Specific Settings.
    /// </summary>
    public class SettingsArtObject : MatchEditorID
    {
        public SettingsArtObject() : base(
            whitelist: new()
            {
                "cloak"
            })
        { }
        [MaintainOrder]
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

        /// <summary>
        /// Get the correct model filepath with the current settings.
        /// </summary>
        /// <param name="current">The record's current model filepath.</param>
        /// <param name="changed">True when the current model filepath should be overridden.</param>
        /// <returns>string</returns>
        public string GetModelFile(string current, out bool changed)
        {
            changed = !current.Equals(EmptyEffectModel, StringComparison.OrdinalIgnoreCase) && EmptyEffectModel.Length > 0;
            return changed ? EmptyEffectModel : current;
        }

        /// <summary>
        /// Check if a given Art Object is present on the whitelist, or if the whitelist is disabled, if it is not on the blacklist
        /// Returns true when the given art object's formkey IS a valid target for the patcher.
        /// </summary>
        /// <param name="arto">Art Object Getter Interface</param>
        /// <returns>bool</returns>
        public bool IsValidPatchTarget(IArtObjectGetter arto)
        {
            if ( arto.EditorID == null )
                return false;
            bool onBlacklist = Blacklist.Contains(arto.FormKey) || IsBlacklistedEditorID(arto.EditorID); // query blacklist
            if ( !EnableWhitelist )
                return !onBlacklist; // return true if not blacklisted
            bool onWhitelist = Whitelist.Contains(arto.FormKey) || IsWhitelistedEditorID(arto.EditorID); // whitelist is enabled, query it
            return !onBlacklist && onWhitelist; // return true if not on blacklist or is on whitelist
        }
    }
}
