namespace Fanex.Athena.Result.Service
{
    using System;
    using Fanex.Athena.Common.Utility.Services.Interfaces;
    using Fanex.Athena.Common.Utility.Services.Internal;
    using Fanex.Athena.Result.Model;
    using Fanex.Athena.Result.Model.Const;
    using Fanex.Athena.Result.Model.DTO.Indo4D;
    using Fanex.Athena.Result.Service.Interfaces;
    using Fanex.Result.Core.Base;

    public class Indo4DResult : IIndo4DResult
    {
        private readonly Indo4DConfig _configs;
        private readonly IWebClientService _client;
        private readonly ISerializer _serializer;
        private readonly INewGui _newGui;
        private const string DefaultPlatform = "d";
        private readonly int GMT8 = 8;

        public Indo4DResult(IWebClientService client, ISerializer serializer, INewGui newGui)
        {
            _serializer = serializer;
            _client = client;
            _newGui = newGui;
            _configs = Indo4DConfig.Configs;
        }

        public string GetResult(string language = "en-US")
        {
            var requestData = GenerateRequestData();

            if ((DebugMode)_configs.DebugMode == DebugMode.DontSendRequest)
            {
                throw new ResultException("Request data:" + _serializer.Serialize(requestData));
            }

            var responseData = _client.UploadString<Indo4DRequest, Indo4DReponse>(_configs.UrlResult, requestData);
            var isValidResponse = ValidateResponse(responseData, requestData);
            if (!isValidResponse)
            {
                throw new ResultException($"Url of Indo4D send request incorrectly! Detail request:" +
                    $" {_serializer.Serialize(requestData)}; Detail response: {_serializer.Serialize(responseData)}");
            }

            return AddMoreInfoToResult(responseData.Result.Url, language);
        }

        private string AddMoreInfoToResult(string resultUrl, string language, string platform = DefaultPlatform)
        {
            var languageCode = LanguageConstants.GetLanguageCode(language);

            return $"{resultUrl}&l={languageCode}&pl={platform}&dm=0";
        }

        private static bool ValidateResponse(Indo4DReponse responseData, Indo4DRequest requestData)
            => responseData.Result.Seq == requestData.Call.Seq &&
                (responseData.Result.ErrorCode == "0" || responseData.Result.ErrorCode == null);

        private Indo4DRequest GenerateRequestData()
        {
            return new Indo4DRequest
            {
                TimeStamp = DateTime.UtcNow.AddHours(GMT8).ToString("yyyy-MM-dd HH:mm:ss.fff"),
                Auth = new Indo4DAuth
                {
                    Login = _configs.Login,
                    Password = _configs.Password
                },
                Call = new Indo4DCall
                {
                    Seq = _newGui.CreateNewId(),
                    SiteId = _configs.SiteId
                }
            };
        }
    }
}