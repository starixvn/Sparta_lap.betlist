import React, { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import Select from 'react-select'
import MultipleSelect from '../../../../components/multiple-select'
import { DateRange } from '../../../../components/date-range/index'
import AutoComplete from '../../../../components/auto-complete'

import { ActionCreators } from '../actions/index.js'
import LangHelper from '../../../../lib/langHelper'
import Site from '../../../../lib/site'

const CUSTOMER_SEARCHING_URL = Site.resolveClientUrl("CustomerAPI/SearchCustomer");

export class FormFilter extends Component {
    constructor(props) {
        super(props);
    }

    submit = (event) => {
        event.preventDefault();
        const params = this._getParams();
        if (!params) {
            return;
        }

        this.props.toggleLoader(true);
        this.props.getGridData(params);
    }

    _getParams() {
        const { fromDate, toDate } = this.props.dateRange;

        return {
            FromDate: Site.convertDateToString(fromDate),
            ToDate: Site.convertDateToString(toDate),
            ProductCodeList: this.props.selectedProduct,
            CustId: 0,
            CurrencyId: this.props.selectedCurrency.value,
            CurrencyName: this.props.selectedCurrency.label,
            BaseCurrency: this.props.selectedBaseCurrency
        }
    }

    productChangeHandler = (currentItem, selectedItemsKey) => {
        this.props.setSelectedProduct(selectedItemsKey)
    }

    applyDateRangeHandler = (date, isValidMonthRange = true) => {
        if (isValidMonthRange) {
            this.props.setDateRange(date);
        }
        else {
            const message = LangHelper.getSingleResource("DateRangeExceedMsg").replace("{0}", RANGE_MONTH_CONSTRAINT);
            alert(message);
        }
    }

    exportExcel = (event) => {
        event.preventDefault();
        
        const params = this._getParams();
        if (!params) {
            return;
        }

        params.IsExport = true;
        this.props.toggleLoader(true);
        this.props.exportExcel(params).then((response) => {
            const url = window.URL.createObjectURL(new Blob([response.data]));
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', 'BettingTicketsCenter_Running.xls');
            document.body.appendChild(link);
            link.click();
            link.parentNode.removeChild(link);
            this.props.toggleLoader(false);
        });
    }

    _renderDateRange() {
        const { dateRange, dateConstraint } = this.props;

        return (
            <DateRange startDate={dateRange.fromDate} endDate={dateRange.toDate}
                minDate={dateConstraint.minDate} maxDate={dateConstraint.maxDate}
                today={dateRange.toDate} limitedMonthRange={dateConstraint.limitedMonthRange}
                onChange={this.applyDateRangeHandler} />
        )
    }

    _renderProductSelectList() {
        const { productOptions } = this.props;
        return productOptions && productOptions.length ? (
            <MultipleSelect id="ProductList"
                dataSource={this.props.productOptions}
                keyField={"Code"}
                valueField={"Name"}
                statusField={"Status"}
                optionAllLabel={LangHelper.getSingleResource("All")}
                noneSelectedLabel={LangHelper.getSingleResource("SelectOptions")}
                onChange={this.productChangeHandler} />
        ) : null;
    }

     _renderCurrencySelectList() {
        const { currencyList, selectedCurrency } = this.props;
        
        return <Select instanceId="ddlCurrency"
                value={selectedCurrency.value}
                options={currencyList}
                onChange={(item) => this.props.setSelectedCurrency(item)} />
    }

    _renderBaseCurrencySelectList() {
        const { baseCurrencyList, selectedBaseCurrency } = this.props;

        return baseCurrencyList && baseCurrencyList.length && (
            <Select instanceId="ddlBased"
                value={selectedBaseCurrency}
                options={baseCurrencyList}
                onChange={(item) => this.props.setSelectedBaseCurrency(item.value)} />
        );
    }

    render() {
        return (
            <div className="page-filter">
                <div className="row">

                     <div className="col-12 col-md-6 col-sm-6 col-lg-4 col-xl-3">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Date")}</label>
                            {this._renderDateRange()}
                        </div>
                    </div>

                    <div className="col-12 col-md-3 col-sm-3 col-lg-4 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Product")}</label>
                            {this._renderProductSelectList()}
                        </div>
                    </div>

                    <div className="col-12 col-md-3 col-sm-3 col-lg-3 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Username")}</label>
                            <input className="input-text" id="Username" name="Username" type="text" value={this.props.username} placeholder={LangHelper.getSingleResource("Username")} 
                            onChange={(event) => this.props.setUsername(event.target.value)} />
                        </div>
                    </div>

                    <div className="col-12 col-md-3 col-sm-3 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Currency")}</label>
                            {this._renderCurrencySelectList()}
                        </div>
                    </div>

                    <div className="col-12 col-md-3 col-sm-3 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("BaseCurrency")}</label>
                            {this._renderBaseCurrencySelectList()}
                        </div>
                    </div>

                    <div className="col-12 col-md-2 col-sm-2 col-lg-2 col-xl-1">
                        <div className="form-group">
                            <div className="button-group">
                            <button type="button" className="btn btn-submit" onClick={this.submit}>{LangHelper.getSingleResource("Submit")}</button>
                            </div>
                        </div>
                    </div>
                </div>
                 <div className="row">
                    <div className="caret-block">
                        <i className="icon-excel right" onClick={this.exportExcel} title={LangHelper.getSingleResource("ExportToExcel")}></i>
                    </div>
               </div>
            </div>
        )
    }
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
}
function mapStateToProps(state) {
    const { gridData, ...rest } = state;
    return Object.assign({}, rest);
};

export default connect(mapStateToProps, mapDispatchToProps)(FormFilter);