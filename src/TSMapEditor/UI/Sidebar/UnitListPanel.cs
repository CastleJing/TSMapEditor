﻿using Microsoft.Xna.Framework.Graphics;
using Rampastring.XNAUI;
using System;
using TSMapEditor.Models;
using TSMapEditor.Rendering;
using TSMapEditor.UI.CursorActions;

namespace TSMapEditor.UI.Sidebar
{
    /// <summary>
    /// A sidebar panel for listing units.
    /// </summary>
    public class UnitListPanel : ObjectListPanel
    {
        public UnitListPanel(WindowManager windowManager, EditorState editorState, Map map, TheaterGraphics theaterGraphics, ICursorActionTarget cursorActionTarget, bool isNaval) : base(windowManager, editorState, map, theaterGraphics)
        {
            unitPlacementAction = new UnitPlacementAction(cursorActionTarget, Keyboard);
            unitPlacementAction.ActionExited += UnitPlacementAction_ActionExited;

            this.isNaval = isNaval;
        }

        private void UnitPlacementAction_ActionExited(object sender, System.EventArgs e)
        {
            ObjectTreeView.SelectedNode = null;
        }

        private readonly bool isNaval;
        private readonly UnitPlacementAction unitPlacementAction;

        private RenderTarget2D renderTarget;

        protected override (Texture2D regular, Texture2D remap) GetObjectTextures<T>(T objectType, ShapeImage[] textures)
        {
            const byte facingSouthEast = 64;

            var unitType = objectType as UnitType;
            if (unitType.ArtConfig.Voxel)
                return GetTextureForVoxel(objectType, TheaterGraphics.UnitModels, renderTarget, facingSouthEast);

            return base.GetObjectTextures(objectType, textures);
        }

        protected override void InitObjects()
        {
            Func<UnitType, bool> filterFunction = null;

            if (isNaval)
            {
                filterFunction = u => u.SpeedType == "Float" || u.SpeedType == "Amphibious" || u.SpeedType == "Hover" || u.MovementZone == "Water";
            }
            else
            {
                filterFunction = u => u.SpeedType != "Float" && u.MovementZone != "Water";
            }

            renderTarget = new RenderTarget2D(GraphicsDevice, ObjectTreeView.Width, ObjectTreeView.LineHeight, false, SurfaceFormat.Color, DepthFormat.None);
            InitObjectsBase(Map.Rules.UnitTypes, TheaterGraphics.UnitTextures, filterFunction);
            renderTarget.Dispose();
        }

        protected override void ObjectSelected()
        {
            if (ObjectTreeView.SelectedNode == null)
                return;

            unitPlacementAction.UnitType = (UnitType)ObjectTreeView.SelectedNode.Tag;
            EditorState.CursorAction = unitPlacementAction;
        }
    }
}
