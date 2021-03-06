﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Mathematica;
using TGC.Core.Text;
using TGC.Group.Model.Items;
using TGC.Group.Model.Utils.Sprites;

namespace TGC.Group.Model.Menus.Inventory
{
    class InventorySlot : MenuSlot
    {
        private readonly Item itemSample;
        private readonly int amount;

        private TgcText2D itemName = new TgcText2D();
        private CustomSprite itemSprite;
        private CustomSprite slotBackground;
        private TgcText2D itemDescription = new TgcText2D();
        private TgcText2D itemAmount = new TgcText2D();


        public InventorySlot(TGCVector2 position, EItemID itemID, int amount, Menu menu, CustomBitmap bitmapBackground)
            : base(position, menu)
        {
            itemSample = ItemDatabase.Instance.Generate(itemID);
            this.amount = amount;

            itemSprite = ItemDatabase.Instance.ItemSprites[itemID];
            itemSprite.Position = Position;
            float scalingFactor = 0.25f;
            itemSprite.Scaling = TGCVector2.One * scalingFactor;

            itemName.Text = itemSample.Name;
            itemName.Position = new Point((int)(itemSprite.Position.X + itemSprite.Bitmap.Size.Width * scalingFactor + 10), (int)itemSprite.Position.Y + 5);
            itemName.Align = TgcText2D.TextAlign.LEFT;
            itemName.changeFont(new Font("TimesNewRoman", 14, FontStyle.Bold));
            itemName.Color = Color.DarkGray;

            itemDescription.Text = itemSample.Description;
            itemDescription.Position = new Point(itemName.Position.X, itemName.Position.Y + 30);
            itemDescription.Size = new Size(450, 300);
            itemDescription.Align = TgcText2D.TextAlign.LEFT;
            itemDescription.changeFont(new Font("TimesNewRoman", 11, FontStyle.Bold));
            itemDescription.Color = Color.LightGray;

            itemAmount.Text = amount.ToString();
            itemAmount.Position = new Point((int)itemSprite.Position.X, (int)itemSprite.Position.Y);
            itemAmount.Align = TgcText2D.TextAlign.LEFT;
            itemAmount.changeFont(new Font("TimesNewRoman", 14, FontStyle.Bold));
            itemAmount.Color = Color.White;

            Size = new Size((int)(itemSprite.Bitmap.Size.Width * scalingFactor + itemDescription.Size.Width + 180), (int)(itemSprite.Bitmap.Height * scalingFactor + 30));

            slotBackground = new CustomSprite();
            slotBackground.Bitmap = bitmapBackground;
            slotBackground.Position = new TGCVector2(Position.X - 25, Position.Y - 15);
            // slotBackground.SrcRect = new Rectangle((int)position.X, (int)position.Y, Size.Width, Size.Height);
            slotBackground.SrcRect = new Rectangle(0, 0, Size.Width, Size.Height);
        }

        public override void RenderSprites(Drawer2D drawer)
        {
            drawer.DrawSprite(slotBackground);
            drawer.DrawSprite(itemSprite);
        }

        public override void RenderText()
        {
            itemName.render();
            itemDescription.render();
            itemAmount.render();
        }

        public override void Dispose()
        {
            slotBackground.Dispose();
            itemSprite.Dispose();
            itemName.Dispose();
            itemDescription.Dispose();
            itemAmount.Dispose();
        }

        public override void OnClick(Player clicker)
        {
            clicker.Inventory.GetItem(itemSample.ID).Use(clicker);
            menu.UpdateSlotDisplay();
        }
    }
}
