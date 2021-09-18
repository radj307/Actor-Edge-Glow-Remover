using System;
using System.Collections.Generic;
using System.Linq;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;
using System.Threading.Tasks;

namespace RemoveEdgeGlow
{
    public static class Program
    {
        internal static Lazy<Settings> _lazySettings = null!;
        internal static Settings Settings => _lazySettings.Value;

        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .SetAutogeneratedSettings("Settings", "settings.json", out _lazySettings)
                .SetTypicalOpen(GameRelease.SkyrimSE, "RemoveEdgeGlow.esp")
                .Run(args);
        }

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            Console.WriteLine("\n\nInitialization Complete.\nBeginning Process...\n");
            int countChanges = 0;

            foreach (var efsh in state.LoadOrder.PriorityOrder.EffectShader().WinningOverrides())
            {
                if ( ShouldSkip(efsh) || Settings.IsBlacklisted(efsh.FormKey) || efsh.EditorID == null )
                    continue;

                Console.WriteLine($"Currently Processing {efsh.EditorID}");

                var efshCopy = efsh.DeepCopy();

                if (Settings.ApplyChanges(ref efshCopy))
                {
                    state.PatchMod.EffectShaders.Set(efshCopy);
                    ++countChanges;
                }
            }

            if (countChanges == 0)
                Console.WriteLine("Failed to modify any records! Check your settings!");
            Console.WriteLine($"\nProcess Complete.\nPatched {countChanges} record{(countChanges > 1 ? "s" : "")}.\n");
        }

        internal static bool ShouldSkip(IEffectShaderGetter efsh)
        {
            return efsh.EditorID == null;
        }
    }
}
