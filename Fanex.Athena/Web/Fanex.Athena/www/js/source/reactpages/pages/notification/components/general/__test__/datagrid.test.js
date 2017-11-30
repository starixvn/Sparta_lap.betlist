import React from 'react';
import renderer from 'react-test-renderer';
import { DataGrid } from '../datagrid';
import Service from '../../../lib/generalService';
import LangHelper from '../../../../../lib/langHelper';

describe('DataGrid ', () => {
    beforeAll(() => {
        LangHelper.getSingleResource = jest.fn((resource) => resource);
        Service.getReportTitle = jest.fn();
    });

    afterAll(() => {
        jest.resetAllMocks();
    })

    describe('render ', () => {
        it('data is undefined - return null', () => {
            const props = { gridData: {} };
            const dataGrid = renderer.create(<DataGrid {...props} />);

            const result = dataGrid.toJSON();

            expect(result).toBeNull();
        })

        it('no data - render No data title', () => {
            const props = { gridData: { Data: [] } };
            const dataGrid = renderer.create(<DataGrid {...props} />);

            const result = dataGrid.toJSON();

            expect(result).toMatchSnapshot();
        })

        it('has data - render table', () => {
            const props = { 
                gridData: { 
                    Data: [{ DateTime: '2017-01-01T12:00:00', Subject: 'Subject', Content: 'Content'}],
                    Total: 1
                },
                param: {
                    PageIndex: 1,
                    PageSize: 10
                }
            };

            const dataGrid = renderer.create(<DataGrid {...props} />);

            const result = dataGrid.toJSON();

            expect(result).toMatchSnapshot();
        })
    })
        
    describe('_buildHeader', () => {
        it('renders correct header', () => {
            const props = { 
                gridData: { 
                    Data: [{ DateTime: '2017-01-01T12:00:00', Subject: 'Subject', Content: 'Content'}],
                    Total: 1
                },
                param: {
                    PageIndex: 1,
                    PageSize: 10
                }
            };
            const grid = new DataGrid(props);
            const headerName = {
                index: 'index',
                type: 'type',
                dateTime: 'dateTime',
                content: 'content'
            };
            Service.getHeaderText = jest.fn(() => headerName);

            const header = renderer.create(grid._buildHeader());
            const result = header.toJSON();

            expect(result.children.length).toEqual(Object.keys(headerName).length);
            result.children.forEach((item, index) => {
                expect(item.innerHtml).toEqual(headerName[index]);
            });
        })
    })

    describe('_buildBody', () => {            
        it('matches with Snapshot', () => {
            const props = { 
                gridData: { 
                    Data: [{ DateTime: '2017-01-01T12:00:00', Subject: 'Subject', Content: 'Content'}],
                    Total: 1
                },
                param: {
                    PageIndex: 1,
                    PageSize: 10
                }
            };
            const grid = new DataGrid(props);

            const body = renderer.create(grid._buildBody()[0]);
            const result = body.toJSON();

            expect(result).toMatchSnapshot();
        })
    })
    
    describe('onPaging', () => {            
        it('calls props functions with correct parameter', () => {
            const props = { 
                gridData: { 
                    Data: [],
                    Total: 0
                },
                param: {
                    PageIndex: 1,
                    PageSize: 10
                },
                xsrfToken: 'token',
                toggleLoader: jest.fn(),
                getGridData: jest.fn(),
                setParam: jest.fn()
            };
            const index = 2;
            const pageSize = 50;
            const expectedParam = {
                PageIndex: index,
                PageSize: pageSize
            };

            const grid = new DataGrid(props);
            grid.onPaging(index, pageSize);

            expect(props.toggleLoader).toHaveBeenCalledWith(true);
            expect(props.getGridData).toHaveBeenCalledWith({
                xsrfToken: props.xsrfToken,
                data: expectedParam
            });
            expect(props.setParam).toHaveBeenCalledWith(expectedParam);
        })
    })

    describe('componentWillReceiveProps', () => {
        it('next grid data is the same with the current - do nothing', () => {
            const props = {
                gridData: {},
                dateRange: {},
                toggleLoader: jest.fn()
            };

            const grid = new DataGrid(props);
            grid.componentWillReceiveProps(props);

            expect(props.toggleLoader).not.toHaveBeenCalledWith();
        })

        it('next grid data is not the same with the current - hide loader', () => {
            const props = {
                gridData: {},
                dateRange: {},
                toggleLoader: jest.fn()
            };
            const nextProps = {
                gridData: {}
            };

            const grid = new DataGrid(props);
            grid.componentWillReceiveProps(nextProps);

            expect(Service.getReportTitle).not.toHaveBeenCalled();
            expect(props.toggleLoader).toHaveBeenCalledWith(false);
        })

        it('next grid data is not the same with the current - param data is existing - update title', () => {
            const props = {
                gridData: {},
                dateRange: {},
                param: {
                    FromData: '',
                    ToDate: ''
                },
                toggleLoader: jest.fn()
            };
            const nextProps = {
                gridData: {},
            };

            const grid = new DataGrid(props);
            grid.componentWillReceiveProps(nextProps);

            expect(Service.getReportTitle).toHaveBeenCalled();
            expect(props.toggleLoader).toHaveBeenCalledWith(false);
        })
    })
})
