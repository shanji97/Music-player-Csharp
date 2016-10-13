using System.Runtime.InteropServices;
using System.Text;

namespace Predvajalnik_v_CSharp
{
    class Predvajanje
    {
        private string ukaz;
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, int hwdCallback);
        
        public void open_mp3(string datoteka)
        {
            ukaz="open \""+datoteka+"\" type MPEGVideo alias MojMp3";
            mciSendString(ukaz, null, 0, 0);
        }
                
        public void play_mp3()
        {
            ukaz = "play MojMp3";
            mciSendString(ukaz, null, 0, 0);
        }
        public void stop_mp3()
        {
            ukaz = "stop MojMp3";
            mciSendString(ukaz, null, 0, 0);
            ukaz = "close MojMp3";
            mciSendString(ukaz, null, 0, 0);
        }
    }
}
