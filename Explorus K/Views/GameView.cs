using Explorus_K.Controllers;
using Explorus_K.Game;
using Explorus_K.Models;
using Explorus_K.Properties;
using Explorus_K.Threads;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Reflection.Emit;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Application = System.Windows.Forms.Application;
using Size = System.Drawing.Size;
using Timer = System.Timers.Timer;
using TrackBar = System.Windows.Forms.TrackBar;

namespace Explorus_K.Views
{
    public class GameView
    {
        public static volatile GameForm gameForm;
        GameEngine gameEngine;

        RenderThread renderThread = new RenderThread();
        public static EventWaitHandle renderWaitHandle;

        string gameTitle;

		internal int largeSpriteDimension = 52;

        private PictureBox gameMenu = new PictureBox();
        private PictureBox gameHeader = new PictureBox();
		private PictureBox gameLabyrinth = new PictureBox();

        private PrivateFontCollection pfc = new PrivateFontCollection();

        private int screenWidth = 1000;
		private int screenHeight = 1000;
		private int menuHeight = 250;

        private static Timer resumeTimer;
        private static Timer replayTimer;
        private int countdown = 3;
        private int replayCountdown = 5;
        private MenuOptions menuOptions;

        public LabyrinthImage labyrinthImage;		

		public GameView(GameEngine gameEngine, int level)
		{
			this.gameEngine = gameEngine;
			gameForm = new GameForm(this);
			gameForm.Size = new Size(screenWidth, screenHeight);
			gameForm.MinimumSize = new Size(1000, 1000);

            AddFontFromMemory();

            gameHeader.Dock = DockStyle.Top;
			gameLabyrinth.Dock = DockStyle.Fill;
            gameMenu.Dock = DockStyle.Fill;

            gameMenu.Paint += new PaintEventHandler(this.MenuRenderer);
            gameHeader.Paint += new PaintEventHandler(this.HeaderRenderer);
			gameLabyrinth.Paint += new PaintEventHandler(this.LabyrinthRenderer);

            gameForm.Controls.Add(gameMenu);
            gameForm.Controls.Add(gameHeader);
            gameForm.Controls.Add(gameLabyrinth);

            gameHeader.Visible = false;
            gameLabyrinth.Visible = false;

            SetMainMenu();

            labyrinthImage = new LabyrinthImage(gameEngine.GetLabyrinth(), gameEngine.getBubbleManager(), gameEngine.GameDifficulty);

            resumeTimer = new Timer(1000);
            resumeTimer.Elapsed += OnTimedEventResume;

            replayTimer = new Timer(1000);
            replayTimer.Elapsed += OnTimedEventReplay;

            gameForm.UpdateLevel("Level " + level.ToString(), Color.Red);

            renderWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            Thread thread = new Thread(new ThreadStart(() => renderThread.startThread()));
            thread.Start();

            resize();
		}

		public void Show() 
		{
			Application.Run(gameForm); 
		}

        public void Render()
		{
            renderWaitHandle.Set();
        }

        public void Restart(GameEngine gameEngine, int level)
		{
			gameForm.UpdateLevel("Level " + level.ToString(), Color.Red);
            this.gameEngine = gameEngine;
            labyrinthImage = new LabyrinthImage(gameEngine.GetLabyrinth(), gameEngine.getBubbleManager(), gameEngine.GameDifficulty);
            resize();
            replayCountdown = 5;
        }

        public void Close()
		{
			if (gameForm.Visible)
				gameForm.BeginInvoke((MethodInvoker)delegate {
					gameForm.Close();
				});
		}

		public void resize()
		{
			screenWidth = gameForm.Width;
            screenHeight = gameForm.Height;
            if (labyrinthImage != null)
			{
                labyrinthImage.resize(gameForm);
            }
		}

		public void Update(bool show_fps, double fps)
		{
			if (show_fps)
			{
                gameTitle = "Explorus-K - FPS " + Math.Round(fps, 1).ToString();
            }
			else
			{
                gameTitle = "Explorus-K";
            }
        }

