import React, { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import Select from 'react-select'
import { DateRange } from '@jsroot/components/date-range/index'
import AutoComplete from '@jsroot/components/auto-complete'
import CheckBox from '@jsroot/components/CheckBox'
import { ActionCreators } from '../actions/index.js'
import LangHelper from '@jsroot/lib/langHelper'
import Site from '@jsroot/lib/site'
import AppData from '@jsroot/lib/AppData'

const CUSTOMER_SEARCHING_URL = Site.resolveClientUrl("SearchCustomerAPI/SearchCustomer");

export class FormFilter extends Component {
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

        params.data.IsExport = true;
        this.props.toggleLoader(true);
        this.props.exportExcel(params).then((response) => {
            const url = window.URL.createObjectURL(new Blob([response.data]));
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', 'BettingTicketsCenter_RefNo.xls');
            document.body.appendChild(link);
            link.click();
            link.parentNode.removeChild(link);
            this.props.toggleLoader(false);
        });
    }

    _getParams() {
        if (!this.isValidModel) {
            return null;
        }

        return {
            data: {
                Refno: this.props.refno,
                Username: this.props.username,
                BaseCurrency: this.props.selectedBaseCurrency,
                IsConvertAll: this.props.isConvertAll
            }
        }
    }

    _renderBaseCurrencySelectList() {
        const selectedBaseCurrency = this.props.selectedBaseCurrency;
        const baseCurrencyList = AppData.baseCurrencyList;

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
                    <div className="col-12 col-md-4 col-sm-4 col-lg-3 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("RefNo")} *</label>
                            <input className="input-text" id="RefNo" name="RefNo" type="text" value={this.props.refno} placeholder={LangHelper.getSingleResource("RefNo")}
                                onChange={(event) => this.props.setRefno(event.target.value)} />
                        </div>
                    </div>

                    <div className="col-12 col-md-4 col-sm-4 col-lg-3 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Username")}</label>
                            <AutoComplete url={CUSTOMER_SEARCHING_URL} 
                                onSelect={(value) => this.props.setUsername(value)}
                                onInput={(value) => this.props.setUsername(value)}
                                />
                        </div>
                    </div>

                    <div className="col-12 col-md-3 col-sm-4 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("BaseCurrency")}</label>
                        {this._renderBaseCurrencySelectList()}
                        </div>
                    </div>

                    <div className="col-12 col-md-4 col-sm-4 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <CheckBox id="IsConvertAll"
                            checked={this.props.isConvertAll}
                            onChange={(checked) => this.props.changeIsConvertAll(checked)}
                            label={LangHelper.getSingleResource("ConvertAllFiguresToBaseCurrency")} />
                        </div>    
                    </div>

                    <div className="col-12 col-md-3 col-sm-4 col-lg-2 col-xl-2">
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
    const { refno, username, selectedBaseCurrency, isConvertAll } = state;
    return Object.assign({}, {refno, username, selectedBaseCurrency, isConvertAll});
};

export default connect(mapStateToProps, mapDispatchToProps)(FormFilter);