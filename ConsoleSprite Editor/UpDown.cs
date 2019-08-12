using System;
using System.Collections.Generic;
using System.Text;
using TGE;

namespace ConsoleSprite_Editor
{
    class UpDown : Panel
    {
        private int _value;
        
        public int Value
        {
            get { return _value; }
            set
            {
                if (value <= MaxValue && value >= MinValue)
                {
                    _value = value;
                }
                else if (value > MaxValue)
                {
                    _value = MaxValue;
                }
                else if (value < MinValue)
                {
                    _value = MinValue;
                }
            }
        }

        public int MaxValue = 100;
        public int MinValue = 0;
        public int TextColor = 15;
        public UpDown(int PosX, int PosY) : base(PosX, PosY, 3, 2)
        {
            InsideColor = 0;
        }

        public void Increment()
        {
            if (Value + 1 <= MaxValue)
            {
                Value++;
            }
        }
        public void Decrement()
        {
            if (Value - 1 >= MinValue)
            {
                Value--;
            }
        }

        public override void Update()
        {
            Width = MaxValue.ToString().Length + 1;
        }

        public bool IsOnUp(int x, int y)
        {
            return x == PosX + Width - 1 && y == PosY;
        }
        public bool IsOnDown(int x, int y)
        {
            return x == PosX + Width - 1 && y == PosY + 2;
        }

        public override void Draw(Display screen)
        {
            base.Draw(screen);
            screen.Draw('↑', PosX + Width - 1, PosY, (short)((BorderColor == 12 ? 15 : 12) | (BorderColor << 4)));
            screen.Draw('↓', PosX + Width - 1, PosY + 2, (short)((BorderColor == 12 ? 15 : 12) | (BorderColor << 4)));
            screen.Draw(Value.ToString(), PosX + 1, PosY + 1, (short)(TextColor | (InsideColor << 4)));
        }
    }
}
