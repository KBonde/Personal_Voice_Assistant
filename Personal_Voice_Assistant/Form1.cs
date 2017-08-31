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
using System.Xml;

namespace Personal_Voice_Assistant
{
    public partial class Form1 : Form
    {
        //Create new synthesizer for speach
        SpeechSynthesizer synth;
        // Create a new SpeechRecognitionEngine instance.
        SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            synth = new SpeechSynthesizer();
            
            //Array of commands
            Choices commands = new Choices();
            commands.Add(new string[] {
                "What is the time", "Tell me the time", "What time is it", "Time", //Time
                "What day is it", "What day is it today", "Which day is it today", "Can you tell me what day it is", "day", //Day
                "play", "stop playing", "poop", //Music commands
                "glitter", //Songs
                "weather" //Weather
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
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();

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

                case "play":
                    //Use the recognizer to determine what song they want to be played
                    recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(songHandler);
                    
                    break;

                //Stop playing anything
                case "stop playing":
                    player.Stop();
                    break;

                case "weather":
                    synth.Speak("The sky is " + GetWeather("cond") + " today, and the temperature outside is " + GetWeather("temp") + " degrees celcius.");
                    break;
            }

            void songHandler(object poop, SpeechRecognizedEventArgs ePoop)
            {
                switch (ePoop.Result.Text)
                {
                    case "glitter":
                        playSong("glitter");
                        break;
                }
            }

            void playSong(string name)
            {
                richTextBox1.Text += "\nPlaying: " + name;

                player.SoundLocation = "../../../music/" + name + ".wav";
                player.Play();
            }

            String GetWeather(String input)
            {
                string temp;
                string condition;

                double tempToCelcius = 0;

                String query = String.Format("https://query.yahooapis.com/v1/public/yql?q=select * from weather.forecast where woeid in (select woeid from geo.places(1) where text='denmark, aarhus')&diagnostics=true&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys");
                                              

                XmlDocument wData = new XmlDocument();
                wData.Load(query);

                XmlNamespaceManager manager = new XmlNamespaceManager(wData.NameTable);
                manager.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");

                XmlNode channel = wData.SelectSingleNode("query").SelectSingleNode("results").SelectSingleNode("channel");
                XmlNodeList nodes = wData.SelectNodes("query/results/channel");
                try
                {
                    temp = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["temp"].Value;
                    condition = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["text"].Value;

                    //Convert from F to C
                    Double.TryParse(temp, out tempToCelcius); //Convert to double

                    tempToCelcius = Math.Round((tempToCelcius - 32) * 0.5556); //Calculate in C

                    temp = Convert.ToString(tempToCelcius); //Convert back to string

                    if (input == "temp")
                    {
                        return temp;
                    }

                    if (input == "cond")
                    {
                        return condition;
                    }
                }
                catch
                {
                    return "Error Reciving data";
                }
                return "error";
            }
        }
    }
}

