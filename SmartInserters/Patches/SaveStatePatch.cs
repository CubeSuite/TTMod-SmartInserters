using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInserters.Patches
{
    internal class SaveStatePatch
    {
        [HarmonyPatch(typeof(SaveState), "SaveToFile")]
        [HarmonyPostfix]
        private static void SaveData() {
            SmartInsertersPlugin.SaveData(SaveState.instance.metadata.worldName);
        }


        [HarmonyPatch(typeof(SaveState), "LoadFileData", typeof(SaveState.SaveMetadata), typeof(string))]
        [HarmonyPostfix]
        private static void LoadData(SaveState __instance, SaveState.SaveMetadata saveMetadata, string replayLocation) {
            SmartInsertersPlugin.LoadData(saveMetadata.worldName);
        }
    }
}
