/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Facebook.WitAi.Lib;
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

    [Serializable]
    public class VoiceDataBase
    {
    }

    [Serializable]
    public class MicData : VoiceDataBase
    {
    }

    [Serializable]
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

    [Serializable]
    public class ListeningData : VoiceDataBase
    {
        public float micLevel = 0;
        public string transcription = String.Empty;
    }

    [Serializable]
    public class NLUResponseData : VoiceDataBase
    {
        public DateTime requestTime;
        public DateTime responseTime;
        public WitResponseNode response = null;
    }
}
