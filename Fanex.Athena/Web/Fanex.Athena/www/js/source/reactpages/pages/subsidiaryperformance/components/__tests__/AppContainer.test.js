import React from 'react'

import service from '../../lib/service'
import { AppContainer } from '../AppContainer'

const defaultProps = {
    isLoading: false,
    dateRange: {},
    selectedBaseCurrency: "UUS"
};

describe('AppContainer ', () => {
    it('reportTitle should be null after constructing', () => {
        const appContainer = new AppContainer(defaultProps);

        expect(appContainer.reportTitle).toBe(null);
    })

    describe('componentWillReceiveProps', () => {
        it('Is not loading data - do nothing', () => {
            service.getReportTitle = jest.fn();
            const appContainer = new AppContainer(defaultProps);

            const nextProps = Object.assign({}, defaultProps, { isLoading: false });
            appContainer.componentWillReceiveProps(nextProps);

            expect(service.getReportTitle).not.toHaveBeenCalled();
        })

        it('Is loading data but not complete yet - do nothing', () => {
            service.getReportTitle = jest.fn();
            const props = Object.assign({}, defaultProps, { isLoading: true });
            const appContainer = new AppContainer(props);

            const nextProps = Object.assign({}, defaultProps, { isLoading: true });
            appContainer.componentWillReceiveProps(nextProps);

            expect(service.getReportTitle).not.toHaveBeenCalled();
        })

        it('Complete loading data - service.getReportTitle should be called with correct parameter', () => {
            service.getReportTitle = jest.fn();
            const props = Object.assign({}, defaultProps, { isLoading: true });
            const appContainer = new AppContainer(props);

            const nextProps = Object.assign({}, defaultProps, { isLoading: false });
            appContainer.componentWillReceiveProps(nextProps);

            expect(service.getReportTitle).toHaveBeenCalledWith(nextProps.dateRange, nextProps.selectedBaseCurrency);
        })
    })

    describe('render', () => {
        it('Loader should has correct prop', () => {
            const appContainer = new AppContainer(defaultProps);
            const tree = appContainer.render();

            const loader = tree.props.children[0];
            const result = loader.props.isLoading;

            expect(result).toBe(false);
        })

        it('FormFilter should has correct type', () => {
            const appContainer = new AppContainer(defaultProps);
            const tree = appContainer.render();

            const formFilter = tree.props.children[1];
            const result = formFilter.type.displayName;

            expect(result).toBe("Connect(FormFilter)");
        })

        it('result container with class "result-content" should be existed with correct index', () => {
            const appContainer = new AppContainer(defaultProps);
            const tree = appContainer.render();

            const resultContainer = tree.props.children[2];
            const result = resultContainer.props.className;

            expect(result).toBe("result-content");
        })

        it("reportTitle is null - first child in result container should be null", () => {
            const appContainer = new AppContainer(defaultProps);
            const tree = appContainer.render();

            const resultContainer = tree.props.children[2];
            const reportTitle = resultContainer.props.children[0];

            expect(reportTitle).toBe(null);
        })

        it("reportTitle has value - first child in result container should be ReportTitle component with correct prop", () => {
            const expectedReportTitleContent = "Title";
            const appContainer = new AppContainer(defaultProps);
            appContainer.reportTitle = expectedReportTitleContent;
            const tree = appContainer.render();

            const resultContainer = tree.props.children[2];
            const reportTitle = resultContainer.props.children[0];
            const content = reportTitle.props.content;

            expect(reportTitle).not.toBe(null);
            expect(content).toBe(expectedReportTitleContent);
        })

        it("second child in result container should be DataGrid", () => {
            const appContainer = new AppContainer(defaultProps);
            const tree = appContainer.render();

            const resultContainer = tree.props.children[2];
            const dataGrid = resultContainer.props.children[1];

            const result = dataGrid.type.displayName;

            expect(result).toBe("Connect(DataGrid)");
        })
    })
})