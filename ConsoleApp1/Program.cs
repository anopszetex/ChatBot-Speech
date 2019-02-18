using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using AIMLbot;
using System.Speech.Synthesis;
using Microsoft.CognitiveServices.Speech.Translation;
using Microsoft.Speech.Recognition;



namespace ConsoleApp1 {
    class Program {

        private static SpeechSynthesizer will = null;
    
        public static async Task RecognizeSpeechAsync() {
            will = new SpeechSynthesizer();
            bool search = false;
            // Creates an instance of a speech config with specified subscription key and service region.
            // Replace with your own subscription key and service region (e.g., "westus").
            var config = SpeechConfig.FromSubscription("a9e9a5a07bb5449ab4251ccc0959db64", "westus");

            // Creates a speech recognizer.
            using (var recognizer = new SpeechRecognizer(config))
            {
                Bot myBot = new Bot();
                myBot.loadSettings();
                User myUser = new User("consoleUser", myBot);
                myBot.isAcceptingUserInput = false;
                myBot.loadAIMLFromFiles();
                myBot.isAcceptingUserInput = true;
               

                while (true) {
                    var result = await recognizer.RecognizeOnceAsync();

                    if (result.Reason == ResultReason.RecognizedSpeech)
                        Console.Write("You: " + result.Text);

                    if (result.Text.ToLower() == "close" || result.Text.ToLower() == "close.")
                        Environment.Exit(1);

                    if (result.Text.ToLower().StartsWith("search") || result.Text.ToLower().StartsWith("Search.")) {

                        String Search = result.Text.ToLower().Replace("search", "");
                        Console.WriteLine();

                        System.Diagnostics.Process.Start("www.google.com/search?q=" + Search);
                        will.SpeakAsyncCancelAll();

                    }

                    if (result.Reason == ResultReason.RecognizedSpeech && !result.Text.ToLower().StartsWith("search")) {

                            string input = result.Text;
                            Request r = new Request(input, myUser, myBot);
                            Result res = myBot.Chat(r);
                            Console.WriteLine("\nWill: " + res.Output);
                            will.Speak(res.Output);

                    }
                }
               
            }
        }


        static void Main(string[] args) {
            try
            {
                RecognizeSpeechAsync().Wait();
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex);
            }
        }




    }
}
