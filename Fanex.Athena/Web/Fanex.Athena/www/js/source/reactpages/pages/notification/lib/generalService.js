import LangHelper from '../../../lib/langHelper'
import Site from '../../../lib/site'

const headerText = {
    index: "#",
    type: LangHelper.getSingleResource("Type"),
    dateTime: `${LangHelper.getSingleResource("Date")}/${LangHelper.getSingleResource("Time")}`,
    content: LangHelper.getSingleResource("Content")
}

export default class GeneralService {
    static getReportTitle(dateRange) {
        const fromDate = Site.convertDateToString(dateRange.fromDate);
        const toDate = Site.convertDateToString(dateRange.toDate);

        return `${LangHelper.getSingleResource("General")}, ${fromDate} - ${toDate}`;
    }

    static getHeaderText() {
        return headerText;
    }
}