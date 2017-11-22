import React from 'react';
import renderer from 'react-test-renderer';
import { FormFilter, connect } from '../formFilter';
import LangHelper from '../../../../../lib/langHelper';
import Site from '../../../../../lib/site';

const dateRangeComponentPath = '../../../../../components/date-range/index';
const notificationTypeComponentPath = 'react-select';

describe('FormFilter', () => {
    beforeAll(() => {
        LangHelper.getSingleResource = jest.fn((resource) => {
            if(resource === 'DateRangeExceedMsg')
            {
                return 'DateRangeExceedMsg {0}';
            }

            return resource;
        });

        Site.convertDateToString = jest.fn((date) => date);
    })
    afterAll(() => {
        jest.resetAllMocks();
    })

    const formFilterProps = {
        dateRange: {
            fromDate: '01/01/2017',
            toDate: '01/10/2017'
        },
        dateConstraint: {
            minDate: '01/01/2015',
            maxDate: '12/31/2020'
        },
        notificationTypes: [
            { value: '0', label: 'A' }
        ],
        selectedType: 0
    }; 

    it('match to snapshot', () => {
        const component = renderer.create(<FormFilter {...formFilterProps}/>);

        const result = component.toJSON();

        expect(result).toMatchSnapshot()
    })

    describe('render components', () => {
        describe('Date Range filter', () => {
            let dateRange = {};

            beforeAll(() => {
                jest.mock(dateRangeComponentPath, (props) => (<div {...props}>Date Range</div>));

                const formFilter = new FormFilter(formFilterProps);
                dateRange = formFilter._renderDateRange();
            })

            afterAll(() => {
                jest.unmock(dateRangeComponentPath);
            })

            it('_renderDateRange - component props should be correct', () => {
                const expectedResult = {
                    startDate: formFilterProps.dateRange.fromDate,
                    endDate: formFilterProps.dateRange.toDate,
                    minDate: formFilterProps.dateConstraint.minDate,
                    maxDate: formFilterProps.dateConstraint.maxDate,
                    limitedMonthRange: 6
                }
                const { startDate, endDate, minDate, maxDate, limitedMonthRange } = dateRange.props;
                const result = { startDate, endDate, minDate, maxDate, limitedMonthRange };

                expect(result).toEqual(expectedResult);
            })

            it('onChange - invalid month range - alert warning message', () => {
                window.alert = jest.fn();
                var expectedMsg = 'DateRangeExceedMsg 6';
                
                dateRange.props.onChange({}, false);

                expect(window.alert).toHaveBeenCalledWith(expectedMsg);
            })

            it('onChange - valid month range - update state for Date range', () => {
                const newDateRange = {
                    fromDate: '02/01/2017',
                    toDate: '02/10/2017'
                };
                
                formFilterProps.setDateRange = jest.fn();
                
                dateRange.props.onChange(newDateRange, true);

                expect(formFilterProps.setDateRange).toHaveBeenCalledWith(newDateRange);
            })
        })

        describe('Notification Type selection', () => {
            let notificationType = {};

            beforeAll(() => {
                jest.mock(notificationTypeComponentPath, (props) => (<div {...props}>Notification Type</div>));

                const formFilter = new FormFilter(formFilterProps);
                notificationType = formFilter._renderNotificationTypeSelectList();
            })

            afterAll(() => {
                jest.unmock(notificationTypeComponentPath);
            })

            it('_renderNotificationTypeSelectList - component props should be correct', () => {
                const expectedResult = {
                    value: formFilterProps.selectedType,
                    options: formFilterProps.notificationTypes
                }
                const { value, options } = notificationType.props;
                const result = { value, options };

                expect(result).toEqual(expectedResult);
            })

            it('onChange - update Selected Type with correct value', () => {
                const newValue = 1;
                
                formFilterProps.setSelectedType = jest.fn();
                
                notificationType.props.onChange({ value: newValue });

                expect(formFilterProps.setSelectedType).toHaveBeenCalledWith(newValue);
            })
        })
    })

    describe('submit', () => {
        let formFilter = {};

        beforeAll(() => {
            formFilter = new FormFilter(formFilterProps);

            formFilterProps.setParam = jest.fn();
            formFilterProps.toggleLoader = jest.fn();
            formFilterProps.getGridData = jest.fn();
        })

        it('param is null - do not call prop functions', () => {
            formFilter._getParams = jest.fn(() => {
                return null;
            });

            const event = {
                preventDefault: jest.fn()
            };
            formFilter.submit(event);

            expect(formFilterProps.setParam).not.toHaveBeenCalled();
            expect(formFilterProps.toggleLoader).not.toHaveBeenCalled();
            expect(formFilterProps.getGridData).not.toHaveBeenCalled();
        })

        it('param is null - call prop functions', () => {
            const param = {
                xsrfToken: 'token',
                data: {
                    FromDate: formFilterProps.dateRange.fromDate,
                    ToDate: formFilterProps.dateRange.toDate,
                    NotificationTypes: formFilterProps.selectedType,
                    PageSize: 10,
                    PageIndex: 1
                }
            };
            formFilter._getParams = jest.fn(() => param);

            const event = {
                preventDefault: jest.fn()
            };
            formFilter.submit(event);

            expect(formFilterProps.setParam).toHaveBeenCalledWith(param.data);
            expect(formFilterProps.toggleLoader).toHaveBeenCalledWith(true);
            expect(formFilterProps.getGridData).toHaveBeenCalledWith(param);
        })

        describe('_getParams', () => {
            it('xsrfToken is null - return null', () => {
                const props = { ...formFilterProps, xsrfToken: null };
                const filter = new FormFilter(props);

                const result = filter._getParams();

                expect(result).toBe(null);
            })

            it('PageSize is not existing - return data with default PageSize', () => {
                const props = { ...formFilterProps, xsrfToken: 'token' };
                const filter = new FormFilter(props);

                var expectedResult = {
                    xsrfToken: 'token',
                    data: {
                        FromDate: formFilterProps.dateRange.fromDate,
                        ToDate: formFilterProps.dateRange.toDate,
                        NotificationTypes: formFilterProps.selectedType,
                        PageSize: 10,
                        PageIndex: 1
                    }
                };

                const result = filter._getParams();

                expect(result).toEqual(expectedResult);
            })

            it('PageSize is existing - return data with correct PageSize', () => {
                const props = { ...formFilterProps, xsrfToken: 'token', param: { PageSize: 15 } };
                const filter = new FormFilter(props);              

                var expectedResult = {
                    xsrfToken: 'token',
                    data: {
                        FromDate: formFilterProps.dateRange.fromDate,
                        ToDate: formFilterProps.dateRange.toDate,
                        NotificationTypes: formFilterProps.selectedType,
                        PageSize: 15,
                        PageIndex: 1
                    }
                };

                const result = filter._getParams();

                expect(result).toEqual(expectedResult);
            })
        })
    })
})