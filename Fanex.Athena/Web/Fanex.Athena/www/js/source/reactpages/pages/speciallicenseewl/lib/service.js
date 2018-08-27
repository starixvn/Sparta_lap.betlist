import LangHelper from '../../../lib/langHelper'

export const Url = 'api/SpecialLicenseeWinLossReport/Index';

export const HeaderText = {
    index: '#',
    date: LangHelper.getSingleResource('Date'),
    currency: LangHelper.getSingleResource('Currency'),
    turnover: LangHelper.getSingleResource('Turnover'),
    totalTransaction: LangHelper.getSingleResource('TotalTransaction'),
    customer: LangHelper.getSingleResource('CustomerW_L'),
    licenseeWL: LangHelper.getSingleResource('LicenseeW_L'),
    total: LangHelper.getSingleResource('Total'),
    SB: LangHelper.getSingleResource('SB'),
    NG: LangHelper.getSingleResource('NG')
};

export const TotalText = {
    total: LangHelper.getSingleResource('Total'),
    totalIn: LangHelper.getSingleResource('Totalin')
}

export class Service {
    static getReportTitle(fromDate, toDate, currencyId, currencyName, baseCurrency) {
        const viewInByCurrency = currencyId == 0 
            ? LangHelper.getSingleResource('AllCurrencies')
            : `${LangHelper.getSingleResource('Viewin').replace('{0}', baseCurrency.toUpperCase() || currencyName)}`;

        return `${LangHelper.getSingleResource('LicenseeWinLoss')}, GMT+8, ${fromDate} - ${toDate} - ${viewInByCurrency}`;
    }

    static showErrorMsg() {
        alert(LangHelper.getSingleResource('ErrorProcessingMsg'));
    }

    static sort(data, sortBy, sortOrder) {
        if (sortOrder === 'asc') {
            data.sort((a, b) => a[sortBy] > b[sortBy] );
        }
        else {
            data.sort((a, b) => a[sortBy] < b[sortBy] );
        }
    }
};