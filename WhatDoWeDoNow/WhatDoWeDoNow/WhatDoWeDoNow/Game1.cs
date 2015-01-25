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
        public float Vitality= 100;
        private LifeTimer timer;
        public static PLAYER_ENTER_FROM PlayerEnterFrom;
        private Texture2D ramytex;
        private Vector2 ramyrec;
        private Texture2D Vitalitytex;
        private Rectangle Vitalityrec;
        private Rectangle Vitalityrec2;
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

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            SCREEN_MANAGER.add_screen(new Screen1(GraphicsDevice,Content));
            SCREEN_MANAGER.add_screen(new Screen2(GraphicsDevice,Content));
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
            ramyrec = new Vector2(1060,600);
            Vitalitytex = Content.Load<Texture2D>("Vitality");
            Vitalityrec = new Rectangle(1060, 650 ,Vitalitytex.Width,Vitalitytex.Height);
            Vitalityrec2 = new Rectangle(0, 0, Vitalitytex.Width,Vitalitytex.Height);
         //   BGM = Content.Load<Song>("MyVeryOwnDeadShip");
        //    MediaPlayer.Play(BGM);
          //  MediaPlayer.IsRepeating = true;
            SCREEN_MANAGER.Init();
            timer = new LifeTimer();
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
            if(Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            SCREEN_MANAGER.Update(gameTime);
            timer.Update(gameTime);
            Vitality = timer.currentLevel/1000;
            Vitalityrec = new Rectangle(1060, 650, Vitalitytex.Width * (int)Vitality/100, Vitalitytex.Height);
            Vitalityrec2 = new Rectangle(0, 0, Vitalitytex.Width * (int)Vitality / 100, Vitalitytex.Height);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>


        protected override void Draw(GameTime gameTime)
        {
            SCREEN_MANAGER.Draw(gameTime);
            spriteBatch.Begin();
        //    spriteBatch.Draw(ramytex,ramyrec,Color.White);
            spriteBatch.Draw(Vitalitytex, Vitalityrec, Vitalityrec2, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
