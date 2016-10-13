using System.Runtime.InteropServices;
using System.Text;


namespace Predvajalnik_v_CSharp
{
    class Playback
    {
        private string ukaz; //command
        [DllImport("winmm.dll")] // v program vključimo DLL za delo z multimedijo
        private static extern long mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, int hwdCallback); //funkcija za delo multimedijo
                                                                                                                                            //sklicuje se na winmm.dll
        public void odpri_skladbo(string datoteka)
        {
            ukaz = "open \"" + datoteka + "\" type MPEGVideo alias MUSIC";
            mciSendString(ukaz, null, 0, 0);
        }//funkcija odpre skladbo za predvajanje
        public void predvajaj()
        {
            ukaz = "play MUSIC";
            mciSendString(ukaz, null, 0, 0);
        }//funkcija začne predvajati skladbo
        public void ustavi()
        {
            ukaz = "stop MUSIC";
            mciSendString(ukaz, null, 0, 0);
            ukaz = "close MUSIC";
            mciSendString(ukaz, null, 0, 0);
        }//funkcija ustavi predvajanje skladbe
        public void ponovi()
        {
            ukaz = "seek MUSIC to start";
            mciSendString(ukaz, null, 0, 0);
            predvajaj();
        }
        public void isci(int cas)
        {
            ukaz = "seek MUSIC to " + cas;
            mciSendString(ukaz, null, 0, 0);
            predvajaj();
        }
    }
}
