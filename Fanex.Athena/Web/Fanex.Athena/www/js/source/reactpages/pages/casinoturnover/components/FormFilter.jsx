import React, { Component } from 'react'
import { connect } from 'react-redux'
import { ActionCreators } from '../actions/index.js'
import { bindActionCreators } from 'redux'
import LangHelper from '@jsroot/lib/langHelper'
import AppData from '@jsroot/lib/AppData'
import Site from '@jsroot/lib/site'

import { DateRange } from '@jsroot/components/date-range/index'
import Select from 'react-select'
import MultipleSelect from '@9softstudio/react-multiselect'
import AutoComplete from '@jsroot/components/auto-complete'
import CheckBox from '@jsroot/components/CheckBox'

const RANGE_MONTH_CONSTRAINT = 3;
const CUSTOMER_SEARCH_URL = Site.resolveClientUrl("SearchCustomerAPI/SearchCustomer");
export class FormFilter extends Component {
    constructor(props) {
        super(props);
    }

    get isValidModel() {
        return true;
    }

    onDateRangeChange = (date, isValidMonthRange = true) => {
        if (isValidMonthRange) {
            this.props.setDateRange(date);
        }
        else {
            const message = LangHelper.getSingleResource('DateRangeExceedMsg').replace('{0}', RANGE_MONTH_CONSTRAINT);
            alert(message);
        }
    }

    onProductChange = (currentItem, selectedItemsKey) => {
        this.props.setSelectedProduct(selectedItemsKey)
    }

    onProductChange = (currentItem, selectedItemsKey) => {
        this.props.setSelectedStatus(selectedItemsKey)
    }

    _renderDateRange() {
        const dateRange = this.props.dateRange;
        const dateConstraint = AppData.dateConstraint;

        return (
            <DateRange 
                startDate={dateRange.fromDate} 
                endDate={dateRange.toDate}
                minDate={dateConstraint.minDate} 
                maxDate={dateConstraint.maxDate}
                today={dateRange.toDate}
                limitedMonthRange={RANGE_MONTH_CONSTRAINT}
                onChange={this.onDateRangeChange} />
        )
    }

    _renderCurrency() {
        const selectedCurrency = this.props.selectedCurrency;
        const currencyList = AppData.currencyList;

        return <Select instanceId='ddlCurrency'
                    name='Currency'
                    value={selectedCurrency.value}
                    options={currencyList}
                    onChange={(item) => this.props.setSelectedCurrency(item)} />
    }

    _renderBaseCurrency() {
        const selectedBaseCurrency = this.props.selectedBaseCurrency;
        const baseCurrencyList = AppData.baseCurrencyList;

        return baseCurrencyList && baseCurrencyList.length && (
            <Select instanceId='ddlBased'
                    name='BaseCurrency'
                    value={selectedBaseCurrency}
                    options={baseCurrencyList}
                    onChange={(item) => this.props.setSelectedBaseCurrency(item.value)} />
        );
    }

    _renderConvertAll() {
        return (
            <CheckBox id="IsConvertAll"
                checked={this.props.isConvertAll}
                onChange={(checked) => this.props.changeIsConvertAll(checked)}
                label={LangHelper.getSingleResource("ConvertAllFiguresToBaseCurrency")} />
            );
    }

    _renderProduct() {
        const productOptions = AppData.productOptions;

        return productOptions && productOptions.length && (
            <MultipleSelect id='ProductList'
                dataSource={productOptions}
                keyField={'Code'}
                valueField={'Name'}
                statusField={'Status'}
                optionAllLabel={LangHelper.getSingleResource('All')}
                noneSelectedLabel={LangHelper.getSingleResource('SelectOptions')}
                onChange={this.onProductChange} />
        );
    }

    _renderUserName() {
        return (
        <AutoComplete url={CUSTOMER_SEARCH_URL} 
            onSelect={(value) => this.props.setUsername(value)}
            onInput={(value) => this.props.setUsername(value)}
            htmlAttributes={{placeholder:LangHelper.getSingleResource('ExternalIdOrSystemId')}} />
        );
    }

    _renderStatus() {
        const statusList = AppData.statusList;

        return statusList && statusList.length && (
            <MultipleSelect id='StatusList'
                dataSource={statusList}
                keyField={'Code'}
                valueField={'Name'}
                statusField={'Status'}
                optionAllLabel={LangHelper.getSingleResource('All')}
                noneSelectedLabel={LangHelper.getSingleResource('SelectOptions')}
                onChange={this.setSelectedStatus} />
        );
    }

    render() {
        return (
            <div className='page-filter'>
                <div className='row'>
                    <div className='col-12 col-md-6 col-sm-12 col-lg-3 col-xl-3'>
                        <div className='form-group'>
                            <label className='label-text'>{LangHelper.getSingleResource('Date')}</label>
                            {this._renderDateRange()}
                        </div>
                    </div>
                    <div className='col-12 col-md-4 col-sm-12 col-lg-3 col-xl-2'>
                        <div className='form-group'>
                            <label className='label-text'>{LangHelper.getSingleResource('Currency')}</label>
                            {this._renderCurrency()}
                        </div>
                    </div>
                    <div className='col-12 col-md-3 col-sm-12 col-lg-3 col-xl-2'>
                        <div className='form-group'>
                            <label className='label-text'>{LangHelper.getSingleResource('BaseCurrency')}</label>
                            {this._renderBaseCurrency()}
                        </div>
                    </div>
                    <div className="col-12 col-md-3 col-sm-3 col-lg-3 col-xl-3">
                        <div className="form-group">
                            {this._renderConvertAll()}
                        </div>    
                    </div>
                    <div className='col-12 col-md-4 col-sm-12 col-lg-3 col-xl-3'>
                        <div className='form-group'>
                            <label className='label-text'>{LangHelper.getSingleResource('Product')}</label>
                            {this._renderProduct()}
                        </div>
                    </div>
                    <div className='col-12 col-md-6 col-sm-12 col-lg-3 col-xl-2'>
                        <div className='form-group'>
                            <label className='label-text'>{LangHelper.getSingleResource('UserName')}</label>
                            {this._renderUserName()}
                        </div>
                    </div>
                    <div className='col-12 col-md-4 col-sm-12 col-lg-3 col-xl-2'>
                        <div className='form-group'>
                            <label className='label-text'>{LangHelper.getSingleResource('Status')}</label>
                            {this._renderStatus()}
                        </div>
                    </div>
                    <div className='col-12 col-md-3 col-sm-12 col-lg-3 col-xl-3'>
                        <div className='form-group'>
                            <div className='button-group'>
                                <button disabled={!this.isValidModel} type='button' className='btn btn-submit' onClick={this.submit}>{LangHelper.getSingleResource('Submit')}</button>
                            </div>
                        </div>
                    </div>
                </div>

                <div className='row'>
                    <div className='caret-block'>
                        <i className='icon-excel right' onClick={this.exportExcel} title={LangHelper.getSingleResource('ExportToExcel')}></i>
                    </div>
                </div>
            </div>
        );
    }
}
function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
}
function mapStateToProps(state) {
    const { reportTitle, isLoading, ...rest } = state;
    return Object.assign({}, rest);
};

export default connect(mapStateToProps, mapDispatchToProps)(FormFilter);