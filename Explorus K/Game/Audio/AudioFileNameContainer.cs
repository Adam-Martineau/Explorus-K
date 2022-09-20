using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace Explorus_K.Game.Audio
{
    public class AudioFileNameContainer
    {
        private static AudioFileNameContainer instance = new AudioFileNameContainer();
        private Dictionary<AudioName, string> _audioFiles = new Dictionary<AudioName, string>();

        private AudioFileNameContainer()
        { 
            initContainer();
        }

        public string getFileName(AudioName audioName)
        {
            return _audioFiles[audioName];
        }

        public static AudioFileNameContainer getInstance()
        {
            return instance;
        }

        private void initContainer()
        {
            _audioFiles.Add(AudioName.BOOM, "boom");
            _audioFiles.Add(AudioName.BEEP_BOOP, "beepBoop");
            _audioFiles.Add(AudioName.BUBBLE_HIT, "bubbleHit");
            _audioFiles.Add(AudioName.GETTTING_COIN, "gettingCoin");
            _audioFiles.Add(AudioName.GETTING_HIT, "gettingHit");
            _audioFiles.Add(AudioName.MOVING, "moving");
            _audioFiles.Add(AudioName.SHOOTING_BUBBLE, "shootingBubble");
            _audioFiles.Add(AudioName.SOUND_2, "sound2");
            _audioFiles.Add(AudioName.SOUND_4, "sound4");
            _audioFiles.Add(AudioName.SOUND_5, "sound5");
            _audioFiles.Add(AudioName.SOUND_6, "sound6");
            _audioFiles.Add(AudioName.SOUND_7, "sound7");
            _audioFiles.Add(AudioName.SOUND_9, "sound9");
            _audioFiles.Add(AudioName.OPEN_DOOR, "sound10");
            _audioFiles.Add(AudioName.SOUND_11, "sound11");
            _audioFiles.Add(AudioName.SOUND_13, "sound13");
            _audioFiles.Add(AudioName.WINNING, "sound14");
            _audioFiles.Add(AudioName.SOUND_17, "sound17");
        }
    }
}
