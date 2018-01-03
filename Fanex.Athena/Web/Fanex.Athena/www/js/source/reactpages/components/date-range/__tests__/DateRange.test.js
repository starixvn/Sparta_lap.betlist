import React from 'react'
import renderer from 'react-test-renderer'

import parseInput from '../utils/parseInput'
import { MomentFormat } from '../constants'
import getTheme from '../styles'
import defaultOptions from '../defaultOptions'
import moment from 'moment'

import DateRange from '../DateRange'

jest.mock('../defaultOptions')
jest.mock('../styles')

describe('DateRange ', () => {
    const defaultProps = {
        linkedCalendars: false,
        theme: {},
        format: MomentFormat.default,
        calendars: 2,
        onlyClasses: true,
        offsetPositive: false,
        classNames: {},
        specialDays: [],
        rangedCalendars: false,
        twoStepChange: false,
        showRange: true,

        startDate: "1/1/2018",
        endDate: "1/3/2018"
    }

    const defaultStartDateMoment = moment(defaultProps.startDate, MomentFormat.default)['startOf']('day');
    const defaultEndDateMoment = moment(defaultProps.endDate, MomentFormat.default)['endOf']('day');

    const defaultRange = {
        startDate: defaultStartDateMoment,
        endDate: defaultEndDateMoment
    }

    const originalLocale = defaultOptions.locale;

    beforeAll(() => {
        getTheme.mockImplementation(() => { return {}; });
        defaultOptions.locale = {};
        defaultOptions.ranges = jest.fn(() => { });
    })

    afterAll(() => {
        defaultOptions.locale = originalLocale;
    })

    describe('constructor', () => {
        it('initial values should be correct', () => {
            const dateRange = new DateRange(defaultProps);

            expect(dateRange.styles).toEqual({});
            expect(dateRange.step).toEqual(0);
            expect(dateRange.locale).toEqual({});
            expect(dateRange.ranges).toEqual({});
            expect(dateRange.isValid).toEqual(true);
            expect(dateRange.currentRange).toEqual(defaultRange);
        })

        it('State should be correct', () => {
            function assertState(linkedCalendars) {
                const props = Object.assign({}, defaultProps, { linkedCalendars });
                const dateRange = new DateRange(props);

                const expectedState = {
                    range: defaultRange,
                    link: linkedCalendars ? defaultEndDateMoment : linkedCalendars,
                    show: false
                }

                expect(dateRange.state).toEqual(expectedState);
            }

            assertState(false);
            assertState(true);
        })

        it('date is function type - range should be correct', () => {
            const expectedStartDate = moment([2018, 1, 1]);
            const startDate = () => expectedStartDate.clone();
            const props = Object.assign({}, defaultProps, { startDate });
            const dateRange = new DateRange(props);

            expect(dateRange.currentRange.startDate).toEqual(expectedStartDate);
        })

        it('date is object but not moment - return null', () => {
            const props = Object.assign({}, defaultProps, { startDate: {} });
            const dateRange = new DateRange(props);

            expect(dateRange.currentRange.startDate).toEqual(null);
        })
    })

    describe('dateRangeText', () => {
        it('currentRange has value - dateRangeText should be correct format', () => {
            const expectedResult = `${defaultStartDateMoment.format(MomentFormat.default)} - ${defaultEndDateMoment.format(MomentFormat.default)}`;

            const dateRange = new DateRange(defaultProps);

            expect(dateRange.dateRangeText).toBe(expectedResult);
        })

        it('currentRange is empty object - return empty string', () => {
            const dateRange = new DateRange(defaultProps);
            dateRange.currentRange = {};

            expect(dateRange.dateRangeText).toBe("");
        })
    })

    describe('show/ hide', () => {
        let dateRange;
        let stateToUpdate;
        beforeEach(() => {
            dateRange = new DateRange(defaultProps);
            dateRange.setState = jest.fn((newState) => { stateToUpdate = newState });
        })

        it('show - step should be 0', () => {
            dateRange.show();

            expect(dateRange.step).toBe(0);
        })

        it('show - setState should be called with correct parameter', () => {
            const expectedState = { show: true };

            dateRange.show();

            expect(stateToUpdate).toEqual(expectedState);
        })

        it('hide - setState should be called with correct parameter', () => {
            const expectedState = { show: false };

            dateRange.hide();

            expect(stateToUpdate).toEqual(expectedState);
        })
    })

    describe('componentDidMount', () => {
        it('onInit is undefined - do not call that function', () => {
            const mockOnInit = jest.fn();
            const props = Object.assign({}, defaultProps);
            const dateRange = new DateRange(props);

            dateRange.componentDidMount();

            expect(mockOnInit).not.toHaveBeenCalled();
        })

        it('onInit is defined - call that function with correct parameter', () => {
            const mockOnInit = jest.fn();
            const props = Object.assign({}, defaultProps, { onInit: mockOnInit });
            const dateRange = new DateRange(props);

            dateRange.componentDidMount();

            expect(mockOnInit).toHaveBeenCalledWith(defaultRange);
        })
    })

    describe('orderRange', () => {
        it('startDate before endDate - do not swap these values', () => {
            const dateRange = new DateRange(defaultProps);

            const result = dateRange.orderRange(defaultRange);

            expect(result).toEqual(defaultRange);
        })

        it('startDate after endDate - swap these values', () => {
            const dateRange = new DateRange(defaultProps);
            const range = {
                startDate: defaultEndDateMoment,
                endDate: defaultStartDateMoment
            }

            const result = dateRange.orderRange(range);

            expect(result).toEqual(defaultRange);
        })
    })

    describe('setRange', () => {
        it('valid month range - setState show be called with correct parameter', () => {
            function assertSetRangeWithValidMonthRange(show) {
                const dateRange = new DateRange(defaultProps);
                dateRange.isValidMonthRange = jest.fn(() => true);
                let stateToUpdate;
                dateRange.setState = jest.fn((newState) => { stateToUpdate = newState });

                const range = Object.assign({}, defaultRange);
                const expectedState = show === undefined ? { range } : { range, show };
                const triggerChange = false;

                dateRange.setRange(range, triggerChange, show);

                expect(stateToUpdate).toEqual(expectedState);
            }

            assertSetRangeWithValidMonthRange();
            assertSetRangeWithValidMonthRange(true);
            assertSetRangeWithValidMonthRange(false);
        })

        it('valid month range - trigger change is true - callback should be called with correct parameter', () => {
            const onChange = jest.fn();

            const props = Object.assign({}, defaultProps, { onChange });
            const dateRange = new DateRange(props);
            dateRange.isValidMonthRange = jest.fn(() => true);
            let callback;
            dateRange.setState = jest.fn((newState, callbackFunc) => { callback = callbackFunc });

            const range = Object.assign({}, defaultRange);
            const triggerChange = true, show = true, isValid = true;

            dateRange.setRange(range, triggerChange, show);

            callback();

            expect(onChange).toHaveBeenCalledWith(range, isValid);
        })

        it('valid month range - trigger change is false - callback should not be called', () => {
            const onChange = jest.fn();

            const props = Object.assign({}, defaultProps, { onChange });
            const dateRange = new DateRange(props);
            dateRange.isValidMonthRange = jest.fn(() => true);
            let callback;
            dateRange.setState = jest.fn((newState, callbackFunc) => { callback = callbackFunc });

            const range = Object.assign({}, defaultRange);
            const triggerChange = false, show = true;

            dateRange.setRange(range, triggerChange, show);

            callback();

            expect(onChange).not.toHaveBeenCalled();
        })

        it('invalid month range - setState should not be called', () => {
            const onChange = jest.fn();

            const props = Object.assign({}, defaultProps, { onChange });
            const dateRange = new DateRange(props);
            dateRange.isValidMonthRange = jest.fn(() => false);
            dateRange.setState = jest.fn();

            const range = Object.assign({}, defaultRange);
            const triggerChange = false, show = true;

            dateRange.setRange(range, triggerChange, show);

            expect(dateRange.setState).not.toHaveBeenCalled();
        })

        it('invalid month range - onChange should be called with correct parameter', () => {
            const onChange = jest.fn();

            const props = Object.assign({}, defaultProps, { onChange });
            const dateRange = new DateRange(props);
            dateRange.isValidMonthRange = jest.fn(() => false);
            dateRange.setState = jest.fn();

            const range = Object.assign({}, defaultRange);
            const triggerChange = false, show = true, isValid = false;

            dateRange.setRange(range, triggerChange, show);

            expect(onChange).toHaveBeenCalledWith(range, isValid);
        })
    })

    describe('handleSelect', () => {
        it('select ranges which are defined - step should be 0 and setRange should be called with correct parameter', () => {
            const dateRange = new DateRange(defaultProps);
            dateRange.setRange = jest.fn();
            const showDaterange = false, triggerChange = true;

            dateRange.handleSelect(defaultRange);

            expect(dateRange.step).toBe(0);
            expect(dateRange.setRange).toHaveBeenCalledWith(defaultRange, triggerChange, showDaterange);
        })

        it('select start date - setRange should be called with correct parameter', () => {
            const dateRange = new DateRange(defaultProps);
            dateRange.setRange = jest.fn();
            const triggerChange = true;
            const expectedRange = {
                startDate: defaultStartDateMoment,
                endDate: defaultStartDateMoment
            }

            dateRange.handleSelect(defaultStartDateMoment);

            expect(dateRange.step).toBe(1);
            expect(dateRange.setRange).toHaveBeenCalledWith(expectedRange, triggerChange);
        })

        it('select end date - setRange should be called with correct parameter', () => {
            const dateRange = new DateRange(defaultProps);
            dateRange.step = 1;
            dateRange.setRange = jest.fn();
            const triggerChange = true;

            dateRange.handleSelect(defaultEndDateMoment);

            expect(dateRange.step).toBe(0);
            expect(dateRange.setRange).toHaveBeenCalledWith(defaultRange, triggerChange);
        })

        it('enable twoStepChange - select start date - triggerChange should be false', () => {
            const props = Object.assign({}, defaultProps, { twoStepChange: true });
            const dateRange = new DateRange(props);
            let trigerChangeResult;
            dateRange.setRange = jest.fn((range, triggerChange) => trigerChangeResult = triggerChange);

            dateRange.handleSelect(defaultEndDateMoment);

            expect(trigerChangeResult).toBe(false);
        })

        it('enable twoStepChange - select end date - triggerChange should be true', () => {
            const props = Object.assign({}, defaultProps, { twoStepChange: true });
            const dateRange = new DateRange(props);
            dateRange.step = 1;
            let trigerChangeResult;
            dateRange.setRange = jest.fn((range, triggerChange) => trigerChangeResult = triggerChange);

            dateRange.handleSelect(defaultEndDateMoment);

            expect(trigerChangeResult).toBe(true);
        })
    })

    describe('isValidMonthRange', () => {
        it('limitedMonthRange props is undefined - return true', () => {
            const dateRange = new DateRange(defaultProps);

            const result = dateRange.isValidMonthRange();

            expect(result).toBe(true);
        })

        it('limitedMonthRange props is defined - pass valid range - return true', () => {
            const props = Object.assign({}, defaultProps, { limitedMonthRange: 3 });
            const dateRange = new DateRange(props);

            const result = dateRange.isValidMonthRange(defaultRange);

            expect(result).toBe(true);
        })

        it('limitedMonthRange props is defined - pass invalid range - return false', () => {
            const props = Object.assign({}, defaultProps, { limitedMonthRange: 1 });
            const dateRange = new DateRange(props);

            const endDate = moment([2018, 3, 1]);
            const range = {
                startDate: defaultStartDateMoment,
                endDate
            }
            const result = dateRange.isValidMonthRange(range);

            expect(result).toBe(false);
        })
    })

    describe('handleLinkChange', () => {
        it('setState should be called with correct parameter', () => {
            const dateRange = new DateRange(defaultProps);
            dateRange.setState = jest.fn();
            const newDate = defaultStartDateMoment;
            const expectedParam = {
                link: newDate
            }

            dateRange.handleLinkChange(newDate);

            expect(dateRange.setState).toHaveBeenCalledWith(expectedParam);
        })
    })

    describe('handleApply', () => {
        it('onClickApplyButton should be called with correct parameter', () => {
            const onClickApplyButton = jest.fn();
            const props = Object.assign({}, defaultProps, { onClickApplyButton });
            const dateRange = new DateRange(props);
            dateRange.hide = jest.fn();

            dateRange.handleApply();

            expect(onClickApplyButton).toHaveBeenCalledWith(defaultRange, true);
        })

        it('currentRange should be correct', () => {
            const selectedRange = {
                startDate: defaultStartDateMoment.clone().month(2),
                endDate: defaultEndDateMoment.clone().month(4)
            }
            const dateRange = new DateRange(defaultProps);
            dateRange.state.range = Object.assign({}, selectedRange);
            dateRange.hide = jest.fn();

            dateRange.handleApply();

            expect(dateRange.currentRange).toEqual(selectedRange);
        })

        it('hide should be called', () => {
            const dateRange = new DateRange(defaultProps);
            dateRange.hide = jest.fn();

            dateRange.handleApply();

            expect(dateRange.hide).toHaveBeenCalled();
        })
    })

    describe('componentWillReceiveProps', () => {
        it('selected start date and end date is same with old start date - do nothing', () => {
            var newProps = {
                startDate: defaultStartDateMoment.clone(),
                endDate: defaultEndDateMoment.clone()
            }

            const dateRange = new DateRange(defaultProps);
            dateRange.setRange = jest.fn();

            dateRange.componentWillReceiveProps(newProps);

            expect(dateRange.setRange).not.toHaveBeenCalled();
        })

        it('selected start date and end date is not same with old start date - setRange should be called with correct parameter', () => {
            var newProps = {
                startDate: defaultStartDateMoment.clone().month(1),
                endDate: defaultEndDateMoment.clone().month(1)
            }

            const dateRange = new DateRange(defaultProps);
            dateRange.setRange = jest.fn();

            dateRange.componentWillReceiveProps(newProps);

            expect(dateRange.setRange).toHaveBeenCalledWith(newProps);
        })

        it('Do not change range - do nothing', () => {
            var newProps = {}

            const dateRange = new DateRange(defaultProps);
            dateRange.setRange = jest.fn();

            dateRange.componentWillReceiveProps(newProps);

            expect(dateRange.setRange).not.toHaveBeenCalled();
        })
    })
})