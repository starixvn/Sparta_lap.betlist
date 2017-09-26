import React from 'react'
import { shallow, configure } from 'enzyme'
import Adapter from 'enzyme-adapter-react-15';
import { DataGrid } from '../datagrid';
import service from '../../lib/service';
import renderer from 'react-test-renderer'

configure({ adapter: new Adapter() });

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
    "SubsidiaryWinLoss": 0.0
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
    describe('No data', () => {
        it('return null when gridData is undefined', () => {
            const props = { gridData: {} };
            const dataGrid = shallow(<DataGrid {...props} />);
            const expectedResult = null;

            const result = dataGrid.html();

            expect(result).toBe(expectedResult);
        })

        it('gridData length is 0 - render container with class name "result-content"', () => {
            const dataGrid = shallow(<DataGrid {...defaultProps} />);
            const expectedResult = 1;

            const result = dataGrid.find('.result-content').length;

            expect(result).toBe(expectedResult);
        })

        it('gridData length is 0 - render correct content of ReportTitle component', () => {
            const dataGrid = shallow(<DataGrid {...defaultProps} />);
            const { gridData, dateRange, selectedBaseCurrency } = defaultProps;
            const expectedResult = `SubsidiaryPerformance, ${dateRange.fromDate} - ${dateRange.toDate} - ${selectedBaseCurrency}`;
            service.getReportTitle = jest.fn((dateRange, selectedBaseCurrency) => {
                return expectedResult;
            })

            dataGrid.setProps({ isLoading: true });
            const reportTitleProps = dataGrid.find('ReportTitle').props();

            const result = reportTitleProps.content;

            expect(result).toBe(expectedResult);
        })

        it('gridData length is 0 - render NoDataTitle component', () => {
            const dataGrid = shallow(<DataGrid {...defaultProps} />);
            const expectedResult = 1;

            const result = dataGrid.find('NoDataTitle').length;

            expect(result).toBe(expectedResult);
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

        afterEach(() => {
            jest.resetAllMocks();
        })
        it('render correct header content', () => {
            service.getHeaderText = jest.fn(() => headerText);

            const dataGrid = new DataGrid(defaultProps);
            const header = renderer.create(dataGrid._buildHeader());

            const result = header.toJSON();

            expect(result).toMatchSnapshot();
        })

        it('render correct body content', () => {
            const props = Object.assign({}, defaultProps, { gridData: { ...defaultProps.gridData, Data: bodyData } });
            const dataGrid = new DataGrid(props);
            const bodyWrapper = (<div>{dataGrid._buildBody()}</div>);
            const content = renderer.create(bodyWrapper);

            const result = content.toJSON();

            expect(result).toMatchSnapshot();
        })

        it('render correct grand total content', () => {
            const props = Object.assign({}, defaultProps, { gridData: { ...defaultProps.gridData, GrandTotal: grandTotalData } });
            const dataGrid = new DataGrid(props);
            const grandTotalWrapper = (<div>{dataGrid._buildGrandTotal()}</div>);
            const content = renderer.create(grandTotalWrapper);

            const result = content.toJSON();

            expect(result).toMatchSnapshot();
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
})