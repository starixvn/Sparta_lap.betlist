import React from 'react'
import { DataGrid } from '../datagrid';
import service from '../../lib/service';
import renderer from 'react-test-renderer'

const Data = [], Total = [], GrandTotal = {};
const defaultProps = {
    gridData: { Data, Total, GrandTotal },
    dateRange: { fromDate: "09/01/2017", toDate: "09/30/2017" },
    selectedBaseCurrency: "EUR",
    toggleLoader: jest.fn()
}

const bodyData = [
    {
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
    }
]

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

describe('DataGrid ', () => {
    afterEach(() => {
        jest.resetAllMocks();
    })

    describe('No data', () => {
        it('return null when gridData is undefined', () => {
            const props = { gridData: {} };
            const dataGrid = renderer.create(<DataGrid {...props} />);

            const result = dataGrid.toJSON();

            expect(result).toBeNull();
        })

        it('gridData length is 0 - render correct content', () => {
            const dataGrid = renderer.create(<DataGrid {...defaultProps} />);

            const result = dataGrid.toJSON();

            expect(result).toMatchSnapshot();
        })

        it('gridData length is 0 - render container with correct report title content', () => {
            const dataGrid = renderer.create(<DataGrid {...defaultProps} />);
            const { gridData, dateRange, selectedBaseCurrency } = defaultProps;
            service.getReportTitle = jest.fn((dateRange, selectedBaseCurrency) => {
                return `SubsidiaryPerformance, ${dateRange.fromDate} - ${dateRange.toDate} - ${selectedBaseCurrency}`;
            })

            const nextProps = Object.assign({}, defaultProps, { gridData: { Data: {} } });
            dataGrid.update(<DataGrid {...nextPropsu} />);

            const result = dataGrid.toJSON();

            expect(result).toMatchSnapshot();
        })
    })

    describe('Data is not empty', () => {
        const headerText = {
            subsidiary: "Subsidiary",
            currency: "Currency",
            product: "Product",
            betCount: "BetCount",
            turnover: "Turnover",
            customerWL: "CustomerW_L",
            subsidiaryWL_comm: "SubsidiaryWL/ Comm"
        }

        it('_buildHeader - render correct header content', () => {
            service.getHeaderText = jest.fn(() => headerText);

            const dataGrid = new DataGrid(defaultProps);
            const header = renderer.create(dataGrid._buildHeader());

            const result = header.toJSON();

            expect(result).toMatchSnapshot();
        })

        it('_buildBody - render correct body content', () => {
            const props = Object.assign({}, defaultProps, { gridData: { ...defaultProps.gridData, Data: bodyData } });
            const dataGrid = new DataGrid(props);
            const bodyWrapper = (<div>{dataGrid._buildBody()}</div>);
            const content = renderer.create(bodyWrapper);

            const result = content.toJSON();

            expect(result).toMatchSnapshot();
        })

        describe('_buildGrandTotal ', () => {
            it('grandTotal is undefined - return null', () => {
                const props = Object.assign({}, defaultProps, { gridData: { GrandTotal: null } });
                const dataGrid = new DataGrid(props);
                const content = dataGrid._buildGrandTotal();

                expect(content).toBeNull();
            })

            it('SubsidiaryWL is positive - render correct grand total content', () => {
                const props = Object.assign({}, defaultProps, { gridData: { ...defaultProps.gridData, GrandTotal: grandTotalData } });
                const dataGrid = new DataGrid(props);
                const grandTotalWrapper = (<div>{dataGrid._buildGrandTotal()}</div>);
                const content = renderer.create(grandTotalWrapper);

                const result = content.toJSON();

                expect(result).toMatchSnapshot();
            })

            it('SubsidiaryWL is negative - render correct grand total content', () => {
                const negativeGrandTotal = { ...grandTotalData, SubsidiaryWinLoss: -123.98 };
                const props = Object.assign({}, defaultProps, { gridData: { ...defaultProps.gridData, GrandTotal: negativeGrandTotal } });
                const dataGrid = new DataGrid(props);
                const grandTotalWrapper = (<div>{dataGrid._buildGrandTotal()}</div>);
                const content = renderer.create(grandTotalWrapper);

                const result = content.toJSON();

                expect(result).toMatchSnapshot();
            })
        })

        it('render correct footer content', () => {
            const props = Object.assign({}, defaultProps, { gridData: { ...defaultProps.gridData, Total: totalData } });
            const dataGrid = new DataGrid(props);
            dataGrid._buildGrandTotal = jest.fn(() => "Grand Total");
            const totalWrapper = (<div>{dataGrid._buildFooter()}</div>);
            const content = renderer.create(totalWrapper);

            const result = content.toJSON();

            expect(result).toMatchSnapshot();
        })

        it('grid data should be rendered correctly', () => {
            service.getHeaderText = jest.fn(() => headerText);

            const gridData = { Data: bodyData, Total: totalData, GrandTotal: grandTotalData };
            const props = Object.assign({}, defaultProps, { gridData: { ...gridData } });

            const dataGrid = new DataGrid(props);
            const content = dataGrid.render();

            const result = renderer.create(content).toJSON();

            expect(result).toMatchSnapshot();
        })
    })

    describe('componentWillReceiveProps ', () => {
        it('gridData update - toggleLoader function should be called with correct parameter', () => {
            const dataGrid = new DataGrid(defaultProps);
            const nextProps = Object.assign({}, defaultProps, { gridData: { Data: { ...bodyData } } });

            dataGrid.componentWillReceiveProps(nextProps);

            expect(dataGrid.props.toggleLoader).toHaveBeenCalledWith(false);
        })

        it('gridData do not update - toggleLoader function should not be called', () => {
            const dataGrid = new DataGrid(defaultProps);
            const nextProps = Object.assign({}, defaultProps, { selectedBaseCurrency: "USD" });

            dataGrid.componentWillReceiveProps(nextProps);

            expect(dataGrid.props.toggleLoader).not.toHaveBeenCalled();
        })
    })
})