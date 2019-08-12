using System;
using TGE;

namespace ConsoleSprite_Editor
{
    class Program
    {
        static void Main(string[] args)
        {
            MyGame g = new MyGame();
            g.Run(new Display(DisplayType.Console, "terminal", 120, 80, 8, 8));
        }
    }
    /*   H        H       I           !
     *   H        H       I           !
     *   HHHHHHHHHH       I           !
     *   H        H       I           !
     *   H        H       I           
     *   H        H       I           !
     */
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
            ActiveState = new InitialState(this, "Initial State");
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
