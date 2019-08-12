using System;
using System.Collections.Generic;
using System.Text;
using TGE;

namespace ConsoleSprite_Editor
{
    class ColorWindow : UiComponent
    {
        public Panel BG;
        public UpDown R;
        public UpDown G;
        public UpDown B;
        public TextBlock ResetButton;
        public short color;
        UpDown[] upDowns = new UpDown[3];
        public ColorWindow(int PosX, int PosY) : base(PosX, PosY, 16, 16)
        {
            ResetButton = new TextBlock(PosX + 19, PosY + 2, "Reset");
            BG = new Panel(PosX, PosY, 26, 5) { BorderColor=7, InsideColor = 15 };
            R = new UpDown(PosX + 1, PosY + 2) { BorderColor = 15, MaxValue = 255 };
            G = new UpDown(PosX + 7, PosY + 2) { BorderColor = 15, MaxValue = 255 };
            B = new UpDown(PosX + 13, PosY + 2) { BorderColor = 15, MaxValue = 255 };
            upDowns[0] = R;
            upDowns[1] = G;
            upDowns[2] = B;
        }

        public override void Draw(Display screen)
        {
            BG.Draw(screen);
            screen.DrawLine('█', PosX + 1, PosY + 1, PosX + BG.Width - 1, PosY + 1, BG.InsideColor);
            screen.Draw("R:", PosX + 3, PosY + 1, (short)(12 | (BG.InsideColor << 4)));
            screen.Draw("G:", PosX + 9, PosY + 1, (short)(10 | (BG.InsideColor << 4)));
            screen.Draw("B:", PosX + 15, PosY + 1, (short)(9 | (BG.InsideColor << 4)));
            R.Draw(screen);
            G.Draw(screen);
            B.Draw(screen);
            ResetButton.Draw(screen);
        }

        int cont = 0;
        int freq = 3;
        public override void Update()
        {
            var mouse = Input.GetMouse();
            if (ResetButton.IsInside(mouse.X, mouse.Y))
            {
                ResetButton.BorderColor = 12;
            }
            else
            {
                ResetButton.BorderColor = 8;
            }
            if (ResetButton.IsInside(mouse.X, mouse.Y) && mouse.IsLeftClick())
            {
                ResetButton.BorderColor = 10;

                R.Value = ColorChanger.StandardColors.colors[color].R;
                G.Value = ColorChanger.StandardColors.colors[color].G;
                B.Value = ColorChanger.StandardColors.colors[color].B;
            }
            cont--;
            for (int i = 0; i < upDowns.Length; i++)
            {
                if (upDowns[i].IsOnUp(mouse.X, mouse.Y) && mouse.IsLeftClick() && cont <= 0)
                {
                    upDowns[i].Increment();
                    cont = freq;
                }
                if (upDowns[i].IsOnDown(mouse.X, mouse.Y) && mouse.IsLeftClick() && cont <= 0)
                {
                    upDowns[i].Decrement();
                    cont = freq;
                }
            }
            ResetButton.Update();
            R.Update();
            G.Update();
            B.Update();
        }
    }
}
