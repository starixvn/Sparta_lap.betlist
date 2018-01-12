import React from 'react';
import { DataGrid } from '../DataGrid';
import service from '../../lib/service';
import NumberFormat from '../../../../lib/numberFormat';
import renderer from 'react-test-renderer';
import { ReportLevel, ReportBy } from '../../../../constants/report';
import { Licensee } from '../../../../constants/licensee';

describe('DataGrid methods ', () => {
    beforeAll(() => {
        service.getReportTitle = jest.fn(() => 'ExchangeWinLoss');
    });

    describe('componentWillReceiveProps ', () => {
        it('gridData update - get title report with correct parameter', () => {
            const level = ReportLevel.MAIN;
            const dataGridProps = { 
                gridData: {},                
                toggleLoader: jest.fn() 
            };

            const dataGrid = new DataGrid(dataGridProps);
            dataGrid.drillDownParams = [dataGridProps.params];
            const nextProps = Object.assign({}, 
                dataGridProps, 
                { 
                    gridData: { level: level, data: { DataList: ['change'] } },
                    params: { FromDate: '01/01/2017', ToDate: '02/01/2017', CurrencyName: 'USD', CurrencyId: 0 }
                });

            dataGrid.componentWillReceiveProps(nextProps);

            expect(dataGrid.drillDownParams[ReportLevel.MAIN]).toEqual(nextProps.params);

            expect(service.getReportTitle).toHaveBeenCalledWith({ 
                fromDate: dataGrid.drillDownParams[level].FromDate, 
                toDate: dataGrid.drillDownParams[level].ToDate }, 
                dataGrid.drillDownParams[level].CurrencyName, 
                dataGrid.drillDownParams[level].CurrencyId === 0);
        })

        it('gridData do not update - do not thing', () => {
            const dataGridProps = { 
                gridData: { level: 0, data: { DataList: [] } },
                toggleLoader: jest.fn() 
            };

            const dataGrid = new DataGrid(dataGridProps);
            const nextProps = Object.assign({}, dataGridProps, { selectedCurrency: 'EUR' });

            dataGrid.componentWillReceiveProps(nextProps);

            expect(dataGrid.props.toggleLoader).not.toHaveBeenCalled();
        })

        it('props.params update - update drill down params at level MAIN', () => {
            const dataGridProps = { 
                gridData: { level: 0, data: { DataList: [] } },
                params: [{ FromDate: '01/01/2017', ToDate: '02/01/2017', CurrencyName: 'USD', CurrencyId: 0 }, {}],
                toggleLoader: jest.fn() 
            };

            const dataGrid = new DataGrid(dataGridProps);
            const nextProps = Object.assign({}, dataGridProps, { params: 'change' });

            dataGrid.componentWillReceiveProps(nextProps);

            expect(dataGrid.drillDownParams[ReportLevel.MAIN]).toEqual(nextProps.params);
        })
    })

    describe('_getDrillDownParams ', () => {
        it('xsrfToken is invalid - return null', () => {
            const dataGrid = new DataGrid({});

            const result = dataGrid._getDrillDownParams();

            expect(result).toBeNull();
        })

        it('Main level params is invalid - return null', () => {
            const props = {
                xsrfToken: 'token',
                params: []
            };

            const dataGrid = new DataGrid(props);

            const result = dataGrid._getDrillDownParams();

            expect(result).toBeNull();
        })

        it('drill down to next level - report by daily - FromDate/ToDate is the same with drill down date', () => {
            const mainParam = {
                ReportBy: ReportBy.DAILY,
                FromDate: '01/01/2017',
                ToDate: '02/01/2017'
            };
            const props = {
                xsrfToken: 'token',
                params: mainParam,
            };
            const level = 1, 
                currencyId = 1, 
                currencyName = 'USD', 
                custId = 2, 
                drillDownDate = '01/25/2017';
            const expectedResult = {
                ...mainParam,
                CurrencyId: currencyId,
                CurrencyName: currencyName,
                FromDate: drillDownDate,
                ToDate: drillDownDate,
                Level: level,
                CustId: custId
            };

            const dataGrid = new DataGrid(props);
            dataGrid.drillDownParams[ReportLevel.MAIN] = mainParam;

            const result = dataGrid._getDrillDownParams(level, drillDownDate, currencyId, currencyName, custId);

            expect(result.data).toEqual(expectedResult);
        })

        it('drill down to next level - report by monthly - FromDate/ToDate is calculated correctly', () => {
            const mainParam = {
                ReportBy: ReportBy.MONTHLY,
                FromDate: '01/01/2017',
                ToDate: '02/10/2017'
            };
            const props = {
                xsrfToken: 'token',
                params: mainParam,
            };
            const level = 1, 
                currencyId = 1, 
                currencyName = 'USD', 
                custId = 2, 
                drillDownDate = '02/05/2017';
            const expectedResult = {
                ...mainParam,
                CurrencyId: currencyId,
                CurrencyName: currencyName,
                FromDate: '02/01/2017',
                ToDate: mainParam.ToDate,
                Level: level,
                CustId: custId
            };

            const dataGrid = new DataGrid(props);
            dataGrid.drillDownParams[ReportLevel.MAIN] = mainParam;

            const result = dataGrid._getDrillDownParams(level, drillDownDate, currencyId, currencyName, custId);

            expect(result.data).toEqual(expectedResult);
        })

        it('drill down to next level - report by monthly - FromDate/ToDate is calculated correctly(2)', () => {
            const mainParam = {
                ReportBy: ReportBy.MONTHLY,
                FromDate: '01/15/2017',
                ToDate: '02/10/2017'
            };
            const props = {
                xsrfToken: 'token',
                params: mainParam,
            };
            const level = 1, 
                currencyId = 1, 
                currencyName = 'USD', 
                custId = 2, 
                drillDownDate = '01/20/2017';
            const expectedResult = {
                ...mainParam,
                    CurrencyId: currencyId,
                CurrencyName: currencyName,
                FromDate: mainParam.FromDate,
                ToDate: '01/31/2017',
                Level: level,
                CustId: custId
            };

            const dataGrid = new DataGrid(props);
            dataGrid.drillDownParams[ReportLevel.MAIN] = mainParam;

            const result = dataGrid._getDrillDownParams(level, drillDownDate, currencyId, currencyName, custId);

            expect(result.data).toEqual(expectedResult);
        })

        it('drill down to next level - update drill down params', () => {
            const mainParam = {
                ReportBy: ReportBy.DAILY,
                FromDate: '01/15/2017',
                ToDate: '02/10/2017'
            };
            const props = {
                xsrfToken: 'token',
                params: mainParam,
            };
            const level = 1, 
                currencyId = 1, 
                currencyName = 'USD', 
                custId = 2, 
                drillDownDate = '01/20/2017';
            const expectedResult = {
                ...mainParam,
                CurrencyId: currencyId,
                CurrencyName: currencyName,
                FromDate: drillDownDate,
                ToDate: drillDownDate,
                Level: level,
                CustId: custId
            };
            const expectedParam = [mainParam, expectedResult];
                        
            const dataGrid = new DataGrid(props);
            dataGrid.drillDownParams[ReportLevel.MAIN] = mainParam;

            const result = dataGrid._getDrillDownParams(level, drillDownDate, currencyId, currencyName, custId);

            expect(dataGrid.drillDownParams).toEqual(expectedParam);
        })
    })

    describe('_drillDownData ', () => {
        it('drill down level is current level - do notthing', () => {
            const level = 1;
            const props = {
                gridData: { level: level },
                toggleLoader: jest.fn()
            };

            const dataGrid = new DataGrid(props);
            dataGrid._drillDownData(level);

            expect(props.toggleLoader).not.toHaveBeenCalled();
        })

        it('drill down level is greater than Customer level - do notthing', () => {
            const level = ReportLevel.CUSTOMER + 1;
            const props = {
                gridData: { level: level },
                toggleLoader: jest.fn()
            };

            const dataGrid = new DataGrid(props);
            dataGrid._drillDownData(level);

            expect(props.toggleLoader).not.toHaveBeenCalled();
        })

        it('drill down level is less than Main level - do notthing', () => {
            const level = ReportLevel.MAIN - 1;
            const props = {
                gridData: { level: level },
                toggleLoader: jest.fn()
            };

            const dataGrid = new DataGrid(props);
            dataGrid._drillDownData(level);

            expect(props.toggleLoader).not.toHaveBeenCalled();
        })

        it('drill down to previous level - get data of previous level for Grid data - data is invalid - do nothing', () => {
            const level = 0;
            const props = {
                gridData: { level: level + 1 },
                toggleLoader: jest.fn(),
                setGridData: jest.fn(),
                levelGridData: []
            };

            const dataGrid = new DataGrid(props);
            dataGrid._drillDownData(level);

            expect(props.setGridData).not.toHaveBeenCalled();
        })

        it('drill down to previous level - get data of previous level for Grid data', () => {
            const level = 0;
            const previousData = { index: 1 };
            const props = {
                gridData: { level: level + 1 },
                toggleLoader: jest.fn(),
                setGridData: jest.fn(),                
            };

            const dataGrid = new DataGrid(props);
            dataGrid.levelGridData = [previousData];
            dataGrid._drillDownData(level);

            expect(props.setGridData).toHaveBeenCalledWith({
                data: previousData,
                level: level
            });
        })

        it('drill down to next level - call getGridData - params is invalid - do nothing', () => {
            const level = 1;
            const previousData = { index: 1 };
            const props = {
                gridData: { level: level - 1 },
                toggleLoader: jest.fn()
            };

            const dataGrid = new DataGrid(props);
            dataGrid._getDrillDownParams = jest.fn(() => { return null; });

            dataGrid._drillDownData(level);

            expect(props.toggleLoader).not.toHaveBeenCalled();
        })

        it('drill down to next level - save current Grid data', () => {
            const level = 1;
            const currentData = { index: 1 };
            const props = {
                gridData: { level: level - 1, data: currentData },
                toggleLoader: jest.fn(),
                getGridData: jest.fn()
            };

            const dataGrid = new DataGrid(props);
            dataGrid._getDrillDownParams = jest.fn(() => { return { level: level }; });

            dataGrid._drillDownData(level);

            expect(dataGrid.levelGridData).toEqual([currentData]);
        })

        it('drill down to next level - call getGridData', () => {
            const level = 1;           
            const props = {
                gridData: { level: level - 1, data: { index: 1 } },
                toggleLoader: jest.fn(),
                getGridData: jest.fn()
            };

            const dataGrid = new DataGrid(props);
            const params = { level: level };
            dataGrid._getDrillDownParams = jest.fn(() => { return params; });

            dataGrid._drillDownData(level);
            const actualParams = props.getGridData.mock.calls[0][0];

            expect(props.getGridData).toHaveBeenCalled();
            expect(actualParams).toEqual(params);
        })

        it('drill down to next level - alert error when getting data failed', () => {
            const level = 1;           
            const props = {
                gridData: { level: level - 1, data: { index: 1 } },
                toggleLoader: jest.fn(),
                getGridData: jest.fn()
            };
            service.showErrorMsg = jest.fn();

            const dataGrid = new DataGrid(props);
            const params = { level: level };
            dataGrid._getDrillDownParams = jest.fn(() => { return params; });

            dataGrid._drillDownData(level);
            const errorHandler = props.getGridData.mock.calls[0][1];
            errorHandler();

            expect(service.showErrorMsg).toHaveBeenCalled();
            expect(props.toggleLoader).toHaveBeenCalledWith(false);
        })
    })
});