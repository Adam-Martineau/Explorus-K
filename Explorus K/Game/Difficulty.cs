using Explorus_K.Controllers;
using Explorus_K.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Game
{
    public enum Difficulties
    {
        EASY = 0,
        NORMAL = 2,
        EXPERT = 4,
        IMPOSSIBLE = 6
    }

    public class GameDifficulty
    {
        private Difficulties difficulty;
        private static Bitmap[] difficultiesBitmap = new Bitmap[] { Resources.easy_noir, Resources.easy_bleu, Resources.normal_noir, Resources.normal_bleu, Resources.expert_noir, Resources.expert_bleu, Resources.impossible_noir, Resources.impossible_bleu };
        private int bubbleTimer;
        private int playerSpeed;
        private int slimusLives;
        private int toxicLives;

        public GameDifficulty()
        {
            this.difficulty = Difficulties.NORMAL;
            bubbleTimer = Constant.BUBBLE_TIMER_NORMAL;
            slimusLives = Constant.SLIME_LIVES_NORMAL;
            toxicLives = Constant.TOXIC_SLIME_LIVES_NORMAL;
            playerSpeed = Constant.PLAYER_SPEED_NORMAL;
        }

        public Difficulties Difficulty { get => difficulty; set => difficulty = value; }

        public void changeDifficulty()
        {
            switch (difficulty)
            {
                case Difficulties.EASY:
                    difficulty = Difficulties.NORMAL;
                    bubbleTimer = Constant.BUBBLE_TIMER_NORMAL;
                    slimusLives = Constant.SLIME_LIVES_NORMAL;
                    toxicLives = Constant.TOXIC_SLIME_LIVES_NORMAL;
                    playerSpeed = Constant.PLAYER_SPEED_NORMAL;
                    break;
                case Difficulties.NORMAL:
                    difficulty = Difficulties.EXPERT;
                    bubbleTimer = Constant.BUBBLE_TIMER_EXPERT;
                    slimusLives = Constant.SLIME_LIVES_EXPERT;
                    toxicLives = Constant.TOXIC_SLIME_LIVES_EXPERT;
                    playerSpeed = Constant.PLAYER_SPEED_EXPERT;
                    break;
                case Difficulties.EXPERT:
                    difficulty = Difficulties.IMPOSSIBLE;
                    bubbleTimer = Constant.BUBBLE_TIMER_IMPOSSIBLE;
                    slimusLives = Constant.SLIME_LIVES_IMPOSSIBLE;
                    toxicLives = Constant.TOXIC_SLIME_LIVES_IMPOSSIBLE;
                    playerSpeed = Constant.PLAYER_SPEED_IMPOSSIBLE;
                    break;
                case Difficulties.IMPOSSIBLE:
                    difficulty = Difficulties.EASY;
                    bubbleTimer = Constant.BUBBLE_TIMER_EASY;
                    slimusLives = Constant.SLIME_LIVES_EASY;
                    toxicLives = Constant.TOXIC_SLIME_LIVES_EASY;
                    playerSpeed = Constant.PLAYER_SPEED_EASY;
                    break;
                default:
                    break;
            }
        }

        public int getBubbleTimer()
        {
            return bubbleTimer;
        }

        public int getSlimusLives()
        {
            return slimusLives;
        }

        public int getToxicLives()
        {
            return toxicLives;
        }

        public int getPlayerSpeed()
        {
            return playerSpeed;
        }

        public Bitmap getBitmap()
        {
            return difficultiesBitmap[(int)difficulty];
        }

        public Bitmap getSelectedBitmap()
        {
            return difficultiesBitmap[(int)difficulty+1];
        }
    }
}
