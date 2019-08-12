using SharpDX.DirectInput;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System;
using TGE;

namespace ConsoleSprite_Editor
{
    class DefaultState : GameState
    {
        #region Variables
        enum State
        {
            Standard,
            colorWindow
        }
        static char[] cp437 = new char[]
        {
            '\0','☺','☻','♥','♦','♣','♠','•','◘','○','◙','♂','♀','♪','♫','☼',
            '►','◄','↕','‼','¶','§','▬','↨','↑','↓','→','←','∟','↔','▲','▼',
            ' ','!','"','#','$','%','&','\'','(',')','*','+',',','-','.','/',
            '0','1','2','3','4','5','6','7','8','9',':',';','<','=','>','?',
            '@','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O',
            'P','Q','R','S','T','U','V','W','X','Y','Z','[','\\',']','^','_',
            '`','a','b','c','d','e','f','g','h','i','j','k','l','m','n','o',
            'p','q','r','s','t','u','v','w','x','y','z','{','|','}','~','⌂',
            'Ç','ü','é','â','ä','à','å','ç','ê','ë','è','ï','î','ì','Ä','Å',
            'É','æ','Æ','ô','ö','ò','û','ù','ÿ','Ö','Ü','¢','£','¥','₧','ƒ',
            'á','í','ó','ú','ñ','Ñ','ª','º','¿','⌐','¬','½','¼','¡','«','»',
            '░','▒','▓','│','┤','╡','╢','╖','╕','╣','║','╗','╝','╜','╛','┐',
            '└','┴','┬','├','─','┼','╞','╟','╚','╔','╩','╦','╠','═','╬','╧',
            '╨','╤','╥','╙','╘','╒','╓','╫','╪','┘','┌','█','▄','▌','▐','▀',
            'α','ß','Γ','π','Σ','σ','µ','τ','Φ','Θ','Ω','δ','∞','ε','♫','∩',
            '≡','±','≥','≤','⌠','⌡','÷','≈','°','∙','·','√','ⁿ','²','■',' ',
        };
        List<Panel> components = new List<Panel>();
        MouseState mouse;
        ConsoleSprite activeSprite;
        Panel BGPanel;
        Panel palettePanel;
        TextBlock exportButton;
        TextBlock importButton;
        TextBlock sizeText;
        TextBlock scaleText;
        TextBlock debugText;
        TextBlock resetButton;
        UpDown colorValue;
        char characterValue = '█';
        ColorWindow colorWindow;
        State stateValue;
        int spriteX;
        int spriteY;
        int spriteWidth = 40;
        int spriteHeight = 40;
        float scale = 1f;
        short colorIndex;
        Color color;
        int cont = 0;
        const int freq = 8;
        #endregion

        public DefaultState(Game parent, string name, object Data) : base(parent, name)
        {
            spriteWidth = ((Tuple<int, int>)Data).Item1;
            spriteHeight = ((Tuple<int, int>)Data).Item2;
            spriteX = Parent.Screen.Width / 2 - spriteWidth;
            spriteY = Parent.Screen.Height / 3 - spriteHeight;
        }

        #region Start
        public override void Initialize()
        {
            var height = Parent.Screen.Height;
            var width = Parent.Screen.Width;
            colorWindow = new ColorWindow(width / 2 - 10, height / 3 - 10);
            resetButton = new TextBlock(width - width / 3, height - 3, "Reset") { BorderColor = 8 };
            sizeText = new TextBlock(width - width / 3, height - height / 3 + 1) { BorderColor = 7 };
            scaleText = new TextBlock(width - width / 3, height - height / 3 + 3) { BorderColor = 7 };
            debugText = new TextBlock(width - width / 3, height - height / 3 + 5) { BorderColor = 7 };
            BGPanel = new Panel(0, height - height / 3, width - 1, height) { BorderColor = 8, InsideColor = 7 };
            exportButton = new TextBlock(width - 31, height - 3, "Export palette") { BorderColor = 8 };
            importButton = new TextBlock(width - 31, height - 6, "Import palette") { BorderColor = 8 };
            palettePanel = new Panel(width - 12, height - 2, 7, 2);
            colorValue = new UpDown(width - width / 3, height - 6) { BorderColor = 8, MinValue = -1, MaxValue = 16 * 16, Value = 15 };
            components.Add(resetButton);
            components.Add(exportButton);
            components.Add(importButton);
            components.Add(palettePanel);
            components.Add(colorValue);
            mouse = Input.GetMouse();
        }
        public override void Start()
        {
            GenerateSprite();
            SetTexts();
        }
        void GenerateSprite()
        {
            activeSprite = new ConsoleSprite(spriteWidth, spriteHeight);
            var chars = new char[spriteWidth, spriteHeight];
            var colors = new short[spriteWidth, spriteHeight];
            activeSprite.SetData(chars, colors);
        }
        void SetTexts()
        {
            debugText.Text = $"X: {mouse.X} Y: {mouse.Y} Z: {mouse.Z}";
            scaleText.Text = $"Scale: {scale * scale}";
            sizeText.Text = $"Size: X:{ activeSprite.Width}, Y:{ activeSprite.Height}";
        }
        #endregion

