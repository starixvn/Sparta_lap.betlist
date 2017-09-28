import React from 'react';
import { FormFilter } from '../formFilter';
import renderer from 'react-test-renderer';

import LangHelper from '../../../../lib/langHelper';
import service from '../../lib/service';

import { defaultProps } from './fakeData';

const { subsidiaryList, productOptions, baseCurrencyList, selectedBaseCurrency, dateRange, dateConstraint } = defaultProps;
const formFilterPropsForRender = { subsidiaryList, productOptions, baseCurrencyList, selectedBaseCurrency, dateRange, dateConstraint };

const multipleSelectComponentPath = '../../../../components/multiple-select';
const dateRangeComponentPath = '../../../../components/date-range/index';

describe('FormFilter ', () => {
    beforeEach(() => {
        LangHelper.getSingleResource = jest.fn((resource) => resource);
        service.getReportTitle = jest.fn(() => "Report Title");
    })
    afterEach(() => {
        jest.resetAllMocks();
    })

    describe('render methods', () => {
        beforeAll(() => {
            jest.mock(multipleSelectComponentPath, (props) => (<div {...props}>MultipleSelect</div>));
        })

        afterAll(() => {
            jest.unmock(multipleSelectComponentPath);
        })

        describe('render MultipleSelect components', () => {
            it("_renderSubsidiarySelectList - MultipleSelect props should be correct", () => {
                const formFilter = new FormFilter(formFilterPropsForRender);
                const multipleSelectComponent = formFilter._renderSubsidiarySelectList();

                const expectedResult = {
                    dataSource: formFilterPropsForRender.subsidiaryList,
                    keyField: "ID",
                    valueField: "Name",
                    statusField: "Checked",
                    optionAllLabel: "All",
                    noneSelectedLabel: "SelectOptions"
                }

                const { dataSource, keyField, valueField, statusField, optionAllLabel, noneSelectedLabel } = multipleSelectComponent.props;
                const result = { dataSource, keyField, valueField, statusField, optionAllLabel, noneSelectedLabel };

                expect(result).toEqual(expectedResult);
            });

            it("_renderSubsidiarySelectList - subsidiaryList is empty - return null", () => {
                const props = {...formFilterPropsForRender, subsidiaryList: []};
                const formFilter = new FormFilter(props);
                const multipleSelectComponent = formFilter._renderSubsidiarySelectList();

                expect(multipleSelectComponent).toBeNull();
            });

            it("_renderProductSelectList - MultipleSelect props should be correct", () => {
                const formFilter = new FormFilter(formFilterPropsForRender);
                const multipleSelectComponent = formFilter._renderProductSelectList();

                const expectedResult = {
                    dataSource: formFilterPropsForRender.productOptions,
                    keyField: "Code",
                    valueField: "Name",
                    statusField: "Status",
                    optionAllLabel: "All",
                    noneSelectedLabel: "SelectOptions"
                }

                const { dataSource, keyField, valueField, statusField, optionAllLabel, noneSelectedLabel } = multipleSelectComponent.props;
                const result = { dataSource, keyField, valueField, statusField, optionAllLabel, noneSelectedLabel };

                expect(result).toEqual(expectedResult);
            });

            it("_renderProductSelectList - productOptions is empty - return null", () => {
                const props = {...formFilterPropsForRender, productOptions: []};
                const formFilter = new FormFilter(props);
                const multipleSelectComponent = formFilter._renderProductSelectList();

                expect(multipleSelectComponent).toBeNull();
            });
        })

        describe('Based Currency filter', () => {
            beforeAll(() => {
                jest.mock('react-select', (props) => (<div {...props}>Based Currency Select</div>));
            })
            afterAll(() => {
                jest.unmock('react-select');
            })

            it('_renderBaseCurrencySelectList - component props should be correct', () => {
                const expectedResult = {
                    value: formFilterPropsForRender.selectedBaseCurrency,
                    options: formFilterPropsForRender.baseCurrencyList
                }
                const formFilter = new FormFilter(formFilterPropsForRender);
                const baseCurrency = formFilter._renderBaseCurrencySelectList();

                const { value, options } = baseCurrency.props;
                const result = { value, options };

                expect(result).toEqual(expectedResult);
            })

            it("_renderBaseCurrencySelectList - baseCurrencyList is empty - return null", () => {
                const props = {...formFilterPropsForRender, baseCurrencyList: []};
                const formFilter = new FormFilter(props);
                const multipleSelectComponent = formFilter._renderBaseCurrencySelectList();

                expect(multipleSelectComponent).toBeNull();
            });
        })

        describe('Date Range filter', () => {
            beforeAll(() => {
                jest.mock(dateRangeComponentPath, (props) => (<div {...props}>Based Currency Select</div>));
            })
            afterAll(() => {
                jest.unmock(dateRangeComponentPath);
            })

            it('_renderDateRange - component props should be correct', () => {
                const expectedResult = {
                    startDate: formFilterPropsForRender.dateRange.fromDate,
                    endDate: formFilterPropsForRender.dateRange.toDate,
                    minDate: formFilterPropsForRender.dateConstraint.minDate,
                    maxDate: formFilterPropsForRender.dateConstraint.maxDate,
                    limitedMonthRange: 3
                }

                const formFilter = new FormFilter(formFilterPropsForRender);
                const dateRange = formFilter._renderDateRange();

                const { startDate, endDate, minDate, maxDate, limitedMonthRange } = dateRange.props;
                const result = { startDate, endDate, minDate, maxDate, limitedMonthRange };

                expect(result).toEqual(expectedResult);
            })
        })

        describe('render whole component', () => {
            it('match to snapshot', () => {
                const component = renderer.create(<FormFilter {...formFilterPropsForRender}/>);

                const result = component.toJSON();

                expect(result).toMatchSnapshot()
            })
        })
    })
})