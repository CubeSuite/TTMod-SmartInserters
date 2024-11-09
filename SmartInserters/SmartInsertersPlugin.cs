using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using CasperEquinoxGUI;
using HarmonyLib;
using SmartInserters.Patches;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SmartInserters
{
    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class SmartInsertersPlugin : BaseUnityPlugin
    {
        private const string MyGUID = "com.equinox.SmartInserters";
        private const string PluginName = "SmartInserters";
        private const string VersionString = "2.0.0";

        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        public static Dictionary<uint, string> inserterLimits = new Dictionary<uint, string>();
        private static string dataFolder => $"{Application.persistentDataPath}/SmartInserters";

        #region Config Entries

        public static ConfigEntry<int> guiXOffset;
        public static ConfigEntry<int> guiYOffset;

        #endregion

        private void Awake() {
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loading...");
            Harmony.PatchAll();

            CaspuinoxGUI.ReadyForGUI += OnReadyForGUI;
            CreateConfigEntries();
            ApplyPatches();

            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loaded.");
            Log = Logger;
        }

        // Events

        private void OnReadyForGUI() {
            NewLimitGUI.CreateLimitGUI();
        }

        // Data Functions

        public static void SaveData(string worldName) {
            Directory.CreateDirectory(dataFolder);
            Directory.CreateDirectory($"{dataFolder}/{worldName}");
            string saveFile = $"{dataFolder}/{worldName}/InserterLimits.txt";

            List<string> lines = new List<string>();
            foreach(KeyValuePair<uint, string> pair in inserterLimits) {
                lines.Add($"{pair.Key}|{pair.Value}");
            }

            File.WriteAllLines(saveFile, lines);
        }

        public static void LoadData(string worldName) {
            string saveFile = $"{dataFolder}/{worldName}/InserterLimits.txt";
            if (!File.Exists(saveFile)) return;

            string[] lines = File.ReadAllLines(saveFile);
            foreach(string line in lines) {
                string[] parts = line.Split('|');
                uint id = uint.Parse(parts[0]);
                inserterLimits[id] = parts[1];
            }
        }

        // Private Functions

        private void ApplyPatches() {
            Harmony.CreateAndPatchAll(typeof(FilterInserterUIPatch));
            Harmony.CreateAndPatchAll(typeof(InserterInstancePatch));
            Harmony.CreateAndPatchAll(typeof(SaveStatePatch));
        }

        private void CreateConfigEntries() {
            guiXOffset = Config.Bind("General", "GUI X Offset", 0, new ConfigDescription("Controls the horizontal position of the limit GUI"));
            guiYOffset = Config.Bind("General", "GUI Y Offset", 0, new ConfigDescription("Controls the vertical position of the limit GUI"));
        }
    }
}
