using System.Runtime.InteropServices;
using System.Text;


namespace Predvajalnik_v_CSharp
{
    class Playback
    {
        private string command; //command
        [DllImport("winmm.dll")] // v program vključimo DLL za delo z multimedijo
        private static extern long mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, int hwdCallback); //funkcija za delo multimedijo
                                                                                                                                            //sklicuje se na winmm.dll
        public void open_song(string file)
        {
            command = "open \"" + file + "\" type MPEGVideo alias MUSIC";
            mciSendString(command, null, 0, 0);
        }//funkcija odpre skladbo za predvajanje
        public void play_song()
        {
            command = "play MUSIC";
            mciSendString(command, null, 0, 0);
        }//funkcija začne predvajati skladbo
        public void stop_music()
        {
            command = "stop MUSIC";
            mciSendString(command, null, 0, 0);
            command = "close MUSIC";
            mciSendString(command, null, 0, 0);
        }//funkcija ustavi predvajanje skladbe
        public void pause_song()
        {
            command = "pause MUSIC";
            mciSendString(command, null, 0, 0);
        }
        public void ponovi()
        {
            command = "seek MUSIC to start";
            mciSendString(command, null, 0, 0);
            play_song();
        }
        public void isci(int cas)
        {
            command = "seek MUSIC to " + cas;
            mciSendString(command, null, 0, 0);
            play_song();
        }
    }
}
