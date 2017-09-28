const Data = [],
    Total = [],
    GrandTotal = {};

const defaultProps = {
    subsidiaryList: [{ ID: 1, Name: "New York", Checked: true }, { ID: 2, Name: "India", Checked: false }],
    productOptions: [{ Code: "SB", Name: "Sportsbook", Status: true }, { Code: "BA", Name: "BA", Status: true }],
    baseCurrencyList: [{ "value": "EUR", "label": "EUR" }, { "value": "USD", "label": "USD" }],
    selectedBaseCurrency: "EUR",
    dateRange: { fromDate: '09/26/2017', toDate: '09/28/2017' },
    dateConstraint: { minDate: '06/01/2012', maxDate: '09/28/2017' },
    gridData: { Data, Total, GrandTotal },
    xsrfToken: "token",

    setSelectedProduct: jest.fn(),
    setSelectedSubsidiary: jest.fn(),
    setDateRange: jest.fn(),
    toggleLoader: jest.fn(),
}

const bodyData = [{
    "Subsidiary": "916515",
    "CurrencyName": "UUS",
    "Details": [{
        "Product": "All",
        "BetCount": 52,
        "Turnover": 322.0000,
        "CustomerWinLoss": 142.3210,
        "SubsidiaryWinLoss": -141.7636
    }, {
        "Product": "Cash Out",
        "BetCount": 4,
        "Turnover": 0.0000,
        "CustomerWinLoss": 0.0000,
        "SubsidiaryWinLoss": 0.5574
    }]
}]

const totalData = [{
    "SubsidiaryId": 858249,
    "Subsidiary": "Total 858249",
    "CurrencyId": 0,
    "CurrencyName": "Based EUR",
    "Product": null,
    "BetCount": 894,
    "Turnover": 0.0,
    "CustomerWinLoss": 0.0,
    "SubsidiaryWinLoss": 0.0
}, {
    "SubsidiaryId": 916515,
    "Subsidiary": "Total 916515",
    "CurrencyId": 0,
    "CurrencyName": "Based EUR",
    "Product": null,
    "BetCount": 99,
    "Turnover": 0.0,
    "CustomerWinLoss": 0.0,
    "SubsidiaryWinLoss": -141.7636
}]

const grandTotalData = {
    "SubsidiaryId": 0,
    "Subsidiary": "Total",
    "CurrencyId": 0,
    "CurrencyName": "Based EUR",
    "Product": null,
    "BetCount": 13286376,
    "Turnover": 405367913.09429135,
    "CustomerWinLoss": -14046804.028517062,
    "SubsidiaryWinLoss": 14046469.750504408
}

export { defaultProps, totalData, bodyData, grandTotalData };