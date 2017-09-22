import LangHelper from '../../../lib/langHelper'
import Site from '../../../lib/site'

export default class Common {
    static getReportTitle(dateRange, selectedBaseCurrency) {
        const fromDate = Site.convertDateToString(dateRange.fromDate);
        const toDate = Site.convertDateToString(dateRange.toDate);
        const viewInByCurrency = `${LangHelper.getSingleResource("Viewin").replace("{0}", selectedBaseCurrency)}`;

        return `${LangHelper.getSingleResource("SubsidiaryPerformance")}, ${fromDate} - ${toDate} - ${viewInByCurrency}`;
    }

    static getHeaderText() {
        return [
            LangHelper.getSingleResource("Subsidiary"), LangHelper.getSingleResource("Currency"),
            LangHelper.getSingleResource("Product"), LangHelper.getSingleResource("BetCount"),
            LangHelper.getSingleResource("Turnover"), LangHelper.getSingleResource("CustomerW_L"),
            `${LangHelper.getSingleResource("SubsidiaryWL")}/ ${LangHelper.getSingleResource("Comm")}`
        ]
    }
}