﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;

namespace Personal_Voice_Assistant
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            // Create a new SpeechRecognitionEngine instance.
            SpeechRecognitionEngine recognizer = new SpeechRecognitionEngine();

            // Create a simple grammar that recognizes "red", "green", or "blue".
            Choices colors = new Choices();
            colors.Add(new string[] { "red", "green", "blue", "What is the time", "Tell me the time", "What time is it", "Time" });
 
            // Create a GrammarBuilder object and append the Choices object.
            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(colors);

            // Create the Grammar instance and load it into the speech recognition engine.
            Grammar g = new Grammar(gb);
            recognizer.LoadGrammar(g);
            recognizer.SetInputToDefaultAudioDevice();

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
                case "red":
                    richTextBox1.Text += "\nred was recognized";
                    break;

                case "green":
                    richTextBox1.Text += "\ngreen was recognized";
                    break;

                case "blue":
                    richTextBox1.Text += "\nblue was recognized";
                    break;

                case "What is the time":
                case "What time is it":
                case "Tell me the time":
                case "Time":
                    currentTime = DateTime.Now; //Get current time

                    /*Convert time to a string, and format to only display hours and minutes*/
                    string timeString = currentTime.ToString("HH:mm");

                    /*Display current time*/
                    richTextBox1.Text += "\nDisplaying time: ";
                    richTextBox1.Text += timeString;
                    break;
            }
        }
    }
}

