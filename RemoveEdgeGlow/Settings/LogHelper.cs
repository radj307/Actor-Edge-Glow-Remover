using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Records;
using RemoveEdgeGlow.Settings.PluginSettings;
using RemoveEdgeGlow.Settings.RecordSettings;

namespace RemoveEdgeGlow.Settings
{
    public readonly struct LogHelper
    {
        internal static void PrintSettingList(string name, List<string> list)
        {
            if (list.Count > 0 )
            {
                Console.WriteLine($"  {name} {{");
                list.ForEach(elem => Console.WriteLine($"    \"{elem}\""));
                Console.WriteLine("  }");
            }
        }
        internal static void PrintSettingList<T>(string name, List<FormLink<T>> list) where T : class, IMajorRecordCommonGetter
        {
            if (list.Count > 0 )
            {
                Console.WriteLine($"  {name} {{");
                list.ForEach(elem => Console.WriteLine($"    ({elem.FormKey.ModKey.ToString()})\t{elem.FormKey.IDString()}"));
                Console.WriteLine("  }");
            }
        }
        internal static void PrintSettingList(string name, List<ModKey> list)
        {
            if ( list.Count > 0 )
            {
                Console.WriteLine($"  {name} {{");
                list.ForEach(elem => Console.WriteLine($"    \"{elem}\""));
                Console.WriteLine("  }");
            }
        }

        internal static void PrintSettingSection(SettingsArtObject section)
        {
            Console.WriteLine($"{Constants.ArtObjectSectionName} {{");
            Console.WriteLine($"  EnableWhitelist = {section.EnableWhitelist}");
            PrintSettingList(nameof(section.Whitelist), section.Whitelist);
            PrintSettingList(nameof(section.Blacklist), section.Blacklist);
            PrintSettingList(nameof(section.StringWhitelist), section.StringWhitelist);
            PrintSettingList(nameof(section.StringBlacklist), section.StringBlacklist);
            Console.WriteLine("}");
        }
        internal static void PrintSettingSection(SettingsEffectShader section)
        {
            Console.WriteLine($"{Constants.EffectShaderSectionName} {{");
            Console.WriteLine($"  EnableWhitelist = {section.EnableWhitelist}");
            PrintSettingList(nameof(section.Whitelist), section.Whitelist);
            PrintSettingList(nameof(section.Blacklist), section.Blacklist);
            PrintSettingList(nameof(section.StringWhitelist), section.StringWhitelist);
            PrintSettingList(nameof(section.StringBlacklist), section.StringBlacklist);
            Console.WriteLine("}");
        }
        internal static void PrintSettingSection(SettingsModKey section)
        {
            Console.WriteLine($"{Constants.PluginSectionName} {{");
            PrintSettingList(nameof(section.Whitelist), section.Whitelist);
            PrintSettingList(nameof(section.Blacklist), section.Blacklist);
            Console.WriteLine("}");
        }

        public static void PrintSettings(TopSettings settings)
        {
            Console.WriteLine("\n=== Current Settings ===");

            PrintSettingSection(settings.Plugin);
            PrintSettingSection(settings.ArtObject);
            PrintSettingSection(settings.EffectShader);

            Console.WriteLine("=== Current Settings ===\n");
        }
    }
}
