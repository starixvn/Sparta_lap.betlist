import React, { Component } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from '../actions/index.js'
import { bindActionCreators } from 'redux'
import Select from 'react-select';
import { Calendar, DateRange, defaultOptions } from '../../../components/date-range/index';

class FormFilter extends Component {
    constructor(props) {
        super(props);
    }

    _submit() {
        if(!window.xsrfToken){
            return;
        }
        
        const params = {
            BaseCurrency: this.props.selectedBaseCurrency,
            SubsidiaryList: this.props.selectedSubsidiary,
            ProductCodeList: this.props.selectedProduct,
            ConvertAll: this.props.convertAll,
            FromDate: this.props.dateRange.fromDate,
            ToDate: this.props.dateRange.toDate
        }

        this.props.fetchData(params, window.xsrfToken);
    }

    _renderSubsidiarySelectList() {
        const { subsidiaryList, selectedSubsidiary } = this.props;

        return subsidiaryList && subsidiaryList.length > 0 ? (
            <Select id="ddlOperator" className="input-select"
                name="form-field-name"
                value={selectedSubsidiary}
                options={subsidiaryList}
                onChange={(item) => this.props.setSubsidiary(item.value)} />
        ) : null;
    }

    _renderProductSelectList() {
        const { productOptions, selectedProduct } = this.props;

        return productOptions && productOptions.length > 0 ? (
            <Select id="ddlProduct"
                name="form-field-name"
                value={selectedProduct}
                options={productOptions}
                onChange={(item) => this.props.setSelectedProduct(item.value)} />
        ) : null;
    }

    _renderBaseCurrencySelectList() {
        const { baseCurrencyList, selectedBaseCurrency } = this.props;

        return baseCurrencyList && baseCurrencyList.length > 0 ? (
            <Select id="ddlBased"
                name="form-field-name"
                value={selectedBaseCurrency}
                options={baseCurrencyList}
                onChange={(item) => this.props.setSelectedBaseCurrency(item.value)} />
        ) : null;
    }

    render() {
        return (
            <div className="page-filter">
                <div className="row">
                    <div className="col-lg-2 col-md-2 col-sm-2 col-xs-12">
                        <div className="form-group">
                            <label className="label-text">Operator</label>
                            {this._renderSubsidiarySelectList()}
                        </div>
                    </div>

                    <div className="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                        <div className="form-group">
                            <label className="label-text">Date</label>

                            <DateRange minDate='01-02-2017' onlyClasses={true}
                                ranges={defaultOptions.ranges}
                                onClickApplyButton={(date) => this.props.setDateRange(date)} />
                        </div>
                    </div>

                    <div className="col-lg-2 col-md-2 col-sm-2 col-xs-12">
                        <div className="form-group">
                            <label className="label-text">Product</label>
                            {this._renderProductSelectList()}
                        </div>
                    </div>

                    <div className="col-lg-2 col-md-2 col-sm-2 col-xs-12">
                        <div className="form-group">
                            <label className="label-text">Based</label>
                            {this._renderBaseCurrencySelectList()}
                        </div>
                    </div>
                </div>

                <div className="row">
                    <div className="col-lg-2 col-md-2 col-sm-2 col-xs-12">
                        <div className="form-group">
                            <div className="checkbox-group">
                                <input type="checkbox" checked={this.props.convertAll} id="IsConvertAll"
                                 onClick={() => this.props.changeConvertAll(!this.props.convertAll)} />
                                <label htmlFor="IsConvertAll">Convert All</label>
                            </div>

                            <div className="button-group">
                                <button type="button" className="btn btn-submit" onClick={() => this._submit()}>Submit</button>
                            </div>
                        </div>
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
    const { gridData, ...rest } = state;
    return Object.assign({}, rest);
};

export default connect(mapStateToProps, mapDispatchToProps)(FormFilter);