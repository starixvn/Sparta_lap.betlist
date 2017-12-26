import React from 'react';
import { FormFilter } from '../formFilter';
import renderer from 'react-test-renderer';

import LangHelper from '../../../../lib/langHelper';
import service from '../../lib/service';
import ReportExcel from '../../lib/reportExcel';

jest.mock('../../lib/reportExcel');

describe('FormFilter ', () => {
    const props = {
        dateRange: { fromDate: '01/01/2017', toDate: '10/10/2017' },
        dateConstraint: { maxDate: '01/01/2010', minDate: '10/10/2030' },
        currencyList: [{ value: '1', label: '1' }, { value: '2', label: '2' }],
        selectedCurrency: { value: '1', label: '1' },
        reportByOption: [{ value: '1', label: '1' }, { value: '2', label: '2' }],
        selectedReportBy: 1,
        isExcludeTest: true,
        isAllowedCustomerComm: true,
        setParams: jest.fn(),
        toggleLoader: jest.fn(),
        getGridData: jest.fn()
    };
    const reportTitle = 'Report Title';

    beforeEach(() => {
        LangHelper.getSingleResource = jest.fn((resource) => {
            if (resource === 'DateRangeExceedMsg') {
                return 'DateRangeExceedMsg' + ' {0}';
            }

            return resource
        });
        service.getReportTitle = jest.fn(() => reportTitle);
    })

    afterEach(() => {
        jest.resetAllMocks();
    })

    describe('filter component', () => {
        describe('render whole component', () => {
            it('match to snapshot', () => {
                const component = renderer.create(<FormFilter {...props}/>);

                const result = component.toJSON();

                expect(result).toMatchSnapshot();
            })
        })

        describe('_renderDateRange', () => {
            it('component props should be correct', () => {
                const expectedResult = {
                    startDate: props.dateRange.fromDate,
                    endDate: props.dateRange.toDate,
                    minDate: props.dateConstraint.minDate,
                    maxDate: props.dateConstraint.maxDate,
                    limitedMonthRange: 3
                };

                const formFilter = new FormFilter(props);
                const dateRange = formFilter._renderDateRange();

                const { startDate, endDate, minDate, maxDate, limitedMonthRange } = dateRange.props;
                const result = { startDate, endDate, minDate, maxDate, limitedMonthRange };

                expect(result).toEqual(expectedResult);
            })

            it('onChange - is invalid month range - alert warning message', () => {
                window.alert = jest.fn();
                const formFilter = new FormFilter(props);
                const expecteMessage = 'DateRangeExceedMsg 3';

                const dateRange = formFilter._renderDateRange();
                dateRange.props.onChange({}, false);

                expect(window.alert).toHaveBeenCalledWith(expecteMessage);
            })

            it('onChange - is valid month range - change date range state', () => {
                const filterProps = { ...props, setDateRange: jest.fn() };
                const formFilter = new FormFilter(filterProps);
                const newDate = { fromDate: '02/02/2017', toDate: '12/12/2017' };

                const dateRange = formFilter._renderDateRange();
                dateRange.props.onChange(newDate, true);

                expect(filterProps.setDateRange).toHaveBeenCalledWith(newDate);
            })
        })

        describe('_renderCurrencySelectList', () => {
            it('component props should be correct', () => {
                const expectedResult = {
                    value: props.selectedCurrency,
                    options: props.currencyList,
                };

                const formFilter = new FormFilter(props);
                const currencySelect = formFilter._renderCurrencySelectList();

                const { value, options } = currencySelect.props;
                const result = { value, options };

                expect(result).toEqual(expectedResult);
            })

            it('onChange - change selected currency state', () => {
                const filterProps = { ...props, setSelectedCurrency: jest.fn() };
                const formFilter = new FormFilter(filterProps);
                const newData = { value: '2', label: '2' };

                const currencySelect = formFilter._renderCurrencySelectList();
                currencySelect.props.onChange(newData);

                expect(filterProps.setSelectedCurrency).toHaveBeenCalledWith(newData);
            })
        })

        describe('_renderReportByOption', () => {
            it('component props should be correct', () => {
                const expectedResult = props.reportByOption.map((item) => {
                    return {
                        value: item.value,
                        checked: item.value === props.selectedReportBy
                    };
                });

                const formFilter = new FormFilter(props);
                const reportByOption = renderer.create(formFilter._renderReportByOption()).toJSON();

                let result = []
                for (let i = 1; i <= props.reportByOption.length; i++) {
                    const option = reportByOption.children[i];
                    result.push({
                        value: option.children[0].props.value,
                        checked: option.children[0].props.checked
                    });
                }

                expect(result).toEqual(expectedResult);
            })

            it('onChange - new state is not the same with current state - change selected report by state', () => {
                const filterProps = { ...props, setReportBy: jest.fn() };
                const newData = '2';
                const event = {
                    target: { value: newData }
                };

                const formFilter = new FormFilter(filterProps);
                const reportByOption = renderer.create(formFilter._renderReportByOption()).toJSON();
                reportByOption.children[2].children[0].props.onChange(event);

                expect(filterProps.setReportBy).toHaveBeenCalledWith(parseInt(newData));
            })
            
            it('onChange - the new state is NaN - do not change "selected report by" state', () => {
                const filterProps = { ...props, setReportBy: jest.fn() };
                const newData = 'a';
                const event = {
                    target: { value: newData }
                };

                const formFilter = new FormFilter(filterProps);
                const reportByOption = renderer.create(formFilter._renderReportByOption()).toJSON();
                reportByOption.children[2].children[0].props.onChange(event);

                expect(filterProps.setReportBy).not.toHaveBeenCalled();
            })

            it('onChange - new state is the same with current state - do not change "selected report by" state', () => {
                const filterProps = { ...props, setReportBy: jest.fn() };
                const newData = '1';
                const event = {
                    target: { value: newData }
                };

                const formFilter = new FormFilter(filterProps);
                const reportByOption = renderer.create(formFilter._renderReportByOption()).toJSON();
                reportByOption.children[1].children[0].props.onChange(event);

                expect(filterProps.setReportBy).not.toHaveBeenCalled();
            })
        })

        describe('_renderExcludeTestDataCheckBox', () => {
            it('component props should be correct', () => {
                const expectedResult = {
                    checked: props.isExcludeTest,
                    htmlFor: 'ExcludeTest'
                }

                const formFilter = new FormFilter(props);
                const reportByOption = renderer.create(formFilter._renderExcludeTestDataCheckBox()).toJSON();

                expect(reportByOption.children[0].props.checked).toEqual(expectedResult.checked);
                expect(reportByOption.children[1].props.htmlFor).toEqual(expectedResult.htmlFor);
            })

            it('onChange - change to new state', () => {
                const filterProps = { ...props, changeIsExcludeTest: jest.fn() };
                const expectedNewState = !props.isExcludeTest;

                const formFilter = new FormFilter(filterProps);
                const reportByOption = renderer.create(formFilter._renderExcludeTestDataCheckBox()).toJSON();
                reportByOption.children[0].props.onChange();

                expect(filterProps.changeIsExcludeTest).toHaveBeenCalledWith(expectedNewState);
            })
        })
    });

    describe('filter method', () => {
        describe('submit', () => {
            it('parameter is null - do nothing', () => {
                const formFilter = new FormFilter(props);
                formFilter._getParams = jest.fn((level) => { return null; });

                formFilter.submit({
                    preventDefault: jest.fn()
                });

                expect(props.toggleLoader).not.toHaveBeenCalled();
                expect(props.getGridData).not.toHaveBeenCalled();
            })

            it('get grid data', () => {
                const formFilter = new FormFilter({ ...props, params:[] });
                const params = { data: {} };
                formFilter._getParams = jest.fn((level) => { return params; });

                formFilter.submit({
                    preventDefault: jest.fn()
                });

                expect(props.toggleLoader).toHaveBeenCalledWith(true);
                expect(props.getGridData).toHaveBeenCalledWith(params);
                expect(props.setParams).toHaveBeenCalledWith([params.data]);
            })

            it('_getParams - xsrfToken is null - retun null', () => {
                const formFilter = new FormFilter({ ...props, xsrfToken: null });

                const result = formFilter._getParams();

                expect(result).toBeNull();
            })

            it('_getParams - level 0 - get data from props', () => {
                const filterProps = { ...props, xsrfToken: 'token', params: [] };
                const formFilter = new FormFilter(filterProps);
                const expectedResult = {
                    xsrfToken: filterProps.xsrfToken,
                    data: {
                        CurrencyId: filterProps.selectedCurrency.value,
                        CurrencyName: filterProps.selectedCurrency.label,
                        BaseCurrency: "",
                        FromDate: filterProps.dateRange.fromDate,
                        ToDate: filterProps.dateRange.toDate,
                        ReportBy: filterProps.selectedReportBy,
                        ExcludeTestData: filterProps.isExcludeTest,
                        Level: 0
                    }
                };

                const result = formFilter._getParams(0);

                expect(result).toEqual(expectedResult);
            })

            it('_getParams - level 0 - Currency is Base - add value for BaseCurrency prop', () => {
                const filterProps = { ...props, xsrfToken: 'token', params: [], selectedCurrency: { value: -1, label: 'Base' } };
                const formFilter = new FormFilter(filterProps);
                const expectedResult = {
                    xsrfToken: filterProps.xsrfToken,
                    data: {
                        CurrencyId: filterProps.selectedCurrency.value,
                        CurrencyName: 'EUR',
                        BaseCurrency: 'EUR',
                        FromDate: filterProps.dateRange.fromDate,
                        ToDate: filterProps.dateRange.toDate,
                        ReportBy: filterProps.selectedReportBy,
                        ExcludeTestData: filterProps.isExcludeTest,
                        Level: 0
                    }
                };

                const result = formFilter._getParams(0);

                expect(result).toEqual(expectedResult);
            })

            it('_getParams - level 1 - get data from props.params', () => {
                const filterProps = { ...props, xsrfToken: 'token', params: [1, 2] };
                const formFilter = new FormFilter(filterProps);
                const expectedResult = {
                    xsrfToken: filterProps.xsrfToken,
                    data: filterProps.params[1]
                };

                const result = formFilter._getParams(1);

                expect(result).toEqual(expectedResult);
            })

            it('_getParams - level 1 - level 1 param is underfined - level 0 param', () => {
                const filterProps = { ...props, xsrfToken: 'token', params: [1] };
                const formFilter = new FormFilter(filterProps);
                const expectedResult = {
                    xsrfToken: filterProps.xsrfToken,
                    data: filterProps.params[0]
                };

                const result = formFilter._getParams(1);

                expect(result).toEqual(expectedResult);
            })
        });

        describe('exportExcel', () => {
            it('_getParams return null - do nothing', () => {
                const filterProps = { ...props, gridData: {} };
                const formFilter = new FormFilter(filterProps);
                formFilter._getParams = jest.fn((level) => { return null; });

                formFilter.exportExcel({
                    preventDefault: jest.fn()
                });

                expect(props.toggleLoader).not.toHaveBeenCalled();
            })

            it('level 0 - export data with correct parameter', () => {
                const fakeResponse = { data: [1, 2] };
                const promise = Promise.resolve(fakeResponse);
                const params = { data: 1 };

                const filterProps = { ...props, xsrfToken: 'token', gridData: {}, exportExcel: jest.fn(() => promise) };
                const formFilter = new FormFilter(filterProps);
                formFilter._getParams = jest.fn((level) => { return params; });
                formFilter._doExportExcel = jest.fn();

                formFilter.exportExcel({
                    preventDefault: jest.fn()
                });

                Promise.resolve().then(() => {
                    expect(formFilter._doExportExcel).toHaveBeenCalledWith(params.data, props.isAllowedCustomerComm, fakeResponse.data);
                });             
            })

            it('level 1 - export data with correct parameter', () => {
                const params = { data: 1 };

                const filterProps = { ...props, xsrfToken: 'token', gridData: { level: 1, data: [] } };
                const formFilter = new FormFilter(filterProps);
                formFilter._getParams = jest.fn((level) => { return params; });
                formFilter._doExportExcel = jest.fn();

                formFilter.exportExcel({
                    preventDefault: jest.fn()
                });

                expect(formFilter._doExportExcel).toHaveBeenCalledWith(params.data, props.isAllowedCustomerComm, filterProps.gridData.data);          
            })

            it('_doExportExcel - call export method of Excel class', () => {
                const exportFnMock = jest.fn();
                const reportExcelMock = jest.fn(() => {
                    return {
                        export: exportFnMock
                    }
                });            
                ReportExcel.mockImplementation(reportExcelMock);

                const formFilter = new FormFilter(props);
                const param = {
                    FromDate: '01/01/2017',
                    ToDate: '01/02/2017',
                    CurrencyName: 'USD',
                    CurrencyId: 0,
                    Level: 0,
                    ReportBy: 1,
                    BaseCurrency: 'USD'
                };
                const isAllowedCustomerComm = true;
                const data = [];

                formFilter._doExportExcel(param, isAllowedCustomerComm, data );

                expect(reportExcelMock).toHaveBeenCalledWith(data, reportTitle, param.Level, isAllowedCustomerComm, param.ReportBy);
                expect(exportFnMock.mock.calls.length).toBe(1);
            })
        });
    });
});