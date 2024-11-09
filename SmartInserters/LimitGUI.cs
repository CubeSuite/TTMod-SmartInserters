using CasperEquinoxGUI;
using CasperEquinoxGUI.Controls;
using CasperEquinoxGUI.Layouts;
using CasperEquinoxGUI.Utilities;
using EquinoxsModUtils;
using FIMSpace;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SmartInserters
{
    public static class NewLimitGUI 
    {
        // GUI
        public static Window limitWindow;
        public static Grid grid;
        public static Textbox limitBox;
        public static Button setButton;

        private static uint currentInserterId;

        // Public Functions

        public static void CreateLimitGUI() {
            grid = new Grid(1, 2, "equal", "equal");

            limitBox = new Textbox() { Margin = new Thickness(0, 0, 0, 10) };
            setButton = new Button("Set") { 
                RowIndex = 1,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };

            setButton.LeftClicked += OnSetClicked;

            grid.AddControl(limitBox);
            grid.AddControl(setButton);

            limitWindow = new Window() {
                Width = 200,
                Height = 90,
                ShowBackground = false,
                ShowTitle = false,
                ShowCloseButton = false,
                ShowShader = false,
                Visible = false,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(SmartInsertersPlugin.guiXOffset.Value, SmartInsertersPlugin.guiYOffset.Value, 0, 0),
                RootLayout = grid
            };

            CaspuinoxGUI.RegisterWindow(ref limitWindow);
        }

        public static void Show() {
            currentInserterId = GetAimedAtInserter().commonInfo.instanceId;
            limitWindow.Show();

            if (SmartInsertersPlugin.inserterLimits.ContainsKey(currentInserterId)) {
                limitBox.Input = SmartInsertersPlugin.inserterLimits[currentInserterId];
            }
            else {
                limitBox.Input = "";
            }
        }

        public static void Hide() {
            limitWindow?.Hide();
        }


        // Events

        public static void OnSetClicked(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(limitBox.Input)) {
                Player.instance.audio.buildError.PlayRandomClip();
                Debug.Log("limit is null or emtpy");
                return;
            }

            string limit = limitBox.Input;
            limit = limit.ToLower();
            int limitInt;

            if (!limit.EndsWith("s") && !int.TryParse(limit, out limitInt)) {
                Player.instance.audio.buildError.PlayRandomClip();
                Debug.Log($"limit: {limit}");
                return;
            }

            uint id = GetAimedAtInserter().commonInfo.instanceId;
            SmartInsertersPlugin.inserterLimits[id] = limit;
            Player.instance.audio.buildClick.PlayRandomClip();
        }

        // Private Functions

        public static InserterInstance GetAimedAtInserter() {
            GenericMachineInstanceRef machine = (GenericMachineInstanceRef)EMU.GetPrivateField("targetMachineRef", Player.instance.interaction);
            return MachineManager.instance.Get<InserterInstance, InserterDefinition>(machine.index, MachineTypeEnum.Inserter);
        }
    }

    //public static class LimitGUI
    //{
    //    // Objects & Variables
    //    public static bool shouldShow = false;

    //    // Textures
    //    private static Texture2D textBoxNormal;
    //    private static Texture2D textBoxHover;

    //    // Field Values
    //    public static string limit;

    //    // Public Functions

    //    public static float xOffset => SmartInsertersPlugin.guiXOffset.Value;
    //    public static float yOffset => SmartInsertersPlugin.guiYOffset.Value;
    //    public static float xPos => (Screen.width / 2.0f) + xOffset;

    //    public static void DrawGUI() {
    //        GUIStyle labelStyle = new GUIStyle() {
    //            fontSize = 16,
    //            normal = { textColor = Color.white, background = null }
    //        };
    //        GUIStyle textBoxStyle = new GUIStyle() {
    //            fontSize = 16,
    //            alignment = TextAnchor.MiddleCenter,
    //            normal = { textColor = Color.white, background = textBoxNormal },
    //            hover = { textColor = Color.white, background = textBoxHover },
    //        };

    //        GUI.Label(new Rect(xPos, yOffset, 200, 40), "Limit:", labelStyle);
    //        limit = GUI.TextField(new Rect(xPos, yOffset + 30, 240, 40), limit, textBoxStyle);
    //        if (GUI.Button(new Rect(xPos, yOffset + 80, 240, 40), "Set", textBoxStyle)) {
    //            if (string.IsNullOrEmpty(limit)) {
    //                Player.instance.audio.buildError.PlayRandomClip();
    //                Debug.Log("limit is null or emtpy");
    //                return;
    //            }

    //            int limitInt;
    //            limit = limit.ToLower();
    //            if(!limit.EndsWith("s") && !int.TryParse(limit, out limitInt)) {
    //                Player.instance.audio.buildError.PlayRandomClip();
    //                Debug.Log($"limit: {limit}");
    //                return;
    //            }

    //            uint id = GetAimedAtInserter().commonInfo.instanceId;
    //            SmartInsertersPlugin.inserterLimits[id] = limit;
    //            Player.instance.audio.buildClick.PlayRandomClip();
    //        }
    //    }

    //    public static void LoadImages() {
    //        LoadImage("SmartInserters.Images.Border240x40.png", ref textBoxNormal);
    //        LoadImage("SmartInserters.Images.BorderHover240x40.png", ref textBoxHover);
    //    }

    //    public static InserterInstance GetAimedAtInserter() {
    //        GenericMachineInstanceRef machine = (GenericMachineInstanceRef)ModUtils.GetPrivateField("targetMachineRef", Player.instance.interaction);
    //        return MachineManager.instance.Get<InserterInstance, InserterDefinition>(machine.index, MachineTypeEnum.Inserter);
    //    }

    //    // Private Functions

    //    private static void LoadImage(string path, ref Texture2D output) {
    //        Assembly assembly = Assembly.GetExecutingAssembly();

    //        using (Stream stream = assembly.GetManifestResourceStream(path)) {
    //            if (stream == null) {
    //                Debug.LogError($"Could not find image with path '{path}'");
    //                return;
    //            }

    //            using (MemoryStream memoryStream = new MemoryStream()) {
    //                stream.CopyTo(memoryStream);
    //                byte[] fileData = memoryStream.ToArray();

    //                output = new Texture2D(2, 2);
    //                output.LoadImage(fileData);
    //            }
    //        }
    //    }
    //}
}
