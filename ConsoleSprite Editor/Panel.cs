using System;
using System.Collections.Generic;
using System.Text;
using TGE;

namespace ConsoleSprite_Editor
{
    class Panel : UiComponent
    {
        public char Border = '█';
        public char Inside = '█';
        public short BorderColor = 15;
        public short InsideColor = -1;
        public Panel(int PosX, int PosY, int Width, int Height) : base(PosX, PosY, Width, Height)
        {

        }
        public override void Draw(Display screen)
        {
            screen.FillRectangle(Inside, PosX, PosY, Width, Height, InsideColor);
            screen.DrawRectangle(Border, PosX, PosY, Width, Height, BorderColor);
        }
    }
}
