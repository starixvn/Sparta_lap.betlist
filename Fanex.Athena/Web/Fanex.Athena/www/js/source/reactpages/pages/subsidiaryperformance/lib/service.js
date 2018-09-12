import LangHelper from '../../../lib/langHelper'
import Site from '../../../lib/site'

export const HeaderText = {
    index: '#',
    subsidiary: LangHelper.getSingleResource("Subsidiary"),
    currency: LangHelper.getSingleResource("Currency"),
    product: LangHelper.getSingleResource("Product"),
    betCount: LangHelper.getSingleResource("BetCount"),
    turnover: LangHelper.getSingleResource("Turnover"),
    customerWL: LangHelper.getSingleResource("CustomerW_L"),
    subsidiaryWL: `${LangHelper.getSingleResource("SubsidiaryWL")}`,
    subsidiaryComm: `${LangHelper.getSingleResource("Subsidiary")} ${LangHelper.getSingleResource("Comm")}`,
    externalId: LangHelper.getSingleResource("ExternalId"),
    systemId: LangHelper.getSingleResource("SystemId"),
}

export const TotalText = {
    total: LangHelper.getSingleResource('Total'),
    totalIn: LangHelper.getSingleResource('Totalin'),
    based: LangHelper.getSingleResource('Based')
}

export const BreadcrumbText = {
    main: LangHelper.getSingleResource('Main'),
    customer: LangHelper.getSingleResource('Customer')
}

export class Service {
    static getReportTitle(fromDate, toDate, selectedBaseCurrency) {
        const baseCurrency = `${LangHelper.getSingleResource("BaseCurrency")}: ${selectedBaseCurrency}`;

        return `${LangHelper.getSingleResource("SubsidiaryPerformance")}, ${fromDate} - ${toDate}, ${baseCurrency}`;
    }

    static showErrorMsg() {
        alert(LangHelper.getSingleResource('ErrorProcessingMsg'));
    }
}