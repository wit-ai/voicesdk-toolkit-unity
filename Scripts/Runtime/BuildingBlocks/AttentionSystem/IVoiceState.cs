/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Meta.WitAi.Json;
using System;
namespace Oculus.Voice.Toolkit
{
    /// <summary>
    /// Implement this interface if you register the error state from VoiceUI Script.
    /// The data you received in the function should be converted to ErrorData. Example: ErrorData data = dataObj as ErrorData;
    /// </summary>
    public interface IErrorState
    {
        void ErrorHandler(VoiceDataBase data);

    }

    /// <summary>
    /// Implement this interface if you register the MicOn or MicOff state from VoiceUI Script.
    /// </summary>
    public interface IMicState
    {
        void MicOnHandler(VoiceDataBase data);
        void MicOffHandler(VoiceDataBase data);
    }

    /// <summary>
    /// Implement this interface if you register the listening state from VoiceUI Script.
    /// The data you received in the function should be converted to ListeningData. Example: ListeningData data = dataObj as ListeningData;
    /// </summary>
    public interface IListeningState
    {
        void ListeningHandler(VoiceDataBase data);
    }

    /// <summary>
    /// Implement this interface if you register the listening state from VoiceUI Script.
    /// The data you received in the function should be converted to ListeningData. Example: ListeningData data = dataObj as ListeningData;
    /// </summary>
    public interface IResponseState
    {
        void ResponseHandler(VoiceDataBase data);
    }
    public class VoiceDataBase
    {
    }
    public class MicData : VoiceDataBase
    {
    }
    public class ErrorData : VoiceDataBase
    {

        public ErrorData(string type, string message)
        {
            this.type = type;
            this.message = message;
        }
        public string type;
        public string message;
    }
    public class ListeningData : VoiceDataBase
    {
        public float micLevel = 0;
        public string transcription = String.Empty;
    }
    public class NLUResponseData : VoiceDataBase
    {
        public DateTime requestTime;
        public DateTime responseTime;
        public WitResponseNode response = null;
    }
}
