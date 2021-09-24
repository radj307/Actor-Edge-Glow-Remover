using System;
using System.Collections.Generic;
using System.Linq;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.WPF.Reflection.Attributes;

namespace RemoveEdgeGlow.Settings.GenericUtils
{
    public class MatchEditorID
    {
        public MatchEditorID()
        {
            InvalidEntryCount = 0;
            CommonNames = new();
        }
        public MatchEditorID(List<string> commonNames)
        {
            InvalidEntryCount = commonNames.RemoveAll(n => n.Length < 2);
            CommonNames = commonNames;
        }

        [MaintainOrder]
        private bool Initialized; // used to remove any invalid user-added entries
        private int InvalidEntryCount;
        [Tooltip("If the Editor ID of a record contains a word included here, it will be considered whitelisted. Entries are Case-Insensitive and must be at least 2 characters long.")]
        public List<string> CommonNames;

        public bool GetInvalidEntryCount(out int invalidEntries)
        {
            if (!Initialized)
                CommonNames = Validate(CommonNames, out InvalidEntryCount);
            invalidEntries = InvalidEntryCount;
            return invalidEntries > 0;
        }

        /// <summary>
        /// Remove short entries from the given string list.
        /// </summary>
        /// <param name="list">List of strings to validate.</param>
        /// <param name="invalid">Number of invalid entries removed.</param>
        /// <param name="rmShorterThan">Any list entries shorter or equal to this number are removed.</param>
        /// <returns>List\<string\></returns>
        private static List<string> Validate(List<string> list, out int invalid, int rmShorterThan = 2)
        {
            invalid = list.RemoveAll(n => n.Length < rmShorterThan);
            return list;
        }

        /// <summary>
        /// Check if the given IMajorRecordCommonGetter instance's editor ID contains a match.
        /// </summary>
        /// <typeparam name="T">Type that implements the IMajorRecordCommonGetter interface.</typeparam>
        /// <param name="obj">An IMajorRecordCommonGetter instance.</param>
        /// <returns>bool</returns>
        public bool HasMatch<T>(T obj) where T : class, IMajorRecordCommonGetter
        {
            if (!Initialized)
            {
                CommonNames = Validate(CommonNames, out InvalidEntryCount);
                Initialized = true;
            }
            if (obj.EditorID == null)
                return false;
            return CommonNames.Any(n => obj.EditorID.Contains(n, StringComparison.OrdinalIgnoreCase));
        }
    }
}
