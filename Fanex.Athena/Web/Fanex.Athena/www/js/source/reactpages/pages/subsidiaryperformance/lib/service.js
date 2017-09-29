import LangHelper from '../../../lib/langHelper'
import Site from '../../../lib/site'

const headerText = {
    subsidiary: LangHelper.getSingleResource("Subsidiary"),
    currency: LangHelper.getSingleResource("Currency"),
    product: LangHelper.getSingleResource("Product"),
    betCount: LangHelper.getSingleResource("BetCount"),
    turnover: LangHelper.getSingleResource("Turnover"),
    customerWL: LangHelper.getSingleResource("CustomerW_L"),
    subsidiaryWL_comm: `${LangHelper.getSingleResource("SubsidiaryWL")}/ ${LangHelper.getSingleResource("Comm")}`
}

export default class Service {
    static getReportTitle(dateRange, selectedBaseCurrency) {
        const fromDate = Site.convertDateToString(dateRange.fromDate);
        const toDate = Site.convertDateToString(dateRange.toDate);
        const viewInByCurrency = `${LangHelper.getSingleResource("Viewin").replace("{0}", selectedBaseCurrency)}`;

        return `${LangHelper.getSingleResource("SubsidiaryPerformance")}, ${fromDate} - ${toDate} - ${viewInByCurrency}`;
    }

    static getHeaderText() {
        return headerText;
    }
}