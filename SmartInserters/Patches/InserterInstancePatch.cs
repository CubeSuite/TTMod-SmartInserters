using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInserters.Patches
{
    internal class InserterInstancePatch
    {
        [HarmonyPatch(typeof(InserterInstance), "Give")]
        [HarmonyPrefix]
        private static bool ShouldGiveItems(InserterInstance __instance) {
            uint id = __instance.commonInfo.instanceId;
            if (!SmartInsertersPlugin.inserterLimits.ContainsKey(id)) return true;
            if(__instance.filterType == -1) return true;

            ResourceInfo filteredItem = SaveState.GetResInfoFromId(__instance.filterType);
            int maxStack = filteredItem.maxStackCount;

            string limit = SmartInsertersPlugin.inserterLimits[id];
            int limitInt = 0;
            if (!limit.EndsWith("s")) {
                limitInt = int.Parse(limit);
            }
            else {
                limit = limit.Replace("s", "");
                limitInt = int.Parse(limit) * maxStack;
            }

            int numItemInGiveContainer = 0;
            foreach(Inventory inventory in __instance.giveResourceContainer.GetCommonInfo().inventories) {
                numItemInGiveContainer += inventory.GetResourceCount(__instance.filterType);
            }

            if (numItemInGiveContainer + __instance.currentHeldStackCount <= limitInt) return true;

            return false;
        }
    }
}
