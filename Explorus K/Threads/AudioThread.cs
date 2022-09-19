
using Explorus_K.Game.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Message = Explorus_K.Game.Audio.Message;
using System.Windows.Media;
using System.Resources;
using System.Media;
using System.Runtime.Remoting.Contexts;
using System.Runtime.CompilerServices;

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

        public void Process(Message msg)
        {
            Console.WriteLine("Got msg: " + msg.Content + " from " + msg.Author + " at " + msg.Date.ToString());
        }

        public void Run()
        {
            running_ = true;

            /*var p1 = new System.Windows.Media.MediaPlayer();
            p1.Open(new System.Uri(@"C:\Users\Mathieu\Documents\S8\APP1\Labo1\Explorus-K\Explorus-K\Explorus K\Resources\gameMusic.wav"));
            p1.Play();

            // this sleep is here just so you can distinguish the two sounds playing simultaneously
            //System.Threading.Thread.Sleep(1);

            var test = new System.Windows.Media.MediaPlayer();
            test.Open(new System.Uri(@"C:\Users\Mathieu\Documents\S8\APP1\Labo1\Explorus-K\Explorus-K\Explorus K\Resources\boom.wav"));
            test.Play();

            System.Threading.Thread.Sleep(3);

            var teste = new System.Windows.Media.MediaPlayer();
            teste.Open(new System.Uri(@"C:\Users\Mathieu\Documents\S8\APP1\Labo1\Explorus-K\Explorus-K\Explorus K\Resources\VERYLOUDCLAPPING.WAV"));
            teste.Play();*/


            initMediaPlayerList();

            //playMusic();

            Play("C:\\Users\\Mathieu\\Documents\\S8\\APP1\\Labo1\\Explorus-K\\Explorus-K\\Explorus K\\Resources\\VERYLOUDCLAPPING.WAV");

            Thread.Sleep(500);

            setSfxVolume(10);

            setMusicVolume(10);

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
                        List<Message> msgs = babillard_.GetMessages();
                        foreach (Message m in msgs)
                        {
                            Process(m);
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
            // MediaPlayer volume is a float value between 0 and 1.
            mediaPlayers[0].Volume = volume / 100.0f;
        }
        public void setSfxVolume(int volume)
        {
            for(int i = 1; i < mediaPlayers.Count; i++)
            {
                mediaPlayers[i].Volume = volume / 100.0f;    
            }
        }
        public void Play(string filename)
        {
            mediaPlayers[nextMediaPlayer].Open(new Uri(filename));
            mediaPlayers[nextMediaPlayer].Volume = sfxPlayerVolume;
            mediaPlayers[nextMediaPlayer].Play();
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
            mediaPlayers[0].Open(new System.Uri(@"C:\Users\Mathieu\Documents\S8\APP1\Labo1\Explorus-K\Explorus-K\Explorus K\Resources\gameMusic.wav"));
            mediaPlayers[0].Play();
        }

        private void changeNextMediaPlayer()
        {
            if(nextMediaPlayer < 5)
            {
                nextMediaPlayer++;
            }
            else
            {
                nextMediaPlayer = 1;
            }
        }
    }
}
