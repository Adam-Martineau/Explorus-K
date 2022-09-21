
using Explorus_K.Game.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Resources;
using System.Media;
using System.Runtime.Remoting.Contexts;
using System.Runtime.CompilerServices;
using System.IO;
using System.Windows.Threading;
using Explorus_K.Game;

/*
 * Code FSP du thread Audio :
 
    AUDIO (N=4)   = AUDIO[N],
    AUDIO[i:0..N] = 
      ( when(i==4) playAudio[0] -> AUDIO[0] |
	    when(i==4) playAudio[1] -> AUDIO[1] |
	    when(i==4) playAudio[2] -> AUDIO[2] |
	    when(i==4) playAudio[3] -> AUDIO[3] |
	    when(i==4) playMusic -> MUSICPLAYER |
	    when(i < 3) nextMediaPlayer[i+1] -> MEDIABUFFER |
	    when(i == 3) nextMediaPlayer[0] -> MEDIABUFFER
      ),


    MEDIABUFFER = ( waitForEvent -> AUDIO),
    MUSICPLAYER = (stop -> AUDIO).
 */

namespace Explorus_K.Threads
{
    public class AudioThread : IListener
    {
        private bool running_;
        private AudioBabillard babillard_;

        private List<MediaPlayer> mediaPlayers = new List<MediaPlayer>();
        private int nextMediaPlayer = 1;
        //private double sfxPlayerVolume = 0.5;

        public AudioThread(AudioBabillard b)
        {
            babillard_ = b;
            b.RegisterListener(this);
        }

        public void Process((string,int) message)
        {
            if (message.Item2 == -1)
            {
                playAudio(message.Item1);
            }
            else if (message.Item1 == "SET_MUSIC")
            {
                setMusicVolume(message.Item2);
            }
            else if (message.Item1 == "SET_SOUND")
            {
                changeSoundVolume(message);
            }
        }

        public void Run()
        {
            running_ = true;

            initMediaPlayerList();

            setMusicVolume(Constant.MUSIC_VOLUME);
            setSfxVolume(Constant.SOUND_VOLUME);

            playMusic();

            while (running_)
            {
                lock (this)
                {
                    if (!babillard_.HasMessages())
                    {
                        Monitor.Wait(this, 1000);
                    }
                    else
                    {
                        List<(string, int)> messages = babillard_.GetMessages();
                        foreach ((string, int) message in messages)
                        {
                            Process(message);
                        }
                    }
                }
            }
        }

        public void Notify()
        {
            lock (this)
            {
                Monitor.Pulse(this);
            }
        }
        public void Stop()
        {
            running_ = false;
        }

        public void setMusicVolume(int volume)
        {
            mediaPlayers[0].Volume = volume / 100.0f;
        }

        public void changeSoundVolume((string, int) message)
        {
            setSfxVolume(message.Item2);
            playAudio(message.Item1);
        }

        public void setSfxVolume(int volume)
        {
            lock (this)
            {
                for (int i = 1; i < mediaPlayers.Count; i++)
                {
                    mediaPlayers[i].Volume = volume / 100.0f;
                }
            }
        }
        public void playAudio(string filename)
        {
            Console.WriteLine(filename);
            mediaPlayers[nextMediaPlayer].Open(new Uri(getResourceFilePath(filename)));
            //mediaPlayers[nextMediaPlayer].Volume = sfxPlayerVolume;
            mediaPlayers[nextMediaPlayer].Dispatcher.Invoke(() => mediaPlayers[nextMediaPlayer].Play());
            changeNextMediaPlayer();
        }

        private void initMediaPlayerList()
        {
            for(int i = 0; i < 5; i++)
            {
                mediaPlayers.Add(new MediaPlayer());
            }
        }

        private void playMusic()
        {
            mediaPlayers[0].Open(new System.Uri(Path.Combine(System.IO.Path.GetFullPath(@"..\..\"), "Resources") + "\\gameMusic.wav"));
            mediaPlayers[0].Dispatcher.Invoke(() => mediaPlayers[0].Play());
        }

        private void changeNextMediaPlayer()
        {
            if(nextMediaPlayer < 4)
            {
                nextMediaPlayer++;
            }
            else
            {
                nextMediaPlayer = 1;
            }
        }

        private string getResourceFilePath(string filename)
        {
            return Path.Combine(System.IO.Path.GetFullPath(@"..\..\"), "Resources") + "\\" + filename + ".wav";
        }
    }
}
