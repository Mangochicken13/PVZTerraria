﻿using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Drawing.Text;
using PlantsVsZombies.Common.Players;
using PlantsVsZombies.Content.Items.Weapons.PlantSummons;
using System.Linq;
using PlantsVsZombies.Common.Systems;

namespace PlantsVsZombies.Common.UI
{
    class SunUISystem : ModSystem
    {

        internal UserInterface MyInterface;
        internal SunUI MyUI;

        private GameTime _lastUpdateUiGameTime;


        internal void ShowMyUI()
        {
            MyInterface?.SetState(MyUI);
        }
        internal void HideMyUI()
        {
            MyInterface?.SetState(null);
        }

        public override void Load()
        {
            if (!Main.dedServ)
            {
                MyInterface = new UserInterface();

                MyUI = new SunUI();
                MyUI.Activate();
            }
        }

        public override void OnWorldLoad()
        {
            MyInterface?.SetState(MyUI);
        }

        public override void UpdateUI(GameTime gameTime)
        {
            _lastUpdateUiGameTime = gameTime;
            if (MyInterface?.CurrentState != null)
            {
                MyInterface.Update(gameTime);
            }
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "PlantsVsZombies: SunResource",
                    delegate
                    {
                        if (_lastUpdateUiGameTime != null && MyInterface?.CurrentState != null)
                        {
                            MyInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
                        }
                        return true;
                    },
                    InterfaceScaleType.UI));
            }
        }
    }
    class SunUI : UIState
    {
        private UIText value;
        private UIElement panel;
        private UIImage sunBg;
        readonly string[] plantWeapons = new string[1] { "PeashooterPacket" };
        public override void OnInitialize()
        {
            panel = new UIElement();
            panel.Left.Set(-panel.Width.Pixels - 400, 1f);
            panel.Top.Set(30, 0);
            panel.Width.Set(60, 0);
            panel.Height.Set(60, 0);

            sunBg = new UIImage(ModContent.Request<Texture2D>("PlantsVsZombies/Common/UI/SunUIImage"));
            sunBg.Left.Set(22, 0);
            sunBg.Top.Set(0, 0);
            sunBg.Width.Set(60, 0);
            sunBg.Height.Set(60, 0);

            value = new UIText("0", 1f);
            value.Width.Set(32, 0);
            value.Height.Set(64, 0);
            value.Top.Set(30, 0);
            value.Left.Set(40, 0);

            panel.Append(sunBg);
            panel.Append(value);
            Append(panel);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Main.LocalPlayer.HeldItem.CountsAsClass<Plants>())
                return;

            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (!Main.LocalPlayer.HeldItem.CountsAsClass<Plants>())
                return;

            var modPlayer = Main.LocalPlayer.GetModPlayer<Sun>();

            value.SetText($"{modPlayer.SunCurrent}");
            base.Update(gameTime);
        }

        
    }
}