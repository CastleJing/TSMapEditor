﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rampastring.Tools;
using Rampastring.XNAUI;
using System;
using System.IO;
using System.Text;
using TSMapEditor.CCEngine;
using TSMapEditor.Settings;
using TSMapEditor.UI;

namespace TSMapEditor.Rendering
{
    public class GameClass : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;

        public GameClass()
        {
            AutoLATType.InitArray();

            new UserSettings();

            graphics = new GraphicsDeviceManager(this);
            graphics.HardwareModeSwitch = false;
            graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Content.RootDirectory = "Content";
            graphics.SynchronizeWithVerticalRetrace = false;
            Window.Title = "DTA Scenario Editor";

            //IsFixedTimeStep = false;
            TargetElapsedTime = TimeSpan.FromMilliseconds(1000.0 / UserSettings.Instance.TargetFPS);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string exceptLogPath = Environment.CurrentDirectory + DSC + "except.txt";
            Exception ex = (Exception)e.ExceptionObject;
            File.Delete(exceptLogPath);

            StringBuilder sb = new StringBuilder();

            LogLineGenerate("Unhandled exception! @ " + DateTime.Now.ToLongTimeString(), sb, exceptLogPath);
            LogLineGenerate("Message: " + ex.Message, sb, exceptLogPath);
            LogLineGenerate("Stack trace: " + ex.StackTrace, sb, exceptLogPath);

            if (ex.InnerException != null)
            {
                LogLineGenerate("***************************", sb, exceptLogPath);
                LogLineGenerate("InnerException information:", sb, exceptLogPath);
                LogLineGenerate("Message: " + ex.InnerException.Message, sb, exceptLogPath);
                LogLineGenerate("Stack trace: " + ex.InnerException.StackTrace, sb, exceptLogPath);
            }

            Logger.Log("Exiting.");

            windowManager.HideWindow();
            System.Windows.Forms.MessageBox.Show("The map editor has crashed. Go bash Rampastring." + Environment.NewLine + Environment.NewLine +
                "Exception information logged into except.txt:" + Environment.NewLine + Environment.NewLine +
                sb.ToString());

            Environment.Exit(255);
        }

        private void LogLineGenerate(string text, StringBuilder sb, string exceptLogPath)
        {
            sb.Append(text + Environment.NewLine);
            Logger.ForceLog(text, exceptLogPath);
        }

        private WindowManager windowManager;

        private readonly char DSC = Path.DirectorySeparatorChar;

        protected override void Initialize()
        {
            base.Initialize();

            AssetLoader.Initialize(GraphicsDevice, Content);
            AssetLoader.AssetSearchPaths.Add(Environment.CurrentDirectory + DSC + "Content" + DSC);

            windowManager = new WindowManager(this, graphics);
            windowManager.Initialize(Content, Environment.CurrentDirectory + DSC + "Content" + DSC);

            bool fullscreenWindowed = UserSettings.Instance.FullscreenWindowed.GetValue();
            bool borderless = UserSettings.Instance.Borderless.GetValue();
            if (fullscreenWindowed && !borderless)
                throw new InvalidOperationException("Borderless= cannot be set to false if FullscreenWindowed= is enabled.");

            windowManager.InitGraphicsMode(
                UserSettings.Instance.ResolutionWidth.GetValue(),
                UserSettings.Instance.ResolutionHeight.GetValue(),
                fullscreenWindowed);

            windowManager.SetRenderResolution(UserSettings.Instance.RenderResolutionWidth.GetValue(), UserSettings.Instance.RenderResolutionHeight.GetValue());
            windowManager.CenterOnScreen();
            windowManager.Cursor.LoadNativeCursor(Environment.CurrentDirectory + DSC + "Content" + DSC + "cursor.cur");
            windowManager.SetBorderlessMode(borderless);

            Components.Add(windowManager);

            UISettings.ActiveSettings.CheckBoxCheckedTexture = AssetLoader.LoadTextureUncached("checkBoxChecked.png");
            UISettings.ActiveSettings.CheckBoxClearTexture = AssetLoader.LoadTextureUncached("checkBoxClear.png");
            UISettings.ActiveSettings.CheckBoxDisabledCheckedTexture = AssetLoader.LoadTextureUncached("checkBoxCheckedD.png");
            UISettings.ActiveSettings.CheckBoxDisabledClearTexture = AssetLoader.LoadTextureUncached("checkBoxClearD.png");

            InitMainMenu();
        }

        private void InitMainMenu()
        {
            var mainMenu = new MainMenu(windowManager);
            windowManager.AddAndInitializeControl(mainMenu);
            windowManager.CenterControlOnScreen(mainMenu);
        }
    }
}
