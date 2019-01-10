import React from 'react';
import renderer from 'react-test-renderer';
import { DataGrid } from '../DataGrid';
import { Service } from '../../lib/service';
import Report from '@jsroot/lib/report';
import LangHelper from '@jsroot/lib/langHelper';
import { defaultGridProps, gridData } from './TestData';

describe('DataGrid ', () => {
    beforeEach(() => {
        Service.sort = jest.fn();
        Service.showErrorMsg = jest.fn();
        Service.setReportTitle = jest.fn();
        LangHelper.getSingleResource = jest.fn((resource) => resource);
    })

    describe('componentDidUpdate', () => {
        it('previous timestamp is difference from the current one - call _getGridData', () => {
            const props = { params: { Timestamp: 1 } };
            const prevProps = { params: { Timestamp: 2 } };

            const dataGrid = new DataGrid(props);
            dataGrid._getGridData = jest.fn();
            dataGrid.componentDidUpdate(prevProps);
            
            expect(dataGrid._getGridData).toHaveBeenCalled();
        })

        it('previous timestamp is the same with the current one - do not call _getGridData', () => {
            const props = { params: { Timestamp: 1 } };
            const prevProps = { params: { Timestamp: 1 } };

            const dataGrid = new DataGrid(props);
            dataGrid._getGridData = jest.fn();
            dataGrid.componentDidUpdate(prevProps);
            
            expect(dataGrid._getGridData).not.toHaveBeenCalled();
        })
    })

    describe('_getGridData', () => {
        it('fetch data successfully - response data has one row - do not call sort function', () => {
            const props = { params: { SortBy: 'Id', SortOrder: 'asc' }, xsrfToken: '123', ...defaultGridProps };
            const fakeResponse = { data: { Data: [{ Id: 1 }]} };
            const promise = Promise.resolve(fakeResponse);
            Report.fetchGridData = jest.fn(() => promise);

            const dataGrid = new DataGrid(props);
            dataGrid.setState = jest.fn();

            dataGrid._getGridData();
            
            Promise.resolve().then(()=>{
                expect(Service.sort).not.toHaveBeenCalled();
                expect(Service.setReportTitle).toHaveBeenCalled();
            });
        })

        it('fetch data successfully - response data has one row - call sort function', () => {
            const props = { params: { SortBy: 'Id', SortOrder: 'asc' }, xsrfToken: '123', ...defaultGridProps };
            const fakeResponse = { data: { Data: [{ Id: 1 }, { Id: 2 }]} };
            const promise = Promise.resolve(fakeResponse);
            Report.fetchGridData = jest.fn(() => promise);

            const dataGrid = new DataGrid(props);
            dataGrid.setState = jest.fn();

            dataGrid._getGridData();
            
            Promise.resolve().then(()=>{
                expect(Service.sort).toHaveBeenCalled();
                expect(Service.setReportTitle).toHaveBeenCalled();
            });
        })

        it('fetch data failed - show error', () => {
            const props = { params: { SortBy: 'Id', SortOrder: 'asc' }, xsrfToken: '123', ...defaultGridProps };
            const promise = Promise.reject('error');
            Report.fetchGridData = jest.fn(() => promise);

            const dataGrid = new DataGrid(props);
            dataGrid.setState = jest.fn();

            dataGrid._getGridData();
            
            Promise.reject().catch(()=>{
                expect(Service.showErrorMsg).toHaveBeenCalled();
            });
        })
    })

    describe('_onExpandTurnOverColumn', () => {
        it('when call - update state', () => {
            const dataGrid = new DataGrid(defaultGridProps);
            dataGrid.setState = jest.fn((callback) => callback(true));
            dataGrid._onExpandTurnOverColumn({ stopPropagation: jest.fn })

            expect(dataGrid.setState).toHaveBeenCalled();
            expect(dataGrid.state.expandTurnover).toBe(false);
        })
    })

    describe('_onExpandLicenseeColumn', () => {
        it('when call - update state', () => {
            const dataGrid = new DataGrid(defaultGridProps);
            dataGrid.setState = jest.fn((callback) => callback(true));
            dataGrid._onExpandLicenseeColumn({ stopPropagation: jest.fn })

            expect(dataGrid.setState).toHaveBeenCalled();
            expect(dataGrid.state.expandLicensee).toBe(false);
        })
    })

    describe('_sort', () => {
        it('param.SortBy is null - update sort infor for param', () => {
            const props = { params: {}, ...defaultGridProps };
            const sortBy = 'col1';

            const dataGrid = new DataGrid(props);
            dataGrid._sort(sortBy);
            
            expect(props.setParams).toHaveBeenCalledWith({ SortBy: sortBy, SortOrder: 'asc' });
        })

        it('sort with desc order - update SortOrder infor for param', () => {
            const sortBy = 'col1';
            const props = { params: { SortBy: sortBy, SortOrder: 'asc' }, ...defaultGridProps };

            const dataGrid = new DataGrid(props);
            dataGrid._sort(sortBy);
            
            expect(props.setParams).toHaveBeenCalledWith({ SortBy: sortBy, SortOrder: 'desc' });
        })

        it('sort with asc order - update SortOrder infor for param', () => {
            const sortBy = 'col1';
            const props = { params: { SortBy: sortBy, SortOrder: 'desc' }, ...defaultGridProps };

            const dataGrid = new DataGrid(props);
            dataGrid._sort(sortBy);
            
            expect(props.setParams).toHaveBeenCalledWith({ SortBy: sortBy, SortOrder: 'asc' });
        })

        it('no data - do not call sort function', () => {
            const sortBy = 'col1';
            const props = { params: { SortBy: sortBy, SortOrder: 'desc' }, ...defaultGridProps };

            const dataGrid = new DataGrid(props);
            dataGrid._sort(sortBy);
            
            expect(Service.sort).not.toHaveBeenCalled();
        })

        it('griddata has more than 1 row - call sort function', () => {
            const sortBy = 'col1';
            const props = { params: { SortBy: sortBy, SortOrder: 'desc' }, ...defaultGridProps };

            const dataGrid = new DataGrid(props);
            dataGrid.setState = jest.fn((data) => { dataGrid.state.gridData = data });
            
            const fakeResponse = { data: { Data: [{ Id: 1 }, { Id: 2 } ]} };
            const promise = Promise.resolve(fakeResponse);
            Report.fetchGridData = jest.fn(() => promise);

            dataGrid._getGridData();
            
            Promise.resolve().then(()=>{
                dataGrid._sort(sortBy);
                expect(Service.sort).toHaveBeenCalled();
            }).catch(()=>{});
        })
    })

    describe('OnClick on Header  ', () => {
        describe('OnClick on main Header  ', () => {
            const options = {
                createNodeMock: (element) => {
                    return {
                        offsetHeight: 1,
                        offsetTop: 1
                    };
                }
            };
            const fakeResponse = { data: gridData };
            const props = { params: { Timestamp: 1 }, ...defaultGridProps };
            let dataGrid = renderer.create(<DataGrid {...props} />, options);

            beforeAll(() => {
                const promise = Promise.resolve(fakeResponse);
                Report.fetchGridData = jest.fn(() => promise);

                const nextProps = { params: { Timestamp: 2, ProductCodeList: ['SB'] }, ...defaultGridProps };
            
                Promise.resolve().then(()=>{
                    dataGrid.update(<DataGrid {...nextProps} />);
                });
            })

            it('onClick on Date column - call sort function', () => {

                const column = dataGrid.root.findAllByProps({ sortable: true })[0];

                column.props.onClick();

                expect(Service.sort).toHaveBeenCalledWith(fakeResponse.data.Data, 'Date', 'asc');
            })

            it('onClick on TotalTurnOver column - call sort function', () => {

                const column = dataGrid.root.findAllByProps({ sortable: true })[2];

                column.props.onClick();

                expect(Service.sort).toHaveBeenCalledWith(fakeResponse.data.Data, 'TotalTurnOver', 'asc');
            })

            it('onClick on TotalTransaction column - call sort function', () => {

                const column = dataGrid.root.findAllByProps({ sortable: true })[3];

                column.props.onClick();

                expect(Service.sort).toHaveBeenCalledWith(fakeResponse.data.Data, 'TotalTransaction', 'asc');
            })

            it('onClick on CustomerWinLoss column - call sort function', () => {

                const column = dataGrid.root.findAllByProps({ sortable: true })[4];

                column.props.onClick();

                expect(Service.sort).toHaveBeenCalledWith(fakeResponse.data.Data, 'CustomerWinLoss', 'asc');
            })

            it('onClick on TotalLicenseeWL column - call sort function', () => {

                const column = dataGrid.root.findAllByProps({ sortable: true })[5];

                column.props.onClick();

                expect(Service.sort).toHaveBeenCalledWith(fakeResponse.data.Data, 'TotalLicenseeWL', 'asc');
            })
        })

        describe('OnClick on expand Header  ', () => {
            const fakeResponse = { data: gridData };
            let dataGrid;

            beforeAll(() => {
                const options = {
                    createNodeMock: (element) => {
                        return {
                            offsetHeight: 1,
                            offsetTop: 1
                        };
                    }
                };

                const promise = Promise.resolve(fakeResponse);
                Report.fetchGridData = jest.fn(() => promise);

                const props = { params: { Timestamp: 1 }, ...defaultGridProps };
                dataGrid = renderer.create(<DataGrid {...props} />, options);

                dataGrid.root.instance.setState({ expandLicensee: true, expandTurnover: true });

                const nextProps = { params: { Timestamp: 2, ProductCodeList: ['SB', 'NG'] }, ...defaultGridProps };
     
                Promise.resolve().then(()=>{
                    dataGrid.update(<DataGrid {...nextProps} />);
                });
            })

            it('onClick on TurnOverSB column - call sort function', () => {

                const column = dataGrid.root.findAllByProps({ sortable: true })[4];

                column.props.onClick();

                expect(Service.sort).toHaveBeenCalledWith(fakeResponse.data.Data, 'TurnOverSB', 'asc');
            })

            it('onClick on TurnOverNG column - call sort function', () => {

                const column = dataGrid.root.findAllByProps({ sortable: true })[5];

                column.props.onClick();

                expect(Service.sort).toHaveBeenCalledWith(fakeResponse.data.Data, 'TurnOverNG', 'asc');
            })

            it('onClick on TotalTurnOver column - call sort function', () => {

                const column = dataGrid.root.findAllByProps({ sortable: true })[6];

                column.props.onClick();

                expect(Service.sort).toHaveBeenCalledWith(fakeResponse.data.Data, 'TotalTurnOver', 'asc');
            })

            it('onClick on LicenseeWLSB column - call sort function', () => {

                const column = dataGrid.root.findAllByProps({ sortable: true })[7];

                column.props.onClick();

                expect(Service.sort).toHaveBeenCalledWith(fakeResponse.data.Data, 'LicenseeWLSB', 'asc');
            })

            it('onClick on LicenseeWLNG column - call sort function', () => {

                const column = dataGrid.root.findAllByProps({ sortable: true })[8];

                column.props.onClick();

                expect(Service.sort).toHaveBeenCalledWith(fakeResponse.data.Data, 'LicenseeWLNG', 'asc');
            })

            it('onClick on TotalLicenseeWL column - call sort function', () => {

                const column = dataGrid.root.findAllByProps({ sortable: true })[9];

                column.props.onClick();

                expect(Service.sort).toHaveBeenCalledWith(fakeResponse.data.Data, 'TotalLicenseeWL', 'asc');
            })
        })
    })
})