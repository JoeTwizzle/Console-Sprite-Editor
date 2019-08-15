using System;
using System.Collections.Generic;
using System.Text;
using TGE;

namespace ConsoleSprite_Editor
{
    class SaveDialog : UiComponent
    {
        public TextField NameField;
        public TextBlock SaveButton;
        public SaveDialog(int PosX, int PosY) : base(PosX, PosY, 0, 0)
        {
            NameField = new TextField(PosX + 1, PosY + 1) { BorderColor = 8 };
            SaveButton = new TextBlock(PosX + NameField.TextDisplayLength + 2, PosY + 1, "Save");
        }

        public override void Draw(Display screen)
        {
            screen.DrawRectangle('█', PosX-1, PosY-1, NameField.TextDisplayLength + SaveButton.Width + 5, 6, 8);
            screen.FillRectangle('█', PosX, PosY, NameField.TextDisplayLength + SaveButton.Width + 4, 5, 7);
            NameField.Draw(screen);
            SaveButton.Draw(screen);
        }

        public override void Update()
        {
            NameField.Update();
            SaveButton.Update();
        }
    }
}
