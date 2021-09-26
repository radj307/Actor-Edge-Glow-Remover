using System;
using System.Collections.Generic;
using System.Linq;
using Mutagen.Bethesda.WPF.Reflection.Attributes;

namespace RemoveEdgeGlow.Settings.GenericUtils
{
    public class MatchEditorID
    {
        public MatchEditorID(List<string>? whitelist = null, List<string>? blacklist = null)
        {
            StringWhitelist = whitelist ?? new();
            StringBlacklist = blacklist ?? new();
        }

        [MaintainOrder]
        [SettingName("Common Names (Whitelist)")]
        [Tooltip("Any records whose Editor ID contains one of these words will be added to the Whitelist")]
        public List<string> StringWhitelist;
        [SettingName("Common Names (Blacklist)")]
        [Tooltip("Any records whose Editor ID contains one of these words will be added to the Blacklist")]
        public List<string> StringBlacklist;

        protected bool IsWhitelistedEditorID(string editorID)
        {
            return StringWhitelist.Any(substr => editorID.Contains(substr, StringComparison.OrdinalIgnoreCase));
        }

        protected bool IsBlacklistedEditorID(string editorID)
        {
            return StringBlacklist.Any(substr => editorID.Contains(substr, StringComparison.OrdinalIgnoreCase));
        }
    }
}
