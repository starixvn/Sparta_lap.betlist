namespace Fanex.Athena.Result.Service.Interfaces
{
    using System;
    using Fanex.Result.Core.KenoLotteryTG;

    [CLSCompliant(false)]
    public interface IKenoLotteryTGResultFactory
    {
        IKenoLotteryTGResult CreateResultApi(int vendorProductId);
    }
}