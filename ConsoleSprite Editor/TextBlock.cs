using System;
using System.Collections.Generic;
using System.Text;
using TGE;

namespace ConsoleSprite_Editor
{
    class TextBlock : Panel
    {
        public short TextColor;
        public string Text;

        public TextBlock(int PosX, int PosY, string Text = "", short TextColor = 15) : base(PosX, PosY, 0, 0)
        {
            this.Clickable = true;
            this.Text = Text;
            this.TextColor = TextColor;
            Update();
        }

        public override void Draw(Display screen)
        {
            if (Text.Length == 0)
            {
                return;
            }
            screen.FillRectangle(Inside, PosX, PosY, Width, Height, InsideColor);
            screen.DrawRectangle(Border, PosX, PosY, Width, Height, BorderColor);
            screen.Draw(Text, PosX + 1, PosY + 1, TextColor);
        }

        public override void Update()
        {
            this.Width = Text.Length + 1;
            this.Height = 2;
        }
    }
}
