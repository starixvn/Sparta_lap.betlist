namespace Fanex.Athena.DataAccess.Core.UnitTests.TestHelpers
{
    using Data;
    using Fanex.Athena.Model.Core.DTO.Common;
    using NSubstitute;

    public static class ResultInfoHelper
    {
        public const int ResultCode = 100;
        public const string ResultMessage = "Success";

        public static void MockResultInfo(IObjectDb objectDb)
        {
            objectDb.GetParameterValue("ErrCode").Returns(ResultCode);
            objectDb.GetParameterValue("ErrMsg").Returns(ResultMessage);
        }
    }
}