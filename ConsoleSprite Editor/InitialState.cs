using System;
using System.Collections.Generic;
using System.Text;
using TGE;
using SharpDX.DirectInput;

namespace ConsoleSprite_Editor
{
    class InitialState : GameState
    {
        public InitialState(Game Parent, string Name) : base(Parent, Name)
        {

        }
        UpDown upDownX;
        UpDown upDownY;
        TextBlock okText;
        Panel panel;
        Panel BG;
        void Pass()
        {
            
            DefaultState state = new DefaultState(this.Parent, "Default State", new Tuple<int, int>(upDownX.Value, upDownY.Value));

            MyGame.ChangeStates(state);
        }

        public override void Initialize()
        {
            var xPos = Parent.Screen.Width / 2 - 9;
            var yPos = Parent.Screen.Height / 2;
            upDownX = new UpDown(xPos, yPos) { MaxValue = 64, TextColor = 7 << 4, BorderColor = 8 };
            upDownY = new UpDown(xPos + 9, yPos) { MaxValue = 64, TextColor = 7 << 4, BorderColor = 8 };
            okText = new TextBlock(xPos + 17, yPos, "Continue") { TextColor = 7 << 4, BorderColor = 8 };
            BG = new Panel(xPos - 2, yPos - 3, 30, 6) { InsideColor = 7, BorderColor = 8 };
            ColorChanger.SetColor(1, Color.FromArgb(200, 100, 0));
            panel = new Panel(0, 0, Parent.Screen.Width - 1, Parent.Screen.Height - 1) { InsideColor = 1, BorderColor = 12 };
        }
        public override void Start()
        {
            upDownX.Value = 16;
            upDownY.Value = 16;
            okText.Update();
        }
        int cont = 0;
        int freq = 6;
        int tabIndex = 0;
        public override void Update()
        {
            upDownX.Update();
            upDownY.Update();
            var mouse = Input.GetMouse();
            cont--;
            if (((upDownX.IsOnUp(mouse.X, mouse.Y) && mouse.IsLeftClick()) || (tabIndex == 0 && Input.GetKey(Key.Add).Held)) && cont <= 0)
            {
                upDownX.Increment();
                cont = freq;
            }
            if (((upDownX.IsOnDown(mouse.X, mouse.Y) && mouse.IsLeftClick()) || (tabIndex == 0 && Input.GetKey(Key.Subtract).Held)) && cont <= 0)
            {
                upDownX.Decrement();
                cont = freq;
            }
            if (((upDownY.IsOnUp(mouse.X, mouse.Y) && mouse.IsLeftClick()) || (tabIndex == 1 && Input.GetKey(Key.Add).Held)) && cont <= 0)
            {
                upDownY.Increment();
                cont = freq;
            }
            if (((upDownY.IsOnDown(mouse.X, mouse.Y) && mouse.IsLeftClick()) || (tabIndex == 1 && Input.GetKey(Key.Subtract).Held)) && cont <= 0)
            {
                upDownY.Decrement();
                cont = freq;
            }

            if (upDownX.IsInside(mouse.X, mouse.Y) && mouse.IsLeftClick())
            {
                tabIndex = 0;
            }
            else if (upDownY.IsInside(mouse.X, mouse.Y) && mouse.IsLeftClick())
            {
                tabIndex = 1;
            }
            if (okText.IsInside(mouse.X, mouse.Y))
            {
                okText.BorderColor = 12;
            }
            else
            {
                okText.BorderColor = 8;
            }
            if (Input.GetKey(Key.Return).Pressed|| okText.IsInside(mouse.X, mouse.Y) && mouse.IsLeftClick())
            {
                okText.BorderColor = 10;
                Pass();
            }
            if (Input.GetKey(Key.Tab).Pressed)
            {
                if (tabIndex < 2)
                {
                    tabIndex++;
                }
                else
                {
                    tabIndex = 0;
                }
            }
            if (Input.GetKey(Key.Back).Pressed)
            {
                Environment.Exit(0);
            }
            upDownX.BorderColor = (short)((tabIndex == 0 ? 12 : 15));
            upDownY.BorderColor = (short)((tabIndex == 1 ? 12 : 15));
            okText.TextColor = (short)((tabIndex == 2 ? 12 : 15));
        }

        public override void LateUpdate()
        {

        }
        public override void Draw()
        {
            Parent.Screen.Clear();
            panel.Draw(Parent.Screen);
            BG.Draw(Parent.Screen);
            Parent.Screen.Draw("Sprite dimensions:", Parent.Screen.Width / 2 - 10, Parent.Screen.Height / 2 - 2, (short)(0 | (7 << 4)));
            upDownX.Draw(Parent.Screen);
            upDownY.Draw(Parent.Screen);
            var mouse = Input.GetMouse();
            okText.Draw(Parent.Screen);
            Parent.Screen.Draw('█', mouse.X, mouse.Y, 12);
            Parent.Screen.Print();
        }

    }
}
