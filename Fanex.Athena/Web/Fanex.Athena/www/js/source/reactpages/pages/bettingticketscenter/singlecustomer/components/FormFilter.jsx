import React, { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import Select from 'react-select'
import MultipleSelect from '@jsroot/components/multiple-select'
import { DateRange } from '@jsroot/components/date-range/index'
import AutoComplete from '@jsroot/components/auto-complete'
import CheckBox from '@jsroot/components/CheckBox'

import { ActionCreators } from '../actions/index.js'
import LangHelper from '@jsroot/lib/langHelper'
import Site from '@jsroot/lib/site'
import AppData from '@jsroot/lib/AppData'
import DataService from '@jsroot/lib/DataService.js'

const CUSTOMER_SEARCHING_URL = Site.resolveClientUrl("SearchCustomerAPI/SearchCustomer");

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
            Username: this.props.username,
            CurrencyId: this.props.selectedCurrency.value,
            CurrencyName: this.props.selectedCurrency.label,
            BaseCurrency: this.props.selectedBaseCurrency
        }
    }

    applyDateRangeHandler = (date, isValidMonthRange = true) => {
        if (isValidMonthRange) {
            this.props.setDateRange(date);
        }
        else {
            const message = LangHelper.getSingleResource("DateRangeExceedMsg").replace("{0}", AppData.dateConstraint.limitedMonthRange);
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
        DataService.getFile(Site.resolveClientUrl('api/BettingTicketCenter/GetByRunning'), params).then((response) => {
            Site.downloadFile(response.data, 'BettingTicketsCenter_Running.xls');
            this.props.toggleLoader(false);
        });
    }

    _renderDateRange() {
        const dateRange = this.props.dateRange;
        const dateConstraint = AppData.dateConstraint;

        return (
            <DateRange startDate={dateRange.fromDate} endDate={dateRange.toDate}
                minDate={dateConstraint.minDate} maxDate={dateConstraint.maxDate}
                today={dateRange.toDate} limitedMonthRange={dateConstraint.limitedMonthRange}
                onChange={this.applyDateRangeHandler} />
        )
    }

    _renderProductSelectList() {
        const productOptions = AppData.productOptions;

        return productOptions && productOptions.length && (
            <MultipleSelect id="ProductList"
                dataSource={productOptions}
                keyField={"Code"}
                valueField={"Name"}
                statusField={"Status"}
                optionAllLabel={LangHelper.getSingleResource("All")}
                noneSelectedLabel={LangHelper.getSingleResource("SelectOptions")}
                onChange={(currentItem, selectedItemsKey) => this.props.setSelectedProduct(selectedItemsKey)} />
        );
    }

    _renderBaseCurrencySelectList() {
        const baseCurrencyList = AppData.baseCurrencyList;

        return baseCurrencyList && baseCurrencyList.length && (
            <Select instanceId="ddlBased"
                value={this.props.selectedBaseCurrency}
                options={baseCurrencyList}
                onChange={(item) => this.props.setSelectedBaseCurrency(item.value)} />
        );
    }

    _renderStatusSelectList() {
        const statusList = AppData.statusList;

        return statusList && statusList.length && (
                <MultipleSelect id="StatusList"
                    dataSource={statusList}
                keyField={"Id"}
                valueField={"Value"}
                statusField={"Status"}
                optionAllLabel={LangHelper.getSingleResource("All")}
                noneSelectedLabel={LangHelper.getSingleResource("SelectOptions")}
                onChange={(currentItem, selectedItemsKey) => this.props.setSelectedStatus(selectedItemsKey)} />);
    }

    _renderPlatformSelectList() {
        const platformList = AppData.platformList;

        return platformList && platformList.length && (
            <Select instanceId="PlatformList"
                value={this.props.selectedPlatform}
                options={platformList}
                onChange={(item) => this.props.setSelectedPlatform(item.value)} />
        );
    }

      _renderLiveStatusSelectList() {
          const liveStatusList = AppData.liveStatusList;

          return liveStatusList && liveStatusList.length && (
            <Select instanceId="LiveStatusList"
                value={this.props.selectedLiveStatus}
      options={liveStatusList}
        onChange={(item) => this.props.setSelectedLiveStatus(item.value)} />
        );
      }

    render() {
        return (
            <div className="page-filter">
                <div className="row">

                     <div className="col-12 col-md-3 col-sm-3 col-lg-4 col-xl-2">
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
                    
                    <div className="col-12 col-md-3 col-sm-3 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("BaseCurrency")}</label>
                        {this._renderBaseCurrencySelectList()}
                        </div>
                    </div>

                    <div className="col-12 col-md-4 col-sm-3 col-lg-2 col-xl-3">
                        <div className="form-group">
                            <CheckBox id="IsConvertAll"
                        checked={this.props.isConvertAll}
                        onChange={(checked) => this.props.changeIsConvertAll(checked)}
                        label={LangHelper.getSingleResource("ConvertAllFiguresToBaseCurrency")} />
                        </div>    
                    </div>
                    </div>
                    <div className="row">
                    <div className="col-12 col-md-3 col-sm-3 col-lg-4 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Username")}</label>
                            <AutoComplete url={CUSTOMER_SEARCHING_URL} 
                                onSelect={(value) => this.props.setUsername(value)}
                                onInput={(value) => this.props.setUsername(value)}
                                htmlAttributes={{placeholder:LangHelper.getSingleResource("All")}} />
                        </div>
                    </div>

                     <div className="col-12 col-md-3 col-sm-3 col-lg-4 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Status")}</label>
                                {this._renderStatusSelectList()}
                        </div>
                    </div>

                    <div className="col-12 col-md-3 col-sm-3 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Platform")}</label>
                        {this._renderPlatformSelectList()}
                        </div>
                    </div>

                    <div className="col-12 col-md-2 col-sm-3 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Live")}</label>
                        {this._renderLiveStatusSelectList()}
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
    const { dateRange, selectedProduct, selectedCurrency, selectedBaseCurrency, username, selectedStatus, selectedPlatform, selectedLiveStatus } = state;
    return Object.assign({}, { dateRange, selectedProduct, selectedCurrency, selectedBaseCurrency, username, selectedStatus, selectedPlatform, selectedLiveStatus });
};

export default connect(mapStateToProps, mapDispatchToProps)(FormFilter);