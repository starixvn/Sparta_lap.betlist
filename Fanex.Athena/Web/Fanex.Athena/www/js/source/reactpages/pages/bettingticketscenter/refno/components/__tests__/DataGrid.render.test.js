import React from 'react';
import { DataGrid } from '../DataGrid';
import ShallowRenderer from 'react-test-renderer/shallow';
import AppData from '@jsroot/lib/AppData'
import LangHelper from '@jsroot/lib/langHelper';

jest.mock('@jsroot/lib/AppData');

describe('DataGrid ', () => {
    const props = { toggleLoader: jest.fn(), gridData: [] };

    beforeAll(() => {
        LangHelper.getSingleResource = jest.fn((resource) => resource);
    });

    it('isAllowedCustomerComm is true - render correctly', () => {
        Object.assign(AppData, { isAllowedCustomerComm: true });

        const grid = new DataGrid(props);
        const renderer = new ShallowRenderer();
        const result = renderer.render(grid._buildHeader());

        expect(result).toMatchSnapshot();
    });

    it('isAllowedCustomerComm is false - render correctly', () => {
        Object.assign(AppData, { isAllowedCustomerComm: false });

        const grid = new DataGrid(props);
        const renderer = new ShallowRenderer();
        const result = renderer.render(grid._buildHeader());

        expect(result).toMatchSnapshot();
    });
});