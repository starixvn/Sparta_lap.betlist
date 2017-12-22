import React from 'react'
import { DataGrid } from '../datagrid';
import service from '../../lib/service';
import NumberFormat from '../../../../lib/numberFormat'
import renderer from 'react-test-renderer';
import { ReportLevel, ReportBy } from '../../../../constants/report';
import { Licensee } from '../../../../constants/licensee';

describe('DataGrid methods ', () => {
    beforeAll(() => {
        service.getReportTitle = jest.fn(() => 'ExchangeWinLoss');
    });

    describe('componentWillReceiveProps ', () => {
        it('gridData update - get title report with correct parameter', () => {
            const level = 0;
            const dataGridProps = { 
                gridData: { level: 0, data: { DataList: [] } },
                params: [{ FromDate: '01/01/2017', ToDate: '02/01/2017', CurrencyName: 'USD', CurrencyId: 0 }, {}],
                toggleLoader: jest.fn() 
            };

            const dataGrid = new DataGrid(dataGridProps);
            const nextProps = Object.assign({}, dataGridProps, { gridData: { level: level, data: { DataList: ['change'] }}});

            dataGrid.componentWillReceiveProps(nextProps);

            expect(service.getReportTitle).toHaveBeenCalledWith({ 
                fromDate: dataGridProps.params[level].FromDate, 
                toDate: dataGridProps.params[level].ToDate }, 
                dataGridProps.params[level].CurrencyName, 
                dataGridProps.params[level].CurrencyId === 0);
        })

        it('gridData do not update - do not thing', () => {
            const dataGridProps = { 
                gridData: { level: 0, data: { DataList: [] } },
                toggleLoader: jest.fn() 
            };

            const dataGrid = new DataGrid(dataGridProps);
            const nextProps = Object.assign({}, dataGridProps, { selectedCurrency: 'EUR' });

            dataGrid.componentWillReceiveProps(dataGridProps);

            expect(dataGrid.props.toggleLoader).not.toHaveBeenCalled();
        })
    })

    describe('_getDrillDownParams ', () => {
        it('xsrfToken is invalid - return null', () => {
            const dataGrid = new DataGrid({});

            const result = dataGrid._getDrillDownParams();

            expect(result).toBeNull();
        })
    })
});