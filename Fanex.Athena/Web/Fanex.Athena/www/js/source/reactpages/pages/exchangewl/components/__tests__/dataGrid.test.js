import React from 'react'
import { DataGrid } from '../datagrid';
import service from '../../lib/service';
import NumberFormat from '../../../../lib/numberFormat'
import renderer from 'react-test-renderer';
import { ReportLevel, ReportBy } from '../../../../constants/report';

describe('DataGrid ', () => {
    describe('Render grid', () => {
        beforeAll(() => {
            service.getReportTitle = jest.fn(() => 'ExchangeWinLoss');
            NumberFormat.formatDecimal = jest.fn((value) => value);
            NumberFormat.format = jest.fn((value) => value);
        });

        describe('No data', () => {
            it('return null when gridData is undefined', () => {
                const props = { gridData: undefined };
                const dataGrid = renderer.create(<DataGrid {...props} />);

                const result = dataGrid.toJSON();

                expect(result).toBeNull();
            })

            it('gridData length is 0 - render correct content', () => {
                const props = { gridData: { level: 1, data: { DataList: [] }, params: [{}, {}] }, toggleLoader: jest.fn() };
                const dataGrid = renderer.create(<DataGrid {...props} />);

                const result = dataGrid.toJSON();

                expect(result).toMatchSnapshot();
            })

            it('gridData length is 0 - update state - render container with correct report title content', () => {
                const props = { gridData: { level: 1, data: { DataList: [] } }, params: [{}, {}] ,toggleLoader: jest.fn() };

                const dataGrid = renderer.create(<DataGrid {...props} />);
                const nextProps = { gridData: { level: 0, data: { DataList: [] } }, params: [{}, {}] };
                dataGrid.update(<DataGrid {...nextProps} />);

                const result = dataGrid.toJSON();

                expect(result).toMatchSnapshot();
            })
        });

        describe('Has data', () => {
            describe('_buildHeader', () => {
                const headerName = {
                    index: '#',
                    date: 'Date',
                    month: 'Month',
                    externalId: 'ExternalId',
                    systemId: 'SystemId',
                    currency: 'Currency',
                    turnover: 'Turnover',
                    netTurnover: 'NetTurnover',
                    totalTransaction: 'TotalTransaction',
                    customer: 'Customer',
                    customerWL: 'CustomerW_L',
                    customerComm: 'CustomerComm',
                    total: 'Total',
                    licensee: 'Licensee',
                    licenseeWL: 'LicenseeW_L',
                    amountDueTo: 'AmountDueTo',
                    licenseeComm: 'LicenseeComm',
                };

                beforeAll(() => {
                    service.getHeaderText = jest.fn(() => headerName);
                });                

                it('level MAIN - render header with Date column', () => {
                    const props = {
                        isAllowedCustomerComm: false,
                        licenseeId: 1
                    };
                    const grid = new DataGrid(props);

                    const header = renderer.create(<div>{grid._buildHeader(ReportLevel.MAIN, ReportBy.DAILY)}</div>);
                    const result = header.toJSON().children[0];

                    expect(result.children[1].props.title).toEqual(headerName.date);
                    expect(result.children[2].props.title).toEqual(headerName.currency);
                });

                it('level MAIN & report by MONTHLY - render header with Month column', () => {
                    const props = {
                        isAllowedCustomerComm: false,
                        licenseeId: 1
                    };
                    const grid = new DataGrid(props);

                    const header = renderer.create(<div>{grid._buildHeader(ReportLevel.MAIN, ReportBy.MONTHLY)}</div>);
                    const result = header.toJSON().children[0];

                    expect(result.children[1].props.title).toEqual(headerName.month);
                    expect(result.children[2].props.title).toEqual(headerName.currency);
                });

                it('level LICENSEE - render header with SystemId column', () => {
                    const props = {
                        isAllowedCustomerComm: false,
                        licenseeId: 1
                    };
                    const grid = new DataGrid(props);

                    const header = renderer.create(<div>{grid._buildHeader(ReportLevel.LICENSEE, ReportBy.DAILY)}</div>);
                    const result = header.toJSON().children[0];

                    expect(result.children[1].props.title).toEqual(headerName.systemId);
                    expect(result.children[2].props.title).toEqual(headerName.currency);
                });

                it('level CUSTOMER - render header with SystemId & ExternalId column', () => {
                    const props = {
                        isAllowedCustomerComm: false,
                        licenseeId: 1
                    };
                    const grid = new DataGrid(props);

                    const header = renderer.create(<div>{grid._buildHeader(ReportLevel.CUSTOMER, ReportBy.DAILY)}</div>);
                    const result = header.toJSON().children[0];

                    expect(result.children[1].props.title).toEqual(headerName.externalId);
                    expect(result.children[2].props.title).toEqual(headerName.systemId);
                    expect(result.children[3].props.title).toEqual(headerName.currency);
                });

                it('is allowed Customer Comm - render customer columns', () => {
                    const props = {
                        isAllowedCustomerComm: true,
                        licenseeId: 1
                    };
                    const grid = new DataGrid(props);

                    const header = renderer.create(<div>{grid._buildHeader(ReportLevel.LICENSEE, ReportBy.DAILY)}</div>);
                    const result = header.toJSON();

                    expect(result.children[0].children[6].props.title).toEqual(headerName.customer);
                    expect(result.children[1].children[0].props.title).toEqual(headerName.customerWL);
                    expect(result.children[1].children[1].props.title).toEqual(headerName.customerComm);
                    expect(result.children[1].children[2].props.title).toEqual(headerName.total);
                });

                it('is not allowedCustomerComm - do not render customer columns', () => {
                    const props = {
                        isAllowedCustomerComm: false,
                        licenseeId: 1
                    };
                    const grid = new DataGrid(props);

                    const header = renderer.create(<div>{grid._buildHeader(ReportLevel.LICENSEE, ReportBy.DAILY)}</div>);
                    const result = header.toJSON();

                    expect(result.children[0].children[6].props.title).toEqual(headerName.customerWL);
                    expect(result.children[1].children[0].props.title).toEqual(headerName.licenseeWL);
                });
            });

            describe('_buildBody', () => {
                const drillDownLink = '<a>Drill down link</a>';
                const cells = '<td>cell</td>';
                const data = {
                    DataList: [{
                        Date: '2017-01-01T12:00:00',
                        ExternalId: 'ExternalId',
                        SystemId: 'SystemId',
                        CurrencyName: 'CurrencyName',
                        CurrencyId: 1
                    }]
                };

                it('level MAIN - render body with Date column', () => {
                    const props = {
                        isAllowedCustomerComm: false,
                        licenseeId: 1,
                        gridData: { data: data }
                    };
                    const grid = new DataGrid(props);
                    grid._renderDrillDownLink = jest.fn( () => drillDownLink );
                    grid._buildCell = jest.fn( () => cells );
                    const expectedDate = '01/01/2017';

                    const body = renderer.create(<div>{grid._buildBody(ReportLevel.MAIN, ReportBy.DAILY)}</div>);
                    const result = body.toJSON().children[0];

                    expect(result.children[1].props.title).toEqual(expectedDate);
                    expect(result.children[2].props.title).toEqual(data.DataList[0].CurrencyName);
                    expect(result.children[2].children[0].toString()).toEqual(drillDownLink);
                });

                it('level MAIN & report by MONTHLY - render header with Month column', () => {
                    const props = {
                        isAllowedCustomerComm: false,
                        licenseeId: 1,
                        gridData: { data: data }
                    };
                    const grid = new DataGrid(props);
                    grid._renderDrillDownLink = jest.fn( () => drillDownLink );
                    grid._buildCell = jest.fn( () => cells );
                    const expectedMonth = '01/2017';

                    const body = renderer.create(<div>{grid._buildBody(ReportLevel.MAIN, ReportBy.MONTHLY)}</div>);
                    const result = body.toJSON().children[0];

                    expect(result.children[1].props.title).toEqual(expectedMonth);
                    expect(result.children[2].props.title).toEqual(data.DataList[0].CurrencyName);
                    expect(result.children[2].children[0].toString()).toEqual(drillDownLink);
                });

                it('level LICENSEE - render header with SystemId column', () => {
                    const props = {
                        isAllowedCustomerComm: false,
                        licenseeId: 1,
                        gridData: { data: data }
                    };
                    const grid = new DataGrid(props);
                    grid._renderDrillDownLink = jest.fn( () => drillDownLink );
                    grid._buildCell = jest.fn( () => cells );

                    const body = renderer.create(<div>{grid._buildBody(ReportLevel.LICENSEE, ReportBy.DAILY)}</div>);
                    const result = body.toJSON().children[0];

                    expect(result.children[1].props.title).toEqual(data.DataList[0].SystemId);
                    expect(result.children[1].children[0].toString()).toEqual(drillDownLink);
                    expect(result.children[2].props.title).toEqual(data.DataList[0].CurrencyName);                    
                });

                it('level CUSTOMER - render header with SystemId & ExternalId column', () => {
                    const props = {
                        isAllowedCustomerComm: false,
                        licenseeId: 1,
                        gridData: { data: data }
                    };
                    const grid = new DataGrid(props);
                    grid._renderDrillDownLink = jest.fn( () => drillDownLink );
                    grid._buildCell = jest.fn( () => cells );

                    const body = renderer.create(<div>{grid._buildBody(ReportLevel.CUSTOMER, ReportBy.DAILY)}</div>);
                    const result = body.toJSON().children[0];

                    expect(result.children[1].props.title).toEqual(data.DataList[0].ExternalId);
                    expect(result.children[2].props.title).toEqual(data.DataList[0].SystemId);
                    expect(result.children[3].props.title).toEqual(data.DataList[0].CurrencyName);  
                });
            });

            describe('_buildCell', () => {
                const item = {
                    Turnover: 'Turnover',
                    NetTurnover: 'NetTurnover',
                    TotalTransaction: 'TotalTransaction',
                    CustomerWL: 'CustomerWL',
                    CustomerComm: 'CustomerComm',
                    CustomerWLTotal: 'CustomerWLTotal',
                    LicenseeWL: 'LicenseeWL',
                    AmountDueTo: 'AmountDueTo',
                    LicenseeComm: 'LicenseeComm',
                    LicenseeWLTotal: 'LicenseeWLTotal'
                };

                it('is allowed Customer Comm - render customer columns', () => {
                    const props = {
                        isAllowedCustomerComm: true,
                        licenseeId: 1
                    };
                    const grid = new DataGrid(props);

                    const body = renderer.create(<div>{grid._buildCell(item)}</div>);
                    const result = body.toJSON();

                    expect(result.children[3].props.title).toEqual(item.CustomerWL);
                    expect(result.children[4].props.title).toEqual(item.CustomerComm);
                    expect(result.children[5].props.title).toEqual(item.CustomerWLTotal);
                    expect(result.children[6].props.title).toEqual(item.LicenseeWL);
                });

                it('is not allowedCustomerComm - do not render customer columns', () => {
                    const props = {
                        isAllowedCustomerComm: false,
                        licenseeId: 1
                    };
                    const grid = new DataGrid(props);

                    const body = renderer.create(<div>{grid._buildCell(item)}</div>);
                    const result = body.toJSON();

                    expect(result.children[3].props.title).toEqual(item.CustomerWL);
                    expect(result.children[4].props.title).toEqual(item.LicenseeWL);
                });
            });

            describe('_renderDrillDownLink', () => {
                const text = 'text',
                    level = 1,
                    date = '01/01/2017',
                    currencyId = 1,
                    currencyName = 'USD',
                    custId = 1;
                const drillDownOnClickHandle = jest.fn();

                let drillDownLink = {};

                beforeAll(() => {
                    const props = {
                        isAllowedCustomerComm: true,
                        licenseeId: 1
                    };
                    const grid = new DataGrid(props);
                    grid._drillDownData = drillDownOnClickHandle;

                    drillDownLink = renderer.create(grid._renderDrillDownLink(text, level, date, currencyId, currencyName, custId)).toJSON();
                });

                it('return correct content', () => {
                    expect(drillDownLink.children[0].toString()).toEqual(text);
                });

                it('onClick - call drill down method', () => {
                    drillDownLink.props.onClick();

                    expect(drillDownOnClickHandle).toHaveBeenCalledWith(level, date, currencyId, currencyName, custId);
                });
            });
        });
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
});