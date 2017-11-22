import React from 'react';
import renderer from 'react-test-renderer';
import { DataGrid } from '../datagrid';
import Service from '../../../lib/resultAmendmentService';
import LangHelper from '../../../../../lib/langHelper';

describe('DataGrid ', () => {
    beforeAll(() => {
        LangHelper.getSingleResource = jest.fn((resource) => resource);
        Service.getReportTitle = jest.fn((dateRange) => dateRange.fromDate + " - " + dateRange.toDate );
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
                    Data: [{ DateTime: new Date(), Subject: 'Subject', Content: 'Content' }],
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
})
