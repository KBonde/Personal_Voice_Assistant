using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace Personal_Voice_Assistant
{
    public partial class Form1 : Form
    {
        //Create new synthesizer for speach
        SpeechSynthesizer synth;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            // Create a new SpeechRecognitionEngine instance.
            SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();
            synth = new SpeechSynthesizer();
            
            //Array of commands
            Choices commands = new Choices();
            commands.Add(new string[] {
                "What is the time", "Tell me the time", "What time is it", "Time", //Time
                "What day is it", "What day is it today", "Which day is it today", "Can you tell me what day it is", "day", //Day
                "play glitter"

            });
 
            // Create a GrammarBuilder object and append the Choices object.
            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(commands);

            // Create the Grammar instance and load it into the speech recognition engine.
            Grammar g = new Grammar(gb);
            recognizer.LoadGrammar(g);
            recognizer.SetInputToDefaultAudioDevice();

            //Speak through default audio device
            synth.SetOutputToDefaultAudioDevice(); 

            recognizer.RecognizeAsync(RecognizeMode.Multiple);

            // Register a handler for the SpeechRecognized event.
            recognizer.SpeechRecognized +=
              new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);
        }

        // Create a simple handler for the SpeechRecognized event.
        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            DateTime currentTime; //Var to hold time

            switch (e.Result.Text)
            {
                case "what day is it":
                case "what day is it today":
                case "which day is it today":
                case "can you tell me what day it is":
                case "day":
                    currentTime = DateTime.Now;
                    string dayString = currentTime.ToString("dddd");

                    richTextBox1.Text += "\nCurrent day: ";
                    richTextBox1.Text += dayString;

                    synth.Speak("Current day is" + dayString);
                    break;

                case "What is the time":
                case "What time is it":
                case "Tell me the time":
                case "Time":
                    currentTime = DateTime.Now; //Get current time

                    /*Convert time to a string, and format to only display hours and minutes*/
                    string timeString = currentTime.ToString("HH:mm");

                    /*Display current time*/
                    richTextBox1.Text += "\nCurrent time: ";
                    richTextBox1.Text += timeString;

                    synth.Speak("Current time is" + timeString);
                    
                    break;

                case "play glitter":
                    System.Media.SoundPlayer player = new System.Media.SoundPlayer();

                    player.SoundLocation = "glitter.wav";
                    player.Play();
                    break;
            }
        }
    }
}