        private void SetMainMenu()
        {
            menuOptions = new MenuOptions();
            if (gameEngine.State == GameState.MENU)
            {
                menuOptions.addOption(MenuCursor.START_GAME, new Bitmap[] { Resources.startgame_noir, Resources.startgame_bleu });
            }
            else
            {
                menuOptions.addOption(MenuCursor.RESUME, new Bitmap[] { Resources.resume_noir, Resources.resume_bleu });
            }
            
            menuOptions.addOption(MenuCursor.AUDIO, new Bitmap[] { Resources.audio_noir, Resources.audio_bleu });
            if (gameEngine.State == GameState.MENU)
            {
                menuOptions.addOption(MenuCursor.DIFFICULTY, new Bitmap[] { Resources.difficulty_noir, Resources.difficulty_bleu });
            }
            menuOptions.addOption(MenuCursor.EXIT_GAME, new Bitmap[] { Resources.exitgame_noir, Resources.exitgame_bleu });
        }

        private void SetAudioMenu()
        {
            menuOptions = new MenuOptions();
            menuOptions.addOption(MenuCursor.MUSIC_VOLUME, new Bitmap[] { Resources.music_volume_noir, Resources.music_volume_bleu });
            menuOptions.addOption(MenuCursor.SOUND_VOLUME, new Bitmap[] { Resources.sound_volume_noir, Resources.sound_volume_bleu });
            menuOptions.addOption(MenuCursor.RETURN, new Bitmap[] { Resources.return_noir, Resources.return_bleu });
        }

        private void showText(Graphics g, string text)
        {
            StringFormat titleFormat = new StringFormat();
            titleFormat.Alignment = StringAlignment.Center;
            titleFormat.LineAlignment = StringAlignment.Center;
            Brush brush = new SolidBrush(Color.FromArgb(64, 255, 255, 255));

            Rectangle menu = new Rectangle(0, (screenHeight / 2) - (menuHeight / 2), screenWidth, menuHeight);
			g.DrawString(text, new Font("Arial", 80), Brushes.Red, menu, titleFormat);
            g.FillRectangle(brush, Rectangle.Round(menu));
        }

        private void MenuRenderer(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(Color.Black);

            int widthText = 300;
            int heightText = 50;
            int cursorDim = 75;

            g.DrawImage(Resources.Title_Bleu, (screenWidth / 2) - (Properties.Resources.Title.Width / 2), (screenHeight / 6) - (Properties.Resources.Title.Height / 2));

            for (int i = 0; i < menuOptions.getLength(); i++)
            {
                if (menuOptions.getCurrentIndex() == i)
                {
                    if (menuOptions.getCurrentType() == MenuCursor.DIFFICULTY)
                    {
                        g.DrawImage(Resources.cursor, (screenWidth / 2) - (widthText + 150), ((screenHeight / 6) * (i + 2)) - (cursorDim / 2), cursorDim, cursorDim);
                        g.DrawImage(menuOptions.getCurrentBitmap()[1], (screenWidth / 2) - (widthText + 25), ((screenHeight / 6) * (i + 2)) - (heightText / 2), widthText, heightText);
                        g.DrawString(": ", new Font(pfc.Families[0], 40, FontStyle.Regular), Brushes.White, (screenWidth / 2)-10, ((screenHeight / 6) * (i + 2)) - 40);
                        g.DrawImage(gameEngine.GameDifficulty.getSelectedBitmap(), (screenWidth / 2) + 50, ((screenHeight / 6) * (i + 2)) - (heightText / 2), widthText, heightText);
                    }
                    else
                    {
                        g.DrawImage(Resources.cursor, (screenWidth / 2) - widthText, ((screenHeight / 6) * (i + 2)) - (cursorDim / 2), cursorDim, cursorDim);
                        g.DrawImage(menuOptions.getCurrentBitmap()[1], (screenWidth / 2) - (widthText / 2), ((screenHeight / 6) * (i + 2)) - (heightText / 2), widthText, heightText);
                    }
                }
                else
                {
                    if (menuOptions.getCurrentType() == MenuCursor.DIFFICULTY)
                    {
                        g.DrawImage(menuOptions.getCurrentBitmap()[0], (screenWidth / 2) - (widthText + 25), ((screenHeight / 6) * (i + 2)) - (heightText / 2), widthText, heightText);
                        g.DrawString(": ", new Font(pfc.Families[0], 40, FontStyle.Regular), Brushes.White, (screenWidth / 2) - 10, ((screenHeight / 6) * (i + 2)) - 40);
                        g.DrawImage(gameEngine.GameDifficulty.getBitmap(), (screenWidth / 2) + 50, ((screenHeight / 6) * (i + 2)) - (heightText / 2), widthText, heightText);
                    }
                    else
                    {
                        g.DrawImage(menuOptions.getCurrentBitmap()[0], (screenWidth / 2) - (widthText / 2), ((screenHeight / 6) * (i + 2)) - (heightText / 2), widthText, heightText);
                    }
                }

                if (menuOptions.getCurrentType() == MenuCursor.MUSIC_VOLUME)
                {
                    g.DrawString(": ", new Font(pfc.Families[0], 40, FontStyle.Regular), Brushes.White, (screenWidth / 2) + (widthText / 2), ((screenHeight / 6) * (i + 2)) - 40);
                    if (gameEngine.MuteMusic)
                    {
                        g.DrawString("Mute", new Font(pfc.Families[0], 40, FontStyle.Regular), Brushes.Yellow, (screenWidth / 2) + (widthText / 2) + 30, ((screenHeight / 6) * (i + 2)) - 40);
                    }
                    else
                    {
                        g.DrawString(gameEngine.MusicVolume.ToString(), new Font(pfc.Families[0], 40, FontStyle.Regular), Brushes.Yellow, (screenWidth / 2) + (widthText / 2) + 30, ((screenHeight / 6) * (i + 2)) - 40);
                    }
                    
                }

                if (menuOptions.getCurrentType() == MenuCursor.SOUND_VOLUME)
                {
                    g.DrawString(": ", new Font(pfc.Families[0], 40, FontStyle.Regular), Brushes.White, (screenWidth / 2) + (widthText / 2), ((screenHeight / 6) * (i + 2)) - 40);
                    if (gameEngine.MuteSound)
                    {
                        g.DrawString("Mute", new Font(pfc.Families[0], 40, FontStyle.Regular), Brushes.Yellow, (screenWidth / 2) + (widthText / 2) + 30, ((screenHeight / 6) * (i + 2)) - 40);
                    }
                    else
                    {
                        g.DrawString(gameEngine.SoundVolume.ToString(), new Font(pfc.Families[0], 40, FontStyle.Regular), Brushes.Yellow, (screenWidth / 2) + (widthText / 2) + 30, ((screenHeight / 6) * (i + 2)) - 40);
                    }
                    
                }
            }

            gameForm.UpdateStatus(gameForm, gameEngine.State.ToString(), Color.Red);
        }

        private void HeaderRenderer(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.Clear(Color.Black);
			gameForm.Text = gameTitle;

			labyrinthImage.drawHeader(g);
		}

		private void LabyrinthRenderer(object sender, PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.Clear(Color.Black);

			labyrinthImage.drawLabyrinthImage(g);

            if (gameEngine.State == GameState.RESUME)
			{
                showText(g, countdown.ToString());
			}
			else if (gameEngine.State == GameState.STOP)
			{
                showText(g, "YOU DIED");
            }
            else if (gameEngine.State == GameState.RESTART)
            {
                showText(g, "YOU WIN");
            }
            else if (gameEngine.State == GameState.REPLAY)
            {
                StringFormat titleFormat = new StringFormat();
                titleFormat.Alignment = StringAlignment.Center;
                titleFormat.LineAlignment = StringAlignment.Center;
                Brush brush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));

                Point p = labyrinthImage.getLabyrinthPosition();
                int rectangleWidth = (Constant.LARGE_SPRITE_DIMENSION * 6) - 20;
                int rectangleHeight = Constant.LARGE_SPRITE_DIMENSION - 20;
                Rectangle replay = new Rectangle(p.X + ((Constant.LARGE_SPRITE_DIMENSION * 6) / 2)-(rectangleWidth / 2), (p.Y + (Constant.LARGE_SPRITE_DIMENSION) / 2) - (rectangleHeight / 2), rectangleWidth, rectangleHeight);
                g.FillRectangle(brush, Rectangle.Round(replay));
                g.DrawString("REPLAY - " + replayCountdown.ToString() + " SECONDS", new Font("Arial", 15, FontStyle.Bold), Brushes.White, replay, titleFormat);
            }

            gameForm.UpdateStatus(gameForm, gameEngine.State.ToString(), Color.Red);
        }

        public void InitializeHeaderBar(ProgressionBarCreator creator, int length, int current)
        {
            IBar bar = creator.InitializeBar(length, current);

            if (creator.GetType() == typeof(HealthBarCreator))
            {
                labyrinthImage.HealthBar = (HealthBar)bar;
            }
            else if (creator.GetType() == typeof(BubbleBarCreator))
            {
                labyrinthImage.BubbleBar = (BubbleBar)bar;
            }
            else if (creator.GetType() == typeof(GemBarCreator))
            {
                labyrinthImage.GemBar = (GemBar)bar;
            }
        }

        public Player getSlimusObject()
		{
			return labyrinthImage.getSlimus();
		}
		public BubbleBar getBubbleBarObject()
        {
            return labyrinthImage.BubbleBar;
        }        

        internal void ReceiveKeyEvent(KeyEventArgs e)
        {
			gameEngine.KeyEventHandler(e);
        }

		internal void LostFocus()
		{
			gameEngine.pause();
		}

		internal void GainFocus()
		{
            if (gameEngine.State != GameState.MENU)
            {
                gameEngine.resume();
            }
		}

		public void Resume()
		{
            gameForm.showControl(gameForm, gameMenu, false);
            gameForm.showControl(gameForm, gameHeader, true);
            gameForm.showControl(gameForm, gameLabyrinth, true);
            resumeTimer.Start();
            countdown = 3;
        }

        public void Pause()
        {
            SetMainMenu();
            gameForm.showControl(gameForm, gameMenu, true);
            gameForm.showControl(gameForm, gameHeader, false);
            gameForm.showControl(gameForm, gameLabyrinth, false);
            resumeTimer.Stop();
        }

        public void Replay()
        {
            labyrinthImage.slimus.setInvincible(false);
            replayCountdown = 5;
            replayTimer.Start();
        }

        private void OnTimedEventResume(Object source, ElapsedEventArgs e)
        {
            countdown -= 1;
            if (countdown == 0)
            {
                resumeTimer.Stop();
                gameEngine.play();
            }
        }

        private void OnTimedEventReplay(Object source, ElapsedEventArgs e)
        {
            replayCountdown -= 1;
            if (replayCountdown == 0)
            {
                replayTimer.Stop();
            }
        }

        public LabyrinthImage getLabyrinthImage()
		{
			return labyrinthImage;
		}

        public void selectMenu()
        {
            switch (menuOptions.getCurrentType())
            {
                case MenuCursor.START_GAME:
                    gameEngine.resume();
                    break;
                case MenuCursor.EXIT_GAME:
                    Application.Exit();
                    break;
                case MenuCursor.AUDIO:
                    SetAudioMenu();
                    break;
                case MenuCursor.RESUME:
                    gameEngine.resume();
                    break;
                case MenuCursor.DIFFICULTY:
                    gameEngine.changeDifficulty();
                    labyrinthImage.setBubbleTimerInterval(gameEngine.GameDifficulty.getBubbleTimer());
                    labyrinthImage.setSlimusLives(gameEngine.GameDifficulty.getSlimusLives());
                    labyrinthImage.setToxicLives(gameEngine.GameDifficulty.getToxicLives());
                    break;
                case MenuCursor.RETURN:
                    SetMainMenu();
                    break;
                default:
                    break;
            }
        }

        public void volumeDown()
        {
            switch (menuOptions.getCurrentType())
            {
                case MenuCursor.MUSIC_VOLUME:
                    gameEngine.downMusicVolume();
                    break;
                case MenuCursor.SOUND_VOLUME:
                    gameEngine.downSoundVolume();
                    break;
                default:
                    break;
            }
        }

        public void volumeUp()
        {
            switch (menuOptions.getCurrentType())
            {
                case MenuCursor.MUSIC_VOLUME:
                    gameEngine.upMusicVolume();
                    break;
                case MenuCursor.SOUND_VOLUME:
                    gameEngine.upSoundVolume();
                    break;
                default:
                    break;
            }
        }

        public void mutevolume()
        {
            switch (menuOptions.getCurrentType())
            {
                case MenuCursor.MUSIC_VOLUME:
                    gameEngine.muteMusicVolume();
                    break;
                case MenuCursor.SOUND_VOLUME:
                    gameEngine.muteSoundVolume();
                    break;
                default:
                    break;
            }
        }

        private void AddFontFromMemory()
        {
            byte[] fontdata = Resources.numberFontMenu;

            unsafe
            {
                fixed (byte* pFontData = fontdata)
                {
                    pfc.AddMemoryFont((IntPtr)pFontData, fontdata.Length);
                }
            }
        }
    }
}
