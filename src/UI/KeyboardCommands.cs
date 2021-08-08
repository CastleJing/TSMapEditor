﻿using Microsoft.Xna.Framework.Input;
using Rampastring.Tools;
using System.Collections.Generic;
using TSMapEditor.Settings;

namespace TSMapEditor.UI
{
    public class KeyboardCommands
    {
        public KeyboardCommands()
        {
            Commands = new List<KeyboardCommand>()
            {
                Undo,
                Redo,
                NextTile,
                PreviousTile,
                NextTileSet,
                PreviousTileSet,
                NextSidebarNode,
                PreviousSidebarNode,
                FrameworkMode,
                NextBrushSize,
                PreviousBrushSize,
                DeleteObject,
                ToggleAutoLAT,
                RotateUnit
            };
        }

        public void ReadFromSettings()
        {
            IniFile iniFile = UserSettings.Instance.UserSettingsIni;

            foreach (var command in Commands)
            {
                string dataString = iniFile.GetStringValue("Keybinds", command.ININame, null);
                if (string.IsNullOrWhiteSpace(dataString))
                    continue;

                command.Key.ApplyDataString(dataString);
            }
        }

        public void WriteToSettings()
        {
            IniFile iniFile = UserSettings.Instance.UserSettingsIni;

            foreach (var command in Commands)
            {
                if (command.Key != command.DefaultKey)
                    iniFile.SetStringValue("Keybinds", command.ININame, command.Key.GetDataString());
            }
        }


        public static KeyboardCommands Instance { get; set; }

        public List<KeyboardCommand> Commands { get; }

        public KeyboardCommand Undo { get; } = new KeyboardCommand("Undo", "Undo", new KeyboardCommandInput(Keys.Z, KeyboardModifiers.Ctrl));
        public KeyboardCommand Redo { get; } = new KeyboardCommand("Redo", "Redo", new KeyboardCommandInput(Keys.Y, KeyboardModifiers.Ctrl));
        public KeyboardCommand NextTile { get; } = new KeyboardCommand("NextTile", "Select Next Tile", new KeyboardCommandInput(Keys.M, KeyboardModifiers.None));
        public KeyboardCommand PreviousTile { get; } = new KeyboardCommand("PreviousTile", "Select Previous Tile", new KeyboardCommandInput(Keys.N, KeyboardModifiers.None));
        public KeyboardCommand NextTileSet { get; } = new KeyboardCommand("NextTileSet", "Select Next TileSet", new KeyboardCommandInput(Keys.J, KeyboardModifiers.None));
        public KeyboardCommand PreviousTileSet { get; } = new KeyboardCommand("PreviousTileSet", "Select Previous TileSet", new KeyboardCommandInput(Keys.H, KeyboardModifiers.None));
        public KeyboardCommand NextSidebarNode { get; } = new KeyboardCommand("NextSidebarNode", "Select Next Sidebar Node", new KeyboardCommandInput(Keys.P, KeyboardModifiers.None));
        public KeyboardCommand PreviousSidebarNode { get; } = new KeyboardCommand("PreviousSidebarNode", "Select Previous Sidebar Node", new KeyboardCommandInput(Keys.O, KeyboardModifiers.None));
        public KeyboardCommand FrameworkMode { get; } = new KeyboardCommand("MarbleMadness", "Framework Mode (Marble Madness)", new KeyboardCommandInput(Keys.F, KeyboardModifiers.Shift));
        public KeyboardCommand NextBrushSize { get; } = new KeyboardCommand("NextBrushSize", "Next Brush Size", new KeyboardCommandInput(Keys.OemPlus, KeyboardModifiers.None));
        public KeyboardCommand PreviousBrushSize { get; } = new KeyboardCommand("PreviousBrushSize", "Previous Brush Size", new KeyboardCommandInput(Keys.D0, KeyboardModifiers.None));
        public KeyboardCommand DeleteObject { get; } = new KeyboardCommand("DeleteObject", "Delete Object", new KeyboardCommandInput(Keys.Delete, KeyboardModifiers.None));
        public KeyboardCommand ToggleAutoLAT { get; } = new KeyboardCommand("ToggleAutoLAT", "Toggle AutoLAT", new KeyboardCommandInput(Keys.L, KeyboardModifiers.Ctrl));
        public KeyboardCommand RotateUnit { get; } = new KeyboardCommand("RotateUnit", "Rotate Unit", new KeyboardCommandInput(Keys.A, KeyboardModifiers.None));

        public Keys SkipConfirmationKey { get; } = Keys.LeftAlt;
    }
}