        #region Updates
        public override void Update()
        {
            if (!Game.ApplicationIsActivated())
            {
                return;
            }
            if (cont <= 0)
            {
                cont = freq;
            }

            switch (stateValue)
            {
                case State.Standard:
                    SetTexts();
                    HandleInput();
                    UpdateUI();
                    if (debugText.IsInside(mouse.X, mouse.Y) && mouse.IsLeftClick())
                    {
                        debugText.BorderColor = 12;
                    }
                    else
                    {
                        debugText.BorderColor = 7;
                    }
                    return;
                case State.colorWindow:
                    ColorState();
                    return;
            }

        }
        void UpdateUI()
        {
            sizeText.Update();
            debugText.Update();
            scaleText.Update();
            colorValue.Update();
            colorValue.InsideColor = (short)(colorValue.Value >> 4);
            colorValue.TextColor = colorValue.Value;
            resetButton.Update();
            exportButton.Update();
            importButton.Update();

        }
        void ColorState()
        {
            if (Input.GetKey(Key.Return).Pressed)
            {
                stateValue = State.Standard;
                ColorChanger.SetColor(colorIndex, Color.FromArgb((byte)colorWindow.R.Value, (byte)colorWindow.G.Value, (byte)colorWindow.B.Value));
                colorWindow.R.Value = 0;
                colorWindow.G.Value = 0;
                colorWindow.B.Value = 0;
            }
            else
            {
                ColorChanger.SetColor(colorIndex, Color.FromArgb((byte)colorWindow.R.Value, (byte)colorWindow.G.Value, (byte)colorWindow.B.Value));
                if (Input.GetKey(Key.Escape).Pressed)
                {
                    stateValue = State.Standard;
                    ColorChanger.SetColor(colorIndex, Color.FromArgb(color.R, color.G, color.B));
                    colorWindow.R.Value = 0;
                    colorWindow.G.Value = 0;
                    colorWindow.B.Value = 0;
                }
                colorWindow.Update();
            }
        }
        void HandleInput()
        {
            var prevMouseX = mouse.X;
            var prevMouseY = mouse.Y;
            mouse = Input.GetMouse();
            if (Input.GetKey(Key.Back).Pressed)
            {
                MyGame.ChangeStates(new InitialState(Parent, "Initial State"));
            }
            if (mouse.IsRightClick())
            {
                var MouseXDelta = mouse.X - prevMouseX;
                var MouseYDelta = mouse.Y - prevMouseY;

                spriteX += MouseXDelta;
                spriteY += MouseYDelta;
            }
            if (Input.GetKey(Key.LeftShift).Held)
            {
                if (scale + mouse.Z * Parent.DeltaTime < 1)
                {
                    scale = 1f;
                }
                else if (scale + mouse.Z * Parent.DeltaTime > MathF.Sqrt(1.98f))
                {
                    scale = MathF.Sqrt(1.98f);
                }
                else
                {
                    scale += mouse.Z * Parent.DeltaTime;
                }
            }
            else
            {
                colorValue.Value += mouse.Z;
            }
            var x = (int)((mouse.X - spriteX) * (1f / (scale * scale)));
            var y = (int)((mouse.Y - spriteY) * (1f / (scale * scale)));//why the fuck does this work?!?!?!?!?

            if (mouse.IsLeftClick())
            {
                activeSprite.SetChar(x, y, characterValue);
                activeSprite.SetColor(x, y, (byte)colorValue.Value);
            }
            if (Input.GetKey(Key.L).Pressed)
            {
                activeSprite = ConsoleSprite.Load("Test");
            }
            if (Input.GetKey(Key.S).Pressed)
            {
                activeSprite.Save("Test");
            }
            cont--;
            if (colorValue.IsOnUp(mouse.X, mouse.Y) && mouse.IsLeftClick() && cont <= 0)
            {
                colorValue.Increment();
                cont = freq;
            }
            if (colorValue.IsOnDown(mouse.X, mouse.Y) && mouse.IsLeftClick() && cont <= 0)
            {
                colorValue.Decrement();
                cont = freq;
            }
            if (palettePanel.IsInside(mouse.X, mouse.Y) && mouse.IsLeftClick())
            {
                var xPos = Parent.Screen.Width - 12;
                var yPos = Parent.Screen.Height - 2;
                colorIndex = (short)(mouse.X - (xPos) + (8 * (mouse.Y - yPos)));
                color = ColorChanger.GetColor(colorIndex);
                colorWindow.color = colorIndex;
                colorWindow.R.Value = color.R;
                colorWindow.G.Value = color.G;
                colorWindow.B.Value = color.B;
                stateValue = State.colorWindow;
            }
            for (int i = 0; i < components.Count; i++)
            {
                if (components[i].IsInside(mouse.X, mouse.Y))
                {
                    components[i].BorderColor = 12;
                }
                else
                {
                    components[i].BorderColor = 8;
                }
                if (components[i].Clickable && components[i].IsInside(mouse.X, mouse.Y) && mouse.IsLeftClick())
                {
                    components[i].BorderColor = 10;
                }
            }
            if (resetButton.IsInside(mouse.X, mouse.Y))
            {
                resetButton.BorderColor = 12;
            }
            else
            {
                resetButton.BorderColor = 8;
            }
            if (resetButton.IsInside(mouse.X, mouse.Y) && mouse.IsLeftClick())
            {
                resetButton.BorderColor = 10;
                ColorChanger.SetColor(0, Color.FromArgb(0, 0, 0));
                for (int i = 0; i < 16; i++)
                {
                    ColorChanger.SetColor(i, ColorChanger.StandardColors.colors[i]);
                }
            }
            if (true)
            {

            }

        }
        public override void LateUpdate()
        {

        }
        #endregion

        #region Drawing
        public override void Draw()
        {
            var screen = Parent.Screen;


            screen.Clear();

            switch (stateValue)
            {
                case State.Standard:
                    if (activeSprite != null)
                    {
                        DrawBorder();
                    }
                    break;

                case State.colorWindow:
                    colorWindow.Draw(screen);
                    break;
            }

            DrawUI();
            DrawPalette();
            DrawMisc();
            if (mouse.IsMiddleClick())
            {
                if (mouse.Y < Parent.Screen.Height && mouse.Y >= 0 && mouse.X < Parent.Screen.Width && mouse.X >= 0)
                {
                    var data = ConsoleRendererExtensions.getCharAt((ConsoleRenderer)Parent.Screen, mouse.X, mouse.Y);
                    if (data.Item1=='█')
                    {
                        colorValue.Value = data.Item2 & 0x0F;
                    }
                    else
                    {
                        colorValue.Value = data.Item2;
                    }
                }
            }
            if (mouse.IsRightClick())
            {
                if (mouse.Y < Parent.Screen.Height && mouse.Y >= 0 && mouse.X < Parent.Screen.Width && mouse.X >= 0)
                {
                    var data = ConsoleRendererExtensions.getCharAt((ConsoleRenderer)Parent.Screen, mouse.X, mouse.Y);
                    characterValue = data.Item1;
                }
            }
            DrawCursor();
            screen.Print();
        }
        void DrawPalette()
        {
            var height = Parent.Screen.Height;
            var width = Parent.Screen.Width;
            var screen = Parent.Screen;
            var xPos = width - 14;
            var yPos = height - 3;
            screen.DrawRectangle('█', xPos - 1, yPos - 1, 15, 5, 8);
            screen.FillRectangle('█', xPos, yPos, 13, 3, 11);
            screen.Draw("Color palette", xPos, yPos, (short)(0 | (11 << 4)));
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    screen.Draw('█', xPos + 2 + j, yPos + 1 + i, (short)(j + (8 * i)));
                }
            }
        }
        void DrawUI()
        {
            var screen = Parent.Screen;
            var height = Parent.Screen.Height;
            var width = Parent.Screen.Width;
            BGPanel.Draw(screen);
            screen.FillRectangle('█', width - width / 3, height - 6, width, height, 8);
            sizeText.Draw(screen);
            scaleText.Draw(screen);
            debugText.Draw(screen);
            screen.DrawLine('█', width - width / 3, height - height / 3 + 1, width - width / 3, height, 8);
            colorValue.Draw(screen);
            resetButton.Draw(screen);
            exportButton.Draw(screen);
            importButton.Draw(screen);
        }
        void DrawBorder()
        {
            if (scale == 1f)
            {
                Parent.Screen.DrawRectangle('!', spriteX - 1, spriteY - 1, activeSprite.Width + 1, activeSprite.Height + 1, 12);
                ConsoleRendererExtensions.DrawConsoleSprite((ConsoleRenderer)Parent.Screen, activeSprite, spriteX, spriteY);
            }
            else
            {
                Parent.Screen.DrawRectangle('!', spriteX - 1, spriteY - 1, (int)(activeSprite.Width * scale * scale), (int)(activeSprite.Height * scale * scale), 12);
                ConsoleRendererExtensions.DrawConsoleSprite((ConsoleRenderer)Parent.Screen, activeSprite, spriteX, spriteY, scale, scale);
            }
        }
        void DrawMisc()
        {
            var height = Parent.Screen.Height;
            var width = Parent.Screen.Width;
            var screen = Parent.Screen;

            for (int i = 0; i < cp437.Length; i++)
            {
                screen.Draw(cp437[i], BGPanel.PosX + 1 + (i % 64), BGPanel.PosY + 1 + (i / 64), (short)(0 | (7 << 4)));
            }

        }
        void DrawCursor()
        {
            var height = Parent.Screen.Height;
            var width = Parent.Screen.Width;
            var screen = Parent.Screen;
            short col = 12;
            var data = ConsoleRendererExtensions.getCharAt((ConsoleRenderer)screen, mouse.X, mouse.Y);
            if (data != null)
            {
                col = (short)(data.Item2 * 2 + 5);
            }
            screen.Draw('▒', mouse.X, mouse.Y, col);
        }
        #endregion
    }
}
