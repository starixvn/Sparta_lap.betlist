import React, { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import Select from 'react-select'
import { DateRange } from '../../../../components/date-range/index'
import AutoComplete from '../../../../components/auto-complete'

import { ActionCreators } from '../actions/index.js'
import LangHelper from '../../../../lib/langHelper'
import Site from '../../../../lib/site'
import service from '../lib/service'

const CUSTOMER_SEARCHING_URL = Site.resolveClientUrl("CustomerAPI/SearchCustomer");
const RANGE_MONTH_CONSTRAINT = 3;

export class MultiCustomerFormFilter extends Component {
    constructor(props) {
        super(props);
    }

    get isValidModel() {
        return this.props.refno !== "";
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

    exportExcel = (event) => {
        event.preventDefault();
        
        const params = this._getParams();
        if (!params) {
            return;
        }

        this.props.toggleLoader(true);
        this.props.exportExcel(params).then(response => {
            this.props.toggleLoader(false);
        });
    }

    _getParams() {
        if (!this.props.xsrfToken || !this.isValidModel) {
            return null;
        }

        return {
            xsrfToken: this.props.xsrfToken,
            data: {
                Refno: this.props.refno,
                Username: this.props.username,
                BaseCurrency: this.props.selectedBaseCurrency,
                IsConvertAll: this.props.isConvertAll
            }
        }
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
                    <div className="col-12 col-md-3 col-sm-3 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("RefNo")} *</label>
                            <input className="input-text" id="RefNo" name="RefNo" type="text" value={this.props.refno} placeholder={LangHelper.getSingleResource("RefNo")}
                            onChange={(event) => this.props.setRefno(event.target.value)} />
                        </div>
                    </div>

                    <div className="col-12 col-md-3 col-sm-2 col-lg-3 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Username")}</label>
                            <input className="input-text" id="Username" name="Username" type="text" value={this.props.username} placeholder={LangHelper.getSingleResource("Username")} 
                            onChange={(event) => this.props.setUsername(event.target.value)} />
                        </div>
                    </div>

                    <div className="col-12 col-md-2 col-sm-2 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("BaseCurrency")}</label>
                        {this._renderBaseCurrencySelectList()}
                        </div>
                    </div>

                    <div className="col-12 col-md-2 col-sm-4 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <div className="checkbox-group">
                                <input type="checkbox" className="checkbox" checked={(this.props.groupBy === 1 && this.props.selectedCurrency.value == 0) || this.props.isConvertAll} id="IsConvertAll"
                                 onChange={() => this.props.changeIsConvertAll(!this.props.isConvertAll)} />
                                    <label htmlFor="IsConvertAll">{LangHelper.getSingleResource("ConvertAllFiguresToBaseCurrency")}</label>
                        </div>
                        </div>    
                    </div>

                    <div className="col-12 col-md-1 col-sm-1 col-lg-1 col-xl-1">
                    <div className="form-group">
                        <div className="button-group">
                        <button type="button" disabled={!this.isValidModel} className="btn btn-submit" onClick={this.submit}>{LangHelper.getSingleResource("Submit")}</button>
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
    return Object.assign({}, state);
};

export default connect(mapStateToProps, mapDispatchToProps)(MultiCustomerFormFilter);