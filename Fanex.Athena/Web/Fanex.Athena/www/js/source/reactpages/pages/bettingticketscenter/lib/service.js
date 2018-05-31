import LangHelper from '../../../lib/langHelper'
import Site from '../../../lib/site'

const HeaderText = {
    Index: "#",
    Platform: LangHelper.getSingleResource("Platform"),
    ExternalId : LangHelper.getSingleResource("ExternalId"),
    SystemId: LangHelper.getSingleResource("SystemId"),
    TransTime: LangHelper.getSingleResource("TransTime"),
    Choice: LangHelper.getSingleResource("Choice"),
    Odds: LangHelper.getSingleResource("Odds"),
    Stake: LangHelper.getSingleResource("Stake"),
    WinLoss: LangHelper.getSingleResource("WinLoss"),
    LicenseeWL: LangHelper.getSingleResource("LicenseeWL"),
    Comm: LangHelper.getSingleResource("Comm"),
    Status: LangHelper.getSingleResource("Status"),
    Live: LangHelper.getSingleResource("Live")
}

export default class Service {
    static getReportTitle(dateRange, currency, baseCurrency) {
        const dateText = Site.convertDateToString(dateRange.fromdate);
        const baseCurrencyText = LangHelper.getSingleResource("BaseCurrency") + ": " + baseCurrency;
        var currencyText = LangHelper.getSingleResource("Currency") + ": " + currency;
        return `${LangHelper.getSingleResource("BettingTicketCenter")}, ${dateText}, ${currencyText}${baseCurrencyText}`;
    }

    static getHeaderText() {
        return HeaderText;
    }
}