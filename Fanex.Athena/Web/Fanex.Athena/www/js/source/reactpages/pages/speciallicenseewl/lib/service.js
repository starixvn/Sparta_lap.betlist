import LangHelper from '../../../lib/langHelper'

export const Url = 'api/SpecialLicenseeWinLossReport/Index';

export const HeaderText = {
    index: '#',
    date: LangHelper.getSingleResource('Date'),
    externalId: LangHelper.getSingleResource('ExternalId'),
    currency: LangHelper.getSingleResource('Currency'),
    turnover: LangHelper.getSingleResource('Turnover'),
    totalTransaction: LangHelper.getSingleResource('TotalTransaction'),
    customer: LangHelper.getSingleResource('CustomerW_L'),
    customerWL: `${LangHelper.getSingleResource('Win')}/${LangHelper.getSingleResource('Loss')}`,
    customerComm: LangHelper.getSingleResource('Comm'),
    customerTotal: LangHelper.getSingleResource('Total'),
    licenseeWL: LangHelper.getSingleResource('LicenseeW_L'),
    total: LangHelper.getSingleResource('Total'),
    SB: LangHelper.getSingleResource('SB'),
    NG: LangHelper.getSingleResource('NG')
};

export const BreadcrumbText = {
    main: LangHelper.getSingleResource('Main'),
    licensee: LangHelper.getSingleResource('Licensee'),
    customer: LangHelper.getSingleResource('Customer')
}

export const TotalText = {
    total: LangHelper.getSingleResource('Total'),
    totalIn: LangHelper.getSingleResource('Totalin')
}

export class Service {
    static getReportTitle(fromDate, toDate, currencyId, currencyName, baseCurrency) {
        const viewInByCurrency = currencyId == 0 
            ? LangHelper.getSingleResource('AllCurrencies')
            : `${LangHelper.getSingleResource('Viewin').replace('{0}', baseCurrency.toUpperCase() || currencyName)}`;

        return `${LangHelper.getSingleResource('LicenseeWinLoss')}, ${fromDate} - ${toDate} - ${viewInByCurrency}`;
    }

    static showErrorMsg() {
        alert(LangHelper.getSingleResource('ErrorProcessingMsg'));
    }
};