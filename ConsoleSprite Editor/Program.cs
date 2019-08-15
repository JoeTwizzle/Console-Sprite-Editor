using System;
using System.IO;
using System.Threading;
using TGE;

namespace ConsoleSprite_Editor
{
    class Program
    {
        public static ConsoleSprite sprite;
        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                string text = File.ReadAllText(args[0]);
                text = text.Replace("~", "~\r\n");
                sprite = ConsoleSprite.Load(args[0], true);
            }

            //sprite = ConsoleSprite.Load(@"F:\Projekte\C#\ConsoleSprite Editor\ConsoleSprite Editor\bin\Release\netcoreapp2.2\publish\Sprites\Test.cpr", true);
            MyGame g = new MyGame();
            g.Run(new Display(DisplayType.Console, "Lucida Console", 128, 88, 8,8));
        }
    }
    class MyGame : Game
    {
        static GameState NextState;
        static GameState ActiveState;

        public static void ChangeStates(GameState state)
        {
            NextState = state;
        }
        public override void Initialize()
        {
            if (Program.sprite == null)
            {
                ActiveState = new InitialState(this, "Initial State");
            }
            else
            {
                ActiveState = new DefaultState(this, "Default State", Program.sprite);
            }

            ActiveState.Parent = this;
            ActiveState.Initialize();
        }

        public override void Start()
        {
            Console.Title = ActiveState.Name;
            ActiveState.Start();
        }

        public override void Update()
        {
            if (NextState != null)
            {
                Console.Title = NextState.Name;
                ActiveState = NextState;
                NextState = null;
                ActiveState.Initialize();
                ActiveState.Start();
            }
            ActiveState.Update();
        }

        public override void LateUpdate()
        {
            ActiveState.LateUpdate();
        }

        public override void Draw()
        {
            ActiveState.Draw();
        }
    }
}
