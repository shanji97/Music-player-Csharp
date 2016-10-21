using System.Runtime.InteropServices;
using System.Text;
//This class handles the audio playback 
namespace Predvajalnik_v_CSharp
{
    class Playback
    {
        private string command; //command
        [DllImport("winmm.dll")] // import the multimedija dll
        private static extern long mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, int hwdCallback); 

        public void open_audio_file(string datoteka)
        {
            command = "open \"" + datoteka + "\" type MPEGVideo alias MUSIC";
            mciSendString(command, null, 0, 0);
        }

        public void play()
        {
            command= "play MUSIC";
            mciSendString(command, null, 0, 0);
        }
        public void stop()
        {
          command= "stop MUSIC";
            mciSendString(command, null, 0, 0);
            command = "close MUSIC";
            mciSendString(command, null, 0, 0);
        }//funkcija ustavi predvajanje skladbe
        public void ponovi()
        {
           command= "seek MUSIC to start";
            mciSendString(command, null, 0, 0);
            play();
        }
        public void isci(int cas)
        {
            command = "seek MUSIC to " + cas;
            mciSendString(command, null, 0, 0);
            play();
        }
    }
}
