using System.Runtime.InteropServices;
using System.Text;
//This class handles the audio playback 
namespace Predvajalnik_v_CSharp
{
    class Playback
    {
        private string command; //command

        [DllImport("winmm.dll")] // include windows multimedia dll
        private static extern long mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, int hwdCallback); //funkcija za delo multimedijo
                                                                                                                                            //sklicuje se na winmm.dll
        public void open_audio_file(string file)
        {
            command = "open \"" + file + "\" type MPEGVideo alias MUSIC";
            mciSendString(command, null, 0, 0);
        }//open the song for playing 
        public void play()
        {
            command = "play MUSIC";
            mciSendString(command, null, 0, 0);
        }//playing the song
        public void stop_music()
        {
            command = "stop MUSIC";
            mciSendString(command, null, 0, 0);
            command = "close MUSIC";
            mciSendString(command, null, 0, 0);
        }//stop the playing of the song //stop will be excluded 
        public void pause_song()
        {
            command = "pause MUSIC";
            mciSendString(command, null, 0, 0);
        } //pausing the music
        public void ponovi()
        {
            command = "seek MUSIC to start";
            mciSendString(command, null, 0, 0);
            play();
        }// repeat the song 

        
      

     
        public void seek(int cas)
        {
            command = "seek MUSIC to " + cas;
            mciSendString(command, null, 0, 0);

            play();
        } //seek a part of the song 

           
        }

    }

