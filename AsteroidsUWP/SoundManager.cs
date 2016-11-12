using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AsteroidsUWP
{
    public class SoundManager
    {
        //SoundPlayer _freeLiveSound;
        bool _loaded = false;

        private void Load()
        {
            if (_loaded)
                return;

            //_freeLiveSound = new SoundPlayer("fanfare_x.wav");
        }

        public void PlayFreeLiveSound()
        {
            Load();

            //_freeLiveSound.Play();
        }
    }
}
