namespace Fanex.Athena.Result.Service.Interfaces
{
    using Fanex.Result.Core.KenoLotteryTG;
    using System;

    [CLSCompliant(false)]
    public interface IKenoLotteryTGResultFactory
    {
        IKenoLotteryTGResult CreateResultApi(int vendorProductId);
    }
}
