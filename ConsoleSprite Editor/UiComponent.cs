using System;
using System.Collections.Generic;
using TGE;

namespace ConsoleSprite_Editor
{
    abstract class UiComponent
    {
        public int PosX, PosY;
        public int Width, Height;
        public bool Visible;
        public bool Clickable;
        public UiComponent(int PosX, int PosY, int Width, int Height)
        {
            this.PosX = PosX;
            this.PosY = PosY;
            if (Width > 0 && Height > 0)
            {
                this.Width = Width;
                this.Height = Height;
            }
        }
        public virtual bool IsInside(int x, int y)
        {
            return (x >= PosX && x <= PosX + Width && y >= PosY && y <= PosY + Height);
        }
        public abstract void Draw(Display screen);
        public virtual void Update()
        {

        }
    }
}
