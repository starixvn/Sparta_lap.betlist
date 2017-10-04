import React from 'react'
import { DataGrid } from '../datagrid';
import service from '../../lib/service';
import renderer from 'react-test-renderer';

import { defaultProps, totalData, bodyData, grandTotalData } from './fakeData';

const Data = [], Total = [], GrandTotal = {};
const { gridData, dateRange, selectedBaseCurrency, toggleLoader } = defaultProps;
const dataGridProps = {
    gridData,
    toggleLoader,
    dateRange,
    selectedBaseCurrency
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
            const dataGrid = renderer.create(<DataGrid {...dataGridProps} />);

            const result = dataGrid.toJSON();

            expect(result).toMatchSnapshot();
        })

        it('gridData length is 0 - render container with correct report title content', () => {
            const dataGrid = renderer.create(<DataGrid {...dataGridProps} />);
            const { gridData, dateRange, selectedBaseCurrency } = dataGridProps;
            service.getReportTitle = jest.fn((dateRange, selectedBaseCurrency) => {
                return `SubsidiaryPerformance, ${dateRange.fromDate} - ${dateRange.toDate} - ${selectedBaseCurrency}`;
            })

            const nextProps = Object.assign({}, dataGridProps, { gridData: { Data: {} } });
            dataGrid.update(<DataGrid {...nextProps} />);

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

            const dataGrid = new DataGrid(dataGridProps);
            const header = renderer.create(dataGrid._buildHeader());

            const result = header.toJSON();

            expect(result).toMatchSnapshot();
        })

        it('_buildBody - render correct body content', () => {
            const props = Object.assign({}, dataGridProps, { gridData: { ...dataGridProps.gridData, Data: bodyData } });
            const dataGrid = new DataGrid(props);
            const bodyWrapper = (<div>{dataGrid._buildBody()}</div>);
            const content = renderer.create(bodyWrapper);

            const result = content.toJSON();

            expect(result).toMatchSnapshot();
        })

        describe('_buildGrandTotal ', () => {
            it('grandTotal is undefined - return null', () => {
                const props = Object.assign({}, dataGridProps, { gridData: { GrandTotal: null } });
                const dataGrid = new DataGrid(props);
                const content = dataGrid._buildGrandTotal();

                expect(content).toBeNull();
            })

            it('SubsidiaryWL is positive - render correct grand total content', () => {
                const props = Object.assign({}, dataGridProps, { gridData: { ...dataGridProps.gridData, GrandTotal: grandTotalData } });
                const dataGrid = new DataGrid(props);
                const grandTotalWrapper = (<div>{dataGrid._buildGrandTotal()}</div>);
                const content = renderer.create(grandTotalWrapper);

                const result = content.toJSON();

                expect(result).toMatchSnapshot();
            })

            it('SubsidiaryWL is negative - render correct grand total content', () => {
                const negativeGrandTotal = { ...grandTotalData, SubsidiaryWinLoss: -123.98 };
                const props = Object.assign({}, dataGridProps, { gridData: { ...dataGridProps.gridData, GrandTotal: negativeGrandTotal } });
                const dataGrid = new DataGrid(props);
                const grandTotalWrapper = (<div>{dataGrid._buildGrandTotal()}</div>);
                const content = renderer.create(grandTotalWrapper);

                const result = content.toJSON();

                expect(result).toMatchSnapshot();
            })
        })

        it('render correct footer content', () => {
            const props = Object.assign({}, dataGridProps, { gridData: { ...dataGridProps.gridData, Total: totalData } });
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
            const props = Object.assign({}, dataGridProps, { gridData: { ...gridData } });

            const dataGrid = new DataGrid(props);
            const content = dataGrid.render();

            const result = renderer.create(content).toJSON();

            expect(result).toMatchSnapshot();
        })
    })

    describe('componentWillReceiveProps ', () => {
        it('gridData update - toggleLoader function should be called with correct parameter', () => {
            const dataGrid = new DataGrid(dataGridProps);
            const nextProps = Object.assign({}, dataGridProps, { gridData: { Data: { ...bodyData } } });

            dataGrid.componentWillReceiveProps(nextProps);

            expect(dataGrid.props.toggleLoader).toHaveBeenCalledWith(false);
        })

        it('gridData do not update - toggleLoader function should not be called', () => {
            const dataGrid = new DataGrid(dataGridProps);
            const nextProps = Object.assign({}, dataGridProps, { selectedBaseCurrency: "USD" });

            dataGrid.componentWillReceiveProps(nextProps);

            expect(dataGrid.props.toggleLoader).not.toHaveBeenCalled();
        })
    })
})