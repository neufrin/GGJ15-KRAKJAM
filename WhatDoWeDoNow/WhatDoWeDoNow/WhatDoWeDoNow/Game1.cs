using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using WhatDoWeDoNow.ScreenManager;
using WhatDoWeDoNow.Screens;
using WhatDoWeDoNow.Screens.MainScreen;
using System.IO;

namespace WhatDoWeDoNow
{
    public enum PLAYER_ENTER_FROM
    {
        Left,
        Right,
        Up,
        Down
    };
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static int MinXPosition = 120;
        public static int MinYPosition = 95;
        public static int MaxXPosition = 880;
        public static int MaxYPosition = 670;
        private Song BGM;
        public float Vitality = 100;
        public static LifeTimer timer;
        public static PLAYER_ENTER_FROM PlayerEnterFrom;
        private Texture2D ramytex;
        private Vector2 ramyrec;
        private Texture2D Vitalitytex;
        private Rectangle Vitalityrec;
        private Rectangle Vitalityrec2;
        private bool win = false;
        private bool loss = false;
        private Texture2D winTexture2D;
        private Texture2D lossTexture2D;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 786;
            graphics.PreferredBackBufferWidth = 1366;
            graphics.IsFullScreen = false;
            IsMouseVisible = true;
            PlayerEnterFrom = PLAYER_ENTER_FROM.Left;
        }
        private List<string[]> Zadania;
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Zadania = new List<string[]>();
            using (var stream = TitleContainer.OpenStream(@"Zadania.txt"))
            {
                using (var reader = new StreamReader(stream))
                {
                    while (reader.Peek() >= 0)
                        Zadania.Add(reader.ReadLine().Split(';'));
                }
            }

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"Zadania.txt"))
            {
                foreach (string[] line in Zadania)
                {

                    file.WriteLine("null;" + line[1] + ";" + line[2] + ";" + line[3] + ";" + line[4] + ";" + line[5] + ";" + line[6] + ";" + line[7] + ";" + line[8]);

                }
            }
            SCREEN_MANAGER.add_screen(new Screen1(GraphicsDevice, Content));
            SCREEN_MANAGER.add_screen(new Screen2(GraphicsDevice, Content));
            SCREEN_MANAGER.add_screen(new MainGameScreen(GraphicsDevice, Content));
            SCREEN_MANAGER.add_screen(new Room1(GraphicsDevice, Content));
            SCREEN_MANAGER.add_screen(new Room2(GraphicsDevice, Content));
            SCREEN_MANAGER.add_screen(new Room3(GraphicsDevice, Content));
            SCREEN_MANAGER.add_screen(new Room4(GraphicsDevice, Content));
            SCREEN_MANAGER.goto_screen("MainGame");

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ramytex = Content.Load<Texture2D>("Ramy");
            ramyrec = new Vector2(1060, 600);
            Vitalitytex = Content.Load<Texture2D>("Vitality");
            Vitalityrec = new Rectangle(1060, 650, Vitalitytex.Width, Vitalitytex.Height);
            BGM = Content.Load<Song>("MyVeryOwnDeadShip");
            MediaPlayer.Play(BGM);
            MediaPlayer.IsRepeating = true;
            Vitalityrec2 = new Rectangle(0, 0, Vitalitytex.Width, Vitalitytex.Height);
            SCREEN_MANAGER.Init();
            timer = new LifeTimer();
            lossTexture2D = Content.Load<Texture2D>("loose");
            winTexture2D = Content.Load<Texture2D>("win");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            if (!win && !loss)
            {
                SCREEN_MANAGER.Update(gameTime);
                timer.Update(gameTime);
                Vitality = timer.currentLevel / 1000;
                Vitalityrec = new Rectangle(1060, 650, Vitalitytex.Width * (int)Vitality / 100, Vitalitytex.Height);
                Vitalityrec2 = new Rectangle(0, 0, Vitalitytex.Width * (int)Vitality / 100, Vitalitytex.Height);
                int wincounter = 0;
                foreach (var room in SCREEN_MANAGER.getList())
                {
                    if (room.IsDone)
                    {
                        wincounter++;
                    }

                }
                if (wincounter >= 4 && timer.currentLevel > 0)
                {
                    win = true;
                }
                if (timer.currentLevel <= 0)
                {
                    loss = true;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        int licznik = 0;
        protected override void Draw(GameTime gameTime)
        {
            SCREEN_MANAGER.Draw(gameTime);
            spriteBatch.Begin();
            //    spriteBatch.Draw(ramytex,ramyrec,Color.White);
            licznik++;
            if (licznik < 10)
                spriteBatch.Draw(Vitalitytex, Vitalityrec, Vitalityrec2, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipVertically, 0f);
            else if (licznik < 20)
                spriteBatch.Draw(Vitalitytex, Vitalityrec, Vitalityrec2, Color.White);
            if (licznik == 19) licznik = 0;

            if (win)
            {
                spriteBatch.Draw(winTexture2D, new Vector2(200, 200), Color.White);
            }
            if (loss)
            {
                spriteBatch.Draw(lossTexture2D, new Vector2(200, 200), Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
