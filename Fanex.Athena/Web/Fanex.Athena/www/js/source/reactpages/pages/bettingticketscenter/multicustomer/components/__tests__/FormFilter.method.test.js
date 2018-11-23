import React from 'react';
import { FormFilter } from '../FormFilter';
import AppData from '@jsroot/lib/AppData';
import LangHelper from '@jsroot/lib/langHelper';
import DataService from '@jsroot/lib/DataService.js';
import ApiUrl from '../../../common/constants/apiUrl';
import Site from '@jsroot/lib/site';
import Service from '../../../common/lib/Service';
import renderer from 'react-test-renderer';
import Select from 'react-select';

jest.mock('@jsroot/lib/AppData');

const DateRange = { fromDate: '06/01/2018', toDate: '07/27/2018'};

describe('FormFilter ', () => {
    beforeAll(() => {
        LangHelper.getSingleResource = jest.fn((resource) => resource);
        Object.assign(AppData, 
            { 
                originalBaseCurrencyList: ['USD', 'EUR'],
                currencyList: ['USD', 'EUR'],
                statusList: ["Win", "Lose", "Draw"],
                liveStatusList: ["Live", "NonLive"],
                productList: [{value: "SB"}, {value:"BA"}],
                baseCurrencyList: ['EUR'],
                dateConstraint: { minDate: '06/01/2012', maxDate: '07/27/2018', limitedMonthRange: 3 }
            });
    });

    describe('isValidModel ', () => {
        it('selectedProduct is empty - return false' ,() => {
            const props = { selectedProduct: '' };

            const filter = new FormFilter(props);

            expect(filter.isValidModel).toBe(false);
        });

        it('selectedProduct and username are not empty - return true' ,() => {
            const props = { selectedProduct: '123', username: 'test' };

            const filter = new FormFilter(props);

            expect(filter.isValidModel).toBe(true);
        });
    })

    describe('submit ', () => {
        it('parameter is null - do nothing' ,() => {
            const event = { preventDefault: jest.fn() };

            const props = { toggleLoader: jest.fn() };
            const filter = new FormFilter(props);
            filter._getParams = jest.fn(() => null);

            filter.submit(event);

            expect(props.toggleLoader).not.toHaveBeenCalled();
        });

        it('parameter is valid - update store' ,() => {
            const event = { preventDefault: jest.fn() };

            const title = 'title';
            Service.getTitle = jest.fn(() => title);

            const props = { 
                toggleLoader: jest.fn(),
                setReportTitle: jest.fn(),
                setRequestParam: jest.fn(),
                getGridData: jest.fn()
            };
            const filter = new FormFilter(props);
            
            var params = { refno: '123', BaseCurrency: 'USD' }
            filter._getParams = jest.fn(() => params);

            filter.submit(event);

            expect(props.setReportTitle).toHaveBeenCalledWith(title);
            expect(props.setRequestParam).toHaveBeenCalledWith(params);
            expect(props.toggleLoader).toHaveBeenCalledWith(true);
            expect(props.getGridData).toHaveBeenCalledWith(ApiUrl.MULTICUSTOMER_DATA_URL, params);
        });
    })

    describe('exportExcel ', () => {
        it('parameter is null - do nothing' ,() => {
            const event = { preventDefault: jest.fn() };

            const props = { toggleLoader: jest.fn() };
            const filter = new FormFilter(props);
            filter._getParams = jest.fn(() => null);

            filter.exportExcel(event);

            expect(props.toggleLoader).not.toHaveBeenCalled();
        });

        it('parameter is valid - call service to download excel file' ,() => {
            const event = { preventDefault: jest.fn() };

            const response = { data: 'excel' };
            const promise = Promise.resolve(response);
            DataService.getFile = jest.fn(() => promise);
            Site.downloadFile = jest.fn();

            const props = { toggleLoader: jest.fn() };
            const filter = new FormFilter(props);
            
            var params = { }
            filter._getParams = jest.fn(() => params);

            filter.exportExcel(event);

            expect(params.IsExport).toBe(true);
            expect(props.toggleLoader).toHaveBeenCalledWith(true);
            expect(DataService.getFile).toHaveBeenCalledWith(ApiUrl.MULTICUSTOMER_DATA_URL, params);
            Promise.resolve().then(() => {
                expect(Site.downloadFile).toHaveBeenCalledWith(response.data, 'BettingTicketsCenter_MultiCustomer.xls');
                expect(props.toggleLoader).toHaveBeenCalledWith(false);
            })
        });
    })

    describe('_getParams ', () => {
        it('model is invalid - return null' ,() => {
            const props = { dateRange: DateRange, requestParam: {}, selectedCurrency: {} };

            const filter = new FormFilter(props);

            expect(filter._getParams()).toBe(null);
        });

        it('model is valid - return correct result' ,() => {
            const props = {
                requestParam: {
                    PageSize: 1,
                    SortBy: 'col1',
                    SortOrder: 'asc'
                },
                dateRange: DateRange,
                selectedProduct: 'BA',
                sportList: "1234",
                selectedSport: 1,
                selectedBettype: 2,
                bettypeList: 3,
                selectedLeague: "5",
                leagueList: 6546,
                selectedMatch: 5,
                matchList: 987,
                selectedCurrency: "sd",
                baseCurrencyList: "sdfsdfsdfsdf",
                selectedBaseCurrency: 'USD',
                selectedStatus: 'status',
                selectedLiveStatus: 'Live'
            };

            const expected = {
                ProductCodeList: props.selectedProduct,
                FromDate: Site.convertDateToString(DateRange.fromDate),
                ToDate: Site.convertDateToString(DateRange.toDate),
                PageSize: props.requestParam.PageSize,
                PageIndex: 1,
                BaseCurrency: props.selectedBaseCurrency,
                StatusList: props.selectedStatus,
                LiveStatus: props.selectedLiveStatus,
                SortBy: props.requestParam.SortBy,
                SortOrder: props.requestParam.SortOrder
            }

            const filter = new FormFilter(props);

            expect(filter._getParams()).toEqual(expected);
        });
    })

    
    describe('_renderCurrencySelectList ', () => {
        it('onChange - update setSelectedCurrency' ,() => {
            const props = { 
                selectedCurrency: 'EUR', 
                setSelectedCurrency: jest.fn(), 
                setBaseCurrencyList: jest.fn(), 
                setSelectedBaseCurrency: jest.fn() };

            const filter = new FormFilter(props);
            const selectComponent = renderer.create(filter._renderCurrencySelectList());

            const newValue = { value: 'USD' }
            const selectedInstance = selectComponent.root.findByType(Select);
            selectedInstance.props.onChange(newValue);

            expect(props.setSelectedCurrency).toHaveBeenCalledWith(newValue);
        });
    })

    describe('_renderBaseCurrencySelectList ', () => {
        it('onChange - update setSelectedBaseCurrency' ,() => {
            const props = { baseCurrencyList: ['EUR', 'USD'], selectedBaseCurrency: 'EUR', setSelectedBaseCurrency: jest.fn() };

            const filter = new FormFilter(props);
            const selectComponent = renderer.create(filter._renderBaseCurrencySelectList());

            const newValue = { value: 'USD' }
            const selectedInstance = selectComponent.root.findByType(Select);
            selectedInstance.props.onChange(newValue);

            expect(props.setSelectedBaseCurrency).toHaveBeenCalledWith(newValue.value);
        });
    })

    describe('_renderDateRange', () => {
        it('component props should be correct', () => {
            const props = { dateRange: DateRange, setDateRange: jest.fn() };
            const expectedResult = {
                startDate: props.dateRange.fromDate,
                endDate: props.dateRange.toDate,
                minDate: AppData.dateConstraint.minDate,
                maxDate: AppData.dateConstraint.maxDate,
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
            const props = { dateRange: DateRange, setDateRange: jest.fn() };
            const formFilter = new FormFilter(props);
            const expecteMessage = 'DateRangeExceedMsg';

            const dateRange = formFilter._renderDateRange();
            dateRange.props.onChange({}, false);

            expect(window.alert).toHaveBeenCalledWith(expecteMessage);
        })

        it('onChange - is valid month range - change date range state', () => {
            const props = { dateRange: DateRange, setDateRange: jest.fn(), getBettypeList: jest.fn() };
            const formFilter = new FormFilter(props);
            const newDate = { fromDate: '02/02/2017', toDate: '12/12/2017' };

            const dateRange = formFilter._renderDateRange();
            dateRange.props.onChange(newDate);

            expect(props.setDateRange).toHaveBeenCalledWith(newDate);
        })
    })

    describe('Event handler ', () => {
        
        it('onChange in Product filter - setSelectedProduct should be called with correct parameter', () => {
            const props = { 
                dateRange: DateRange, 
                setSelectedProduct: jest.fn(), 
                getSportList: jest.fn(),
                getBettypeList: jest.fn()
            };
            const formFilter = new FormFilter(props);
            
            const productFilter = formFilter._renderProductSelectList();
            productFilter.props.onChange({value: "SB"});

            expect(props.setSelectedProduct).toHaveBeenCalledWith("SB");
        });

        it('onChange in Sport filter - setSelectedSport should be called with correct parameter', () => {
            const props = { dateRange: DateRange, sportList: [{value: 1}, {value:2}], setSelectedSport: jest.fn()};
            const formFilter = new FormFilter(props);
            
            const selectList = formFilter._renderSportSelectList();
            selectList.props.onChange({value: 2});

            expect(props.setSelectedSport).toHaveBeenCalledWith(2);
        });

        it('onChange in Bettype filter - setSelectedBettype should be called with correct parameter', () => {
            const props = { dateRange: DateRange, bettypeList: [{value: 1}, {value:2}], setSelectedBettype: jest.fn()};
            const formFilter = new FormFilter(props);
            
            const selectList = formFilter._renderBettypeSelectList();
            selectList.props.onChange({value: 2});

            expect(props.setSelectedBettype).toHaveBeenCalledWith({value: 2});
        });

        it('onChange in Status filter - setSelectedStatus should be called with correct parameter', () => {
            const props = {setSelectedStatus: jest.fn()};
            const formFilter = new FormFilter(props);
            
            const selectList = formFilter._renderStatusSelectList();
            selectList.props.onChange("oldItem", "newItem");

            expect(props.setSelectedStatus).toHaveBeenCalledWith("newItem");
        });

        it('onChange in LiveStatus filter - setSelectedLiveStatus should be called with correct parameter', () => {
            const props = {setSelectedLiveStatus: jest.fn()};
            const formFilter = new FormFilter(props);
            
            const selectList = formFilter._renderLiveStatusSelectList();
            selectList.props.onChange({value: "123"});

            expect(props.setSelectedLiveStatus).toHaveBeenCalledWith("123");
        });

        it('onClick submit button - call submit method' ,() => {
            const props = { refno: '1', dateRange: DateRange, selectedCurrency: {} };

            const filter = new FormFilter(props);
            filter.submit = jest.fn();
            const component = renderer.create(filter.render());

            const button = component.root.findByProps({ className: 'btn btn-submit' });
            button.props.onClick();

            expect(filter.submit).toHaveBeenCalled();
        });

        it('onClick excel button - call exportExcel method' ,() => {
            const props = { refno: '1', dateRange: DateRange, selectedCurrency: {} };

            const filter = new FormFilter(props);
            filter.exportExcel = jest.fn();
            const component = renderer.create(filter.render());

            const button = component.root.findByProps({ className: 'icon-excel right' });
            button.props.onClick();

            expect(filter.exportExcel).toHaveBeenCalled();
        });
    })
});