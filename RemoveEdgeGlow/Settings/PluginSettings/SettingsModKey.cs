using System.Collections.Generic;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.WPF.Reflection.Attributes;

namespace RemoveEdgeGlow.Settings.PluginSettings
{
    /// <summary>
    /// <para>Plugin-wide whitelist/blacklist settings object.</para>
    /// <para>
    /// Allows users to prevent patcher from tampering with a specific plugin,
    /// or specifying that all records from a given plugin should
    /// always be considered a valid target.
    /// </para>
    /// </summary>
    public class SettingsModKey
    {
        [SettingName("Always Override Plugins")]
        [Tooltip("Always consider records to be a match if they were last modified by one of these plugins.")]
        public List<ModKey> Whitelist = new()
        { // Default Plugin Whitelist
        };

        [SettingName("Plugin Blacklist")]
        [Tooltip("Never consider records to be a match if they were last modified by one of these plugins, EVEN IF IT APPEARS ON THE WHITELIST!")]
        public List<ModKey> Blacklist = new()
        { // Default Plugin Blacklist
        };

        public bool IsWhitelistedPlugin(ModKey modkey)
        {
            return Whitelist.Contains(modkey);
        }

        public bool IsBlacklistedPlugin(ModKey modkey)
        {
            return Blacklist.Contains(modkey);
        }
    }
}
