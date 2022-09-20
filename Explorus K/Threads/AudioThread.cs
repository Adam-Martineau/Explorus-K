
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

namespace Explorus_K.Threads
{
    public class AudioThread : IListener
    {
        private bool running_;
        private AudioBabillard babillard_;

        private List<MediaPlayer> mediaPlayers = new List<MediaPlayer>();
        private int nextMediaPlayer = 1;
        private double sfxPlayerVolume = 0.5;

        public AudioThread(AudioBabillard b)
        {
            babillard_ = b;
            b.RegisterListener(this);
        }

        public void Process(string filename)
        {
           playAudio(filename);
        }

        public void Run()
        {
            running_ = true;

            initMediaPlayerList();

            setMusicVolume(10);

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
                        List<string> fileNames = babillard_.GetMessages();
                        foreach (string fileName in fileNames)
                        {
                            Process(fileName);
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
        public void setSfxVolume(int volume)
        {
            for(int i = 1; i < mediaPlayers.Count; i++)
            {
                mediaPlayers[i].Volume = volume / 100.0f;    
            }
        }
        public void playAudio(string filename)
        {
            mediaPlayers[nextMediaPlayer].Open(new Uri(getResourceFilePath(filename)));
            mediaPlayers[nextMediaPlayer].Volume = sfxPlayerVolume;
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
