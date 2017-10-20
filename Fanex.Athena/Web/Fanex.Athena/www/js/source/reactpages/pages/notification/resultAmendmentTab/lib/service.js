import LangHelper from '../../../../lib/langHelper'
import Site from '../../../../lib/site'

const headerText = {
    ticketList: LangHelper.getSingleResource("TicketList"),
    dateTime: `${LangHelper.getSingleResource("Date")}/${LangHelper.getSingleResource("Time")}`,
    content: LangHelper.getSingleResource("Content")
}

export default class Service {
    static getReportTitle(dateRange) {
        const fromDate = Site.convertDateToString(dateRange.fromDate);
        const toDate = Site.convertDateToString(dateRange.toDate);
        
        return `${LangHelper.getSingleResource("ResultAmendment")}, ${fromDate} - ${toDate}`;
    }

    static getHeaderText() {
        return headerText;
    }
}