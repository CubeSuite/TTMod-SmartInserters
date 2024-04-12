using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SmartInserters.Patches
{
    internal class FilterInserterUIPatch
    {
        [HarmonyPatch(typeof(FilterInserterUI), "OnOpen")]
        [HarmonyPostfix]
        private static void ShowLimitGUI() {
            LimitGUI.shouldShow = true;
            uint id = LimitGUI.GetAimedAtInserter().commonInfo.instanceId;
            LimitGUI.limit = SmartInsertersPlugin.inserterLimits.ContainsKey(id) ? SmartInsertersPlugin.inserterLimits[id] : "";
            Debug.Log("Opening FilterInserter GUI");
        }

        [HarmonyPatch(typeof(FilterInserterUI), "OnClose")]
        [HarmonyPostfix]
        private static void HideLimitGUI() {
            LimitGUI.shouldShow = false;
            Debug.Log("Closing FilterInserter GUI");
        }
    }
}
