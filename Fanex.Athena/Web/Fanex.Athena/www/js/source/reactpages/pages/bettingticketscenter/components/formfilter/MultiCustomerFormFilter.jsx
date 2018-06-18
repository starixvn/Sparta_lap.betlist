import React, { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import Select from 'react-select'
import { DatePicker } from '../../../../components/date-range'
import AutoComplete from '../../../../components/auto-complete'

import { ActionCreators } from '../../actions'
import LangHelper from '../../../../lib/langHelper'
import Site from '../../../../lib/site'

const CUSTOMER_SEARCHING_URL = Site.resolveClientUrl("CustomerAPI/SearchCustomer");

export class MultiCustomerFormFilter extends Component {
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
        return {
            xsrfToken: this.props.xsrfToken,
            data: {
                Date: '05/31/2018',
                ProductCode: 'SB',
                Status: 'won',
                CurrencyId: 0
            }
        }
    }

    _onSelectAutoCompleteItem(selectedItem) {
        console.log(selectedItem);
    }

    _renderDateRange() {
        return (
            <DatePicker onChange={this.handleChange} />
        )
    }

    _renderProductSelectList() {
        const productOptions = [{ "value": "SB", "label": "Sportsbook" }, { "value": "RB", "label": "Racing" }];
        const selectedProduct = productOptions[0];

        return productOptions && productOptions.length ? (
            <Select id="ddlBased"
                name="form-field-name"
                value={selectedProduct}
                options={productOptions} />
        ) : null;
    }

    productChangeHandler() {

    }

    _renderBaseCurrencySelectList() {
        //const { baseCurrencyList, selectedBaseCurrency } = this.props;
        const baseCurrencyList = [{ "value": "EUR", "label": "EUR" }, { "value": "USD", "label": "USD" }, { "value": "GBP", "label": "GBP" }];
        const selectedBaseCurrency = "EUR";

        return baseCurrencyList && baseCurrencyList.length ? (
            <Select id="ddlBased"
                name="form-field-name"
                value={selectedBaseCurrency}
                options={baseCurrencyList} />
        ) : null;
    }

    render() {
        return (
            <div className="page-filter">
                <div className="row">
                    <div className="col-12 col-md-3 col-sm-3 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Date")}</label>
                            {this._renderDateRange()}
                        </div>
                    </div>

                    <div className="col-12 col-md-3 col-sm-2 col-lg-3 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Product")}</label>
                            {this._renderProductSelectList()}
                        </div>
                    </div>

                    <div className="col-12 col-md-3 col-sm-3 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("League")}</label>
                            {this._renderProductSelectList()}
                        </div>
                    </div>

                    <div className="col-12 col-md-3 col-sm-3 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Match")}</label>
                            {this._renderProductSelectList()}
                        </div>
                    </div>

                    <div className="col-12 col-md-2 col-sm-2 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Currency")}</label>
                        {this._renderBaseCurrencySelectList()}
                        </div>
                    </div>

                    <div className="col-12 col-md-2 col-sm-2 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Based")}</label>
                        {this._renderBaseCurrencySelectList()}
                        </div>
                    </div>

                </div>

                <div className="row">
                    
                    <div className="col-12 col-md-2 col-sm-2 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Status")}</label>
                        {this._renderBaseCurrencySelectList()}
                        </div>
                    </div>

                    <div className="col-12 col-md-2 col-sm-2 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Platform")}</label>
                        {this._renderBaseCurrencySelectList()}
                        </div>
                    </div>

                    <div className="col-12 col-md-2 col-sm-2 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Live")}</label>
                        {this._renderBaseCurrencySelectList()}
                        </div>
                    </div>

                    <div className="col-12 col-md-2 col-sm-2 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <div className="button-group">
                                <button type="button" className="btn btn-submit" onClick={this.submit}>{LangHelper.getSingleResource("Submit")}</button>
                            </div>
                        </div>
                    </div>

                    <div className="caret-block">
                        <i className="icon-excel right" title={LangHelper.getSingleResource("ExportToExcel")}></i>
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
    return Object.assign({}, state);
};

export default connect(mapStateToProps, mapDispatchToProps)(MultiCustomerFormFilter);