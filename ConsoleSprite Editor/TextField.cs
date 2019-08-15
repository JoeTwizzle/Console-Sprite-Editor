using System;
using System.Collections.Generic;
using System.Text;
using TGE;

namespace ConsoleSprite_Editor
{
    class TextField : TextBlock
    {
        public KeyboardInput KeyboardInput;
        public int TextDisplayLength = 30;
        
        public TextField(int PosX, int PosY, short TextColor = 15) : base(PosX, PosY, "", TextColor)
        {
            KeyboardInput = new KeyboardInput();
            this.Width = TextDisplayLength + 1;
            this.Height = 2;
        }

        public override void Draw(Display screen)
        {
            string txt;
            if (Text.Length > TextDisplayLength)
            {
                txt= Text.Substring(Text.Length - TextDisplayLength);
            }
            else
            {
                txt = Text;
            }

            screen.FillRectangle(Inside, PosX, PosY, Width, Height, InsideColor);
            screen.DrawRectangle(Border, PosX, PosY, Width, Height, BorderColor);
            screen.Draw(txt, PosX + 1, PosY + 1, TextColor);
        }

        public override void Update()
        {
            if (KeyboardInput != null)
            {
                KeyboardInput.Update();
                base.Text = KeyboardInput.Text;
            }
        }
    }
}
