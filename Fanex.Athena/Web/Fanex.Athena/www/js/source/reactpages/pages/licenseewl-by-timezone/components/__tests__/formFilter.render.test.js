import React from 'react';
import { FormFilter } from '../formFilter';
import renderer from 'react-test-renderer';

import LangHelper from '../../../../lib/langHelper';
import service from '../../lib/service';

import { defaultProps } from './fakeData';

const formFilterPropsForRender = { ...defaultProps };

const multipleSelectComponentPath = '../../../../components/multiple-select';
const dateRangeComponentPath = '../../../../components/date-range/index';
const userGuidePath = '../../../../components/UserGuide';

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
                const props = { ...formFilterPropsForRender, productOptions: [] };
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
                const props = { ...formFilterPropsForRender, baseCurrencyList: [] };
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

        describe('User Guide', () => {
            beforeAll(() => {
                jest.mock(userGuidePath, (props) => (<div {...props}></div>));
            })
            afterAll(() => {
                jest.unmock(userGuidePath);
            })

            it('_renderUserGuide - component props should be correct', () => {
                const expectedResult = {
                    content: '',
                    title: 'UserGuide',
                    contentTitle: 'WinLossByTimeZone'
                }

                const formFilter = new FormFilter({ userGuideContent: '' });
                const userGuide = formFilter._renderUserGuide();

                const { content, title, contentTitle } = userGuide.props;
                const result = { content, title, contentTitle };

                expect(result).toEqual(expectedResult);
            })
        })

        describe('render whole component', () => {
            it('group by product - IsConverAll checkbox is disabled - match to snapshot', () => {
                const component = renderer.create(<FormFilter {...formFilterPropsForRender} />);

                const result = component.toJSON();

                expect(result).toMatchSnapshot()
            })

            it('group by currency - IsConverAll checkbox is enabled - match to snapshot', () => {
                const props = Object.assign({}, formFilterPropsForRender, { groupBy: 2 });
                const component = renderer.create(<FormFilter {...props} />);

                const result = component.toJSON();

                expect(result).toMatchSnapshot()
            })

            it('GroupBy filter - onChange event fired - changeGroupBy should be called with correct parameter', () => {
                function checkOnChangeEvent(indexRadioElement, expectedParam) {
                    const component = renderer.create(<FormFilter {...formFilterPropsForRender} />);

                    const instance = component.getInstance();
                    instance.changeGroupBy = jest.fn();

                    const tree = component.toJSON();
                    const row2Children = tree.children[1].children;
                    const radioElement = row2Children[0].children[0].children[indexRadioElement];

                    radioElement.props.onChange();

                    expect(instance.changeGroupBy).toHaveBeenCalledWith(expectedParam);
                }

                checkOnChangeEvent(3, 2);
                checkOnChangeEvent(1, 1);
            })

            it('onChange event of IncludeDrawData checkbox - props.changeIsIncludeDrawData should be called with correct parameter', () => {
                const component = renderer.create(<FormFilter {...formFilterPropsForRender} />);

                const instance = component.getInstance();

                const tree = component.toJSON();
                const row2Children = tree.children[1].children;
                const checkboxElement = row2Children[2].children[0].children[0].children[0];

                checkboxElement.props.onChange();

                expect(instance.props.changeIsIncludeDrawData).toHaveBeenCalledWith(false);
            })

            it('ExcludeTest checkbox - onChange event fired - props.changeIsExcludeTest should be called with correct parameter', () => {
                const component = renderer.create(<FormFilter {...formFilterPropsForRender} />);

                const instance = component.getInstance();

                const tree = component.toJSON();
                const row2Children = tree.children[1].children;
                const checkboxElement = row2Children[4].children[0].children[0].children[0];

                checkboxElement.props.onChange();

                expect(instance.props.changeIsExcludeTest).toHaveBeenCalledWith(false);
            })
        })
    })
})