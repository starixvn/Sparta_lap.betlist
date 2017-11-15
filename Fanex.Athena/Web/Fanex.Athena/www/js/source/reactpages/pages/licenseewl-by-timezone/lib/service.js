import LangHelper from '../../../lib/langHelper'
import Site from '../../../lib/site'

const headerText = {
    Product : LangHelper.getSingleResource("Product"),
    Currency: LangHelper.getSingleResource("Currency"),
    BetCount: LangHelper.getSingleResource("BetCount"),
    Turnover: LangHelper.getSingleResource("Turnover"),
    CustomerComm: LangHelper.getSingleResource("CustomerComm"),
    LicenseeWL: LangHelper.getSingleResource("LicenseeWL"),
    LicenseeNetWL: LangHelper.getSingleResource("LicenseeNetWL"),
    Margin: LangHelper.getSingleResource("Margin")
}

export default class Service {
    static getReportTitle(dateRange, selectedBaseCurrency) {
        const fromDate = Site.convertDateToString(dateRange.fromDate);
        const toDate = Site.convertDateToString(dateRange.toDate);
        const viewInByCurrency = `${LangHelper.getSingleResource("Viewin").replace("{0}", selectedBaseCurrency)}`;

        return `${LangHelper.getSingleResource("LicenseeWinLossByTimeZone")}, ${fromDate} - ${toDate} - ${viewInByCurrency}`;
    }

    static getHeaderText() {
        return headerText;
    }
}