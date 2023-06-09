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
using PlantsVsZombies.Common.Systems;

namespace PlantsVsZombies.Common.UI
{
    //TLDR: this lets the player see how much sun they currently have
    //I barely understand this part myself, there is a guide on the tModLoader wiki pages
    class SunUISystem : ModSystem
    {

        internal UserInterface MyInterface;
        internal SunUI MyUI;

        private GameTime _lastUpdateUiGameTime;

        //hiding and showing the ui by a hotkey
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
            if (!Main.dedServ) //dedServ means dedicated server, so the ui doesn't load on the server, as it doesn't need to
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
            //only update the ui if it's visible, also using a case statement since a use case hasn't yet presented itself
            _lastUpdateUiGameTime = gameTime;
            var InterfaceState = 0;
            if (MyInterface?.CurrentState != null)
            {
                InterfaceState = 1;
            }
            else { InterfaceState = 0; }
            switch (InterfaceState)
            {
                case 1:
                    {
                        MyInterface.Update(gameTime);
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            //this block puts the sun ui directly behind the mouse text layer (text when hovering an item with the mouse)
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

        //this method was my original way of checking the item the player was holding, until a better way was made known to me
#pragma warning disable IDE0052 // Remove unread private members
        readonly string[] plantWeapons = new string[2] { "PeashooterPacket", "SunflowerPacket" };
#pragma warning restore IDE0052 // Remove unread private members
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
            //this block makes the ui not show when you aren't holding a plant or packet
            if (!Main.LocalPlayer.HeldItem.CountsAsClass<Plants>())
                return;

            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            //same as above, only updates if holding a plant
            if (!Main.LocalPlayer.HeldItem.CountsAsClass<Plants>())
                return;

            var modPlayer = Main.LocalPlayer.GetModPlayer<Sun>();

            value.SetText($"{modPlayer.SunCurrent}");
            base.Update(gameTime);
        }

        
    }
}