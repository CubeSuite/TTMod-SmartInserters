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
            NewLimitGUI.Show();
        }

        [HarmonyPatch(typeof(FilterInserterUI), "OnClose")]
        [HarmonyPostfix]
        private static void HideLimitGUI() {
            NewLimitGUI.Hide();
        }
    }
}
