import React from 'react';
import BaseDataGrid from '../BaseDataGrid';
import renderer from 'react-test-renderer';
import LangHelper from '@jsroot/lib/langHelper';
import AppData from '@jsroot/lib/AppData';

jest.mock('@jsroot/lib/AppData');

describe('Render grid', () => {
    describe('No data', () => {
        it('return null when gridData is undefined', () => {          
            const props = { requestParam: {}, setRequestParam: jest.fn() };
            const dataGrid = renderer.create(<BaseDataGrid {...props} />);

            const result = dataGrid.toJSON();

            expect(result).toBeNull();
        });

        it('gridData length is 0 - render correct content', () => {
            const props = { gridData: { Data: []}, requestParam: {}, setRequestParam: jest.fn() };
            const dataGrid = renderer.create(<BaseDataGrid {...props} />);

            const result = dataGrid.toJSON();

            expect(result).toMatchSnapshot();
        });
    });

    describe('Has data', () => {
    const dataItem = {
        Index: 1,
        Platform: "Asia",
        ExternalId: 'ExternalId',
        SystemId: 'SystemId',
        TransTime: "123",
        Choice: 'Choice1',
        Odds: 1.11,
        Stake: 1234,
        WinLoss: 456.34,
        LicenseeWL: 5.3,
        Status: 'Dead',
        Live: 'No'
    };

    let gridData = { Data: [dataItem] };
    it('grid data should be rendered correctly',() => {
        const props = { 
            isAllowedCustomerComm: false,
            gridData: gridData,
            requestParam: { }
        };

        const dataGrid = new BaseDataGrid(props);

        const content = dataGrid.render();

        const result = renderer.create(content).toJSON();

        expect(result).toMatchSnapshot();
    });
});
});