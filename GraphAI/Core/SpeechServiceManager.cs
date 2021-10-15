using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphAI.Core
{
    public class SpeechServiceManager
    {
        private readonly AudioConfig audioConfig;
        private readonly SpeechConfig speechConfig;

        public SpeechServiceManager()
        {
            this.audioConfig = AudioConfig.FromDefaultMicrophoneInput();
            this.speechConfig = SpeechConfig.FromSubscription("<add speechserviceId>", "<add location>");
        }

        public async Task<string> CaptureAudioFromMicrophone()
        {
            using var recognizer = new SpeechRecognizer(speechConfig, audioConfig);
            var result = await recognizer.RecognizeOnceAsync();

            return result.Text;
        }
    }
}
