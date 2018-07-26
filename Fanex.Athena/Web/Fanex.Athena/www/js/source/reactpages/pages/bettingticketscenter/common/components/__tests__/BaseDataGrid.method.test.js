import React from 'react';
import axios from 'axios'

import BaseDataGrid from '../BaseDataGrid';

const DefaultProps = { 
    requestParam: {},
    setRequestParam: jest.fn()
};
describe('BaseDataGrid methods ', () => {
    it('componentDidMount - call setRequestParam action with PageSize: 50', () => { 
        const dataGrid = new BaseDataGrid(DefaultProps);
            
        dataGrid.componentDidMount();

        expect(DefaultProps.setRequestParam).toHaveBeenCalledWith({ PageSize: 50 });
    });

    it('onPaging(index, size) - call setRequestParam action with PageSize: 50', () => { 
        const PageIndex = 101;
        const PageSize = 199;
        const dataGrid = new BaseDataGrid(DefaultProps);
        dataGrid._getData = jest.fn();

        dataGrid.onPaging(PageIndex, PageSize);

        expect(dataGrid._getData).toHaveBeenCalledWith({ PageIndex: PageIndex, PageSize: PageSize });
    });

    it('_getData(param) - call relevent action methods', () => { 
        const param = {};
        const props = {
            toggleLoader: jest.fn(),
            getGridData: jest.fn(),
            setRequestParam: jest.fn()
        };
        const dataGrid = new BaseDataGrid(props);
        
        dataGrid._getData(param);

        expect(props.toggleLoader).toHaveBeenCalledWith(true);
        expect(props.getGridData).toHaveBeenCalledWith(undefined, param);
        expect(props.setRequestParam).toHaveBeenCalledWith(param);
    });

    it('_sort(sortBy) - grid was NOT sorted: call _getData() with correct param', () => { 
        const SortBy = "stake";
        const props = { requestParam: {} };
        const dataGrid = new BaseDataGrid(props);
        dataGrid._getData = jest.fn();

        dataGrid._sort(SortBy);

        expect(dataGrid._getData).toHaveBeenCalledWith({SortBy: SortBy, SortOrder: 'asc'});
    });

    it('_sort(sortBy) - grid was sorted: call _getData() with correct param', () => { 
        const SortBy = "stake";
        const props = { requestParam: { SortBy: 'stake', SortOrder: 'asc' } };
        const dataGrid = new BaseDataGrid(props);
        dataGrid._getData = jest.fn();

        dataGrid._sort(SortBy);

        expect(dataGrid._getData).toHaveBeenCalledWith({SortBy: SortBy, SortOrder: 'desc'});
    });

    it('_sort(sortBy) - grid was sorted: call _getData() with correct param', () => { 
        const SortBy = "stake";
        const props = { requestParam: { SortBy: 'stake', SortOrder: 'desc' } };
        const dataGrid = new BaseDataGrid(props);
        dataGrid._getData = jest.fn();

        dataGrid._sort(SortBy);

        expect(dataGrid._getData).toHaveBeenCalledWith({SortBy: SortBy, SortOrder: 'asc'});
    });

    it('_sort(sortBy) - grid was sorted: call _getData() with correct param', () => { 
        const SortBy = "winloss";
        const props = { requestParam: { SortBy: 'stake', SortOrder: 'desc' } };
        const dataGrid = new BaseDataGrid(props);
        dataGrid._getData = jest.fn();

        dataGrid._sort(SortBy);

        expect(dataGrid._getData).toHaveBeenCalledWith({SortBy: SortBy, SortOrder: 'asc'});
    });
});