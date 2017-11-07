import React, { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import Select from 'react-select'
import { DatePicker } from '../../../../components/date-range'
import AutoComplete from '../../../../components/auto-complete'

import { ActionCreators } from '../../actions/casinodetailed'
import LangHelper from '../../../../lib/langHelper'

export class FormFilter extends Component {
    constructor(props) {
        super(props);
    }

    _onSelectAutoCompleteItem(selectedItem) {
        console.log(selectedItem);
    }

    _renderDateRange() {
        return (
            <DatePicker onChange={this.handleChange} />
        )
    }

    handleChange = (date) => {
        console.log(date);
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

                    <div className="col-12 col-md-2 col-sm-2 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Username")}</label>
                            <AutoComplete url="/CustomerAPI/SearchCustomer" placeholder="External ID or System ID"
                                onSelect={this._onSelectAutoCompleteItem} />
                        </div>
                    </div>

                    <div className="col-12 col-md-3 col-sm-3 col-lg-1 col-xl-1">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Status")}</label>
                            {this._renderProductSelectList()}
                        </div>
                    </div>

                    <div className="col-12 col-md-2 col-sm-2 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <div className="button-group">
                                <button type="button" className="btn btn-submit">{LangHelper.getSingleResource("Submit")}</button>
                            </div>
                        </div>
                    </div>

                    <div className="caret-block">
                        <i className="icon-excel right" title={LangHelper.getSingleResource("ExportToExcel")}></i>
                    </div>
                </div>

                <div className="row">
                    <div className="col-12 col-md-12 col-sm-12 col-lg-12 col-xl-12">
                        <div className="form-group">
                            <p>Note: Data is available from MM/DD/YYYY</p>
                        </div>
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

export default connect(mapStateToProps, mapDispatchToProps)(FormFilter);