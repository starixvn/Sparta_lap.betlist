namespace Fanex.Athena.DataAccess.Core.Tests.TestHelpers
{
    using Data;
    using Fanex.Athena.Model.Core.DTO.Common;
    using NSubstitute;

    public static class ResultInfoHelper
    {
        public static IResultInfo GetFakeResultInfo(IObjectDb objectDb)
        {
            var code = 100;
            var message = "Success";
            objectDb.GetParameterValue("ErrCode").Returns(code);
            objectDb.GetParameterValue("ErrMsg").Returns(message);
            return new ResultInfo { Code = code, Message = message };
        }
    }
}