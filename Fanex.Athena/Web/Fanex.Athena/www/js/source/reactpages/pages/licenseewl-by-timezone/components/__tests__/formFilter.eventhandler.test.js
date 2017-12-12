import React from 'react';
import renderer from 'react-test-renderer';
import { FormFilter } from '../formFilter';

import Site from '../../../../lib/site';
import LangHelper from '../../../../lib/langHelper';
import service from '../../lib/service';
import ReportExcel from '../../lib/reportExcel'

import { defaultProps } from './fakeData';

const { gridData, ...rest } = defaultProps;
const formFilterPropsForEventHandler = {...rest };

jest.mock('../../lib/reportExcel');

describe('FormFilter ', () => {
    beforeAll(() => {
        LangHelper.getSingleResource = jest.fn(resource => {
            if (resource === "DateRangeExceedMsg") {
                return "Exceed {0} Months";
            }

            return resource;
        })
    })
    afterAll(() => {
        jest.resetAllMocks();
    })

    describe('Event handler', () => {
        describe('onChange event fire on select component', () => {
            it('onChange in Product filter - setSelectedProduct should be called with correct parameter', () => {
                const formFilter = new FormFilter(formFilterPropsForEventHandler);
                formFilterPropsForEventHandler.setSelectedProduct = jest.fn();

                const productFilter = formFilter._renderProductSelectList();
                productFilter.props.onChange({}, "SB,BA");

                expect(formFilterPropsForEventHandler.setSelectedProduct).toHaveBeenCalledWith("SB,BA");
            })

            it('onChange in time zone filter - setSelectedTimeZone should be called with correct parameter', () => {
                const formFilter = new FormFilter(formFilterPropsForEventHandler);

                const selectList = formFilter._renderTimeZoneSelectList();
                selectList.props.onChange({value: 8});

                expect(formFilterPropsForEventHandler.setSelectedTimeZone).toHaveBeenCalledWith(8);
            })

            it('onChange in ReportBy filter - setSelectedReportBy should be called with correct parameter', () => {
                const formFilter = new FormFilter(formFilterPropsForEventHandler);

                const selectList = formFilter._renderReportBySelectList();
                selectList.props.onChange({value: 8});

                expect(formFilterPropsForEventHandler.setSelectedReportBy).toHaveBeenCalledWith(8);
            })

            it('onChange in Currency filter - setSelectedCurrency should be called with correct parameter', () => {
                const formFilter = new FormFilter(formFilterPropsForEventHandler);

                const selectList = formFilter._renderCurrencySelectList();
                selectList.props.onChange(123);

                expect(formFilterPropsForEventHandler.setSelectedCurrency).toHaveBeenCalledWith(123);
            })

            it('onChange in Base Currency filter - setSelectedBaseCurrency should be called with correct parameter', () => {
                const formFilter = new FormFilter(formFilterPropsForEventHandler);

                const selectList = formFilter._renderBaseCurrencySelectList();
                selectList.props.onChange({value: 123});

                expect(formFilterPropsForEventHandler.setSelectedBaseCurrency).toHaveBeenCalledWith(123);
            })
        })

        describe('onChange in DateRange', () => {
            it('date range is valid month range - setDateRange should be called with correct parameter', () => {
                const formFilter = new FormFilter(formFilterPropsForEventHandler);
                formFilterPropsForEventHandler.setDateRange = jest.fn();
                const date = "09/28/2017"

                const dateRange = formFilter._renderDateRange();
                dateRange.props.onChange(date);

                expect(formFilterPropsForEventHandler.setDateRange).toHaveBeenCalledWith(date);
            })

            it('date range is invalid month range - window.alert should be called with correct parameter', () => {
                const formFilter = new FormFilter(formFilterPropsForEventHandler);
                formFilterPropsForEventHandler.setDateRange = jest.fn();
                const date = "09/28/2017",
                    isValidMonthRange = false;
                window.alert = jest.fn();
                const expectedMessage = "Exceed 3 Months";

                const dateRange = formFilter._renderDateRange();
                dateRange.props.onChange(date, isValidMonthRange);

                expect(window.alert).toHaveBeenCalledWith(expectedMessage);
            })
        })

        describe('IsConvertAll checkbox fired click event - props.changeIsConvertAll should be called with correct parameter', () => {
            const findCheckBoxElement = (children) => {
                for(let i = 0; i < children.length; i++){
                    const child = children[i];

                    if(child.props && child.props.id === "IsConvertAll"){
                        return child;
                    }

                    if(child.children && child.children.length){
                        const result = findCheckBoxElement(child.children);

                        if(result){
                            return result;
                        }
                    }
                }

                return null;
            }
            const formFilter = renderer.create(<FormFilter {...formFilterPropsForEventHandler} />).toJSON();
            const checkBox = findCheckBoxElement(formFilter.children);

            checkBox.props.onChange();
            expect(formFilterPropsForEventHandler.changeIsConvertAll).toHaveBeenCalledWith(!checkBox.props.checked);
        })

        describe('isValidModel', () => {
            it('selectedProduct is not empty - return true', () => {
                const formFilter = new FormFilter(formFilterPropsForEventHandler);

                const result = formFilter.isValidModel;

                expect(result).toBeTruthy();
            })

            it('selectedProduct is empty - return false', () => {
                const props = {...formFilterPropsForEventHandler, selectedProduct: "" };
                const formFilter = new FormFilter(props);

                const result = formFilter.isValidModel;

                expect(result).toBeFalsy();
            })
        })

        describe('submit data', () => {
            describe('_getParams', () => {
                it('props.xsrfToken has no value - return null', () => {
                    const props = {...formFilterPropsForEventHandler, xsrfToken: "" };
                    const formFilter = new FormFilter(props);

                    const result = formFilter._getParams();

                    expect(result).toBeNull();
                })

                it('filter data is invalid - return null', () => {
                    const props = {...formFilterPropsForEventHandler, selectedProduct: "" };
                    const formFilter = new FormFilter(props);

                    const result = formFilter._getParams();

                    expect(result).toBeNull();
                })

                it('props.xsrfToken has value and filter data is valid - return correct param data', () => {
                    const { convertDateToString } = Site;
                    Site.convertDateToString = jest.fn((date) => date);
                    const expectedResult =  { 
                        xsrfToken: formFilterPropsForEventHandler.xsrfToken,
                        data: {
                            BaseCurrency: formFilterPropsForEventHandler.selectedBaseCurrency,
                            CurrencyId: formFilterPropsForEventHandler.selectedCurrency.value,
                            ProductCodeList: formFilterPropsForEventHandler.selectedProduct,
                            IsConvertAll: formFilterPropsForEventHandler.isConvertAll,
                            IsIncludeDrawData: formFilterPropsForEventHandler.isIncludeDrawData,
                            FromDate: formFilterPropsForEventHandler.dateRange.fromDate,
                            ToDate: formFilterPropsForEventHandler.dateRange.toDate,
                            TimeZone: formFilterPropsForEventHandler.selectedTimeZone,
                            ReportBy: formFilterPropsForEventHandler.selectedReportBy,
                            GroupBy: formFilterPropsForEventHandler.groupBy
                        }
                    };
                  
                    const formFilter = new FormFilter(formFilterPropsForEventHandler);

                    const result = formFilter._getParams();
                    expect(result).toEqual(expectedResult);
                })
            })

            describe('submit', () => {
                const event = { preventDefault: jest.fn() };
                it('_getParams return null - do nothing', () => {
                    const props = {...formFilterPropsForEventHandler, selectedProduct: "" };
                    const formFilter = new FormFilter(props);

                    formFilter._getParams = jest.fn(() => null);
                    formFilter.props.toggleLoader = jest.fn();

                    formFilter.submit(event);

                    expect(formFilter.props.toggleLoader).not.toHaveBeenCalled();
                })

                it('props.toggleLoader should be called with correct parameter', () => {
                    const props = formFilterPropsForEventHandler;
                    const formFilter = new FormFilter(props);

                    formFilter._getParams = jest.fn(() => { return {} });
                    formFilter.props.toggleLoader = jest.fn();
                    formFilter.props.getGridData = jest.fn();

                    formFilter.submit(event);

                    expect(formFilter.props.toggleLoader).toHaveBeenCalledWith(true);
                })

                it('props.getGridData should be called with correct parameter', () => {
                    const props = formFilterPropsForEventHandler;
                    const expectedParams = {};
                    const formFilter = new FormFilter(props);

                    formFilter._getParams = jest.fn(() => expectedParams);
                    formFilter.props.toggleLoader = jest.fn();
                    formFilter.props.getGridData = jest.fn();

                    formFilter.submit(event);

                    expect(formFilter.props.getGridData).toHaveBeenCalledWith(expectedParams);
                })
            })

            describe('exportExcel', () => {
                const event = { preventDefault: jest.fn() };
                const fakeReportTitle = "Report Title";

                beforeEach(() => {
                    service.getReportTitle = jest.fn(() => fakeReportTitle);
                })

                afterEach(() => {
                    ReportExcel.mockReset();
                    service.getReportTitle.mockReset();
                })

                it('_getParams return null - do nothing', () => {
                    const props = {...formFilterPropsForEventHandler, selectedProduct: "" };
                    const formFilter = new FormFilter(props);

                    formFilter._getParams = jest.fn(() => null);
                    formFilter.props.toggleLoader = jest.fn();

                    formFilter.exportExcel(event);

                    expect(formFilter.props.toggleLoader).not.toHaveBeenCalled();
                })

                describe('_getParams return data ', () => {
                    const fakeResponse = { 
                        data: { 
                            Data:[1, 2]
                        }
                    };
                    const promise = Promise.resolve(fakeResponse);
                    let formFilter;

                    beforeEach(() => {
                        formFilter = new FormFilter(formFilterPropsForEventHandler);
                        formFilter._getParams = jest.fn(() => { return {} });
                        formFilter.props.toggleLoader = jest.fn();
                        formFilter.props.exportExcel = jest.fn(() => promise);
                    })

                    it('props.toggleLoader should be called with correct parameter', () => {
                        formFilter.exportExcel(event);

                        expect(formFilter.props.toggleLoader.mock.calls[0][0]).toBeTruthy();
                    })

                    it('action return promise - props.exportExcel should be called with correct parameter', () => {
                        const expectedParams = {};

                        formFilter.exportExcel(event);

                        expect(formFilter.props.exportExcel).toHaveBeenCalledWith(expectedParams);
                    })

                    it('action return promise - Excel class should be instantiated with correct parameter', () => {
                        const reportExcelMock = jest.fn();
                        ReportExcel.mockImplementation(reportExcelMock);

                        formFilter.exportExcel(event);

                        Promise.resolve().then(() => {
                            expect(reportExcelMock).toHaveBeenCalledWith(
                                fakeResponse.data, 
                                fakeReportTitle, 
                                formFilterPropsForEventHandler.selectedReportBy,
                                formFilterPropsForEventHandler.groupBy,
                                formFilterPropsForEventHandler.selectedBaseCurrency);
                        })
                    })

                    it('action return promise - props.toggleLoader should be called with correct parameter', () => {
                        formFilter.exportExcel(event);

                        Promise.resolve().then(() => {
                            expect(formFilter.props.toggleLoader.mock.calls[1][0]).toBeFalsy();
                        })
                    })
                })
            })
        })
    })
})