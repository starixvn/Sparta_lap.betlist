import React, { Component } from 'react'
import { connect } from 'react-redux'
import { ActionCreators } from '../actions/index.js'
import { bindActionCreators } from 'redux'
import Select from 'react-select'
import MultipleSelect from '../../../components/multiple-select'
import { Calendar, DateRange } from 'lrfcomponents/date-range/index'
import LangHelper from 'lrflib/langHelper'
import ReportExcel from '../lib/reportExcel';

const RANGE_MONTH_CONSTRAINT = 3;

class FormFilter extends Component {
    constructor(props) {
        super(props);
    }

    get isValidModel() {
        const { selectedSubsidiary, selectedProduct, isValidDateRange } = this.props;
        return selectedSubsidiary !== "" && selectedProduct !== "" && isValidDateRange;
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

        this.props.exportExcel(params).then(response => {
            const excel = new ReportExcel(response.data);
            excel.export();
        });
    }

    _getParams() {
        if (!this.props.xsrfToken || !this.isValidModel) {
            return null;
        }

        const fromDate = this.props.dateRange.fromDate;
        const toDate = this.props.dateRange.toDate;

        return {
            xsrfToken: this.props.xsrfToken,
            data: {
                BaseCurrency: this.props.selectedBaseCurrency,
                SubsidiaryList: this.props.selectedSubsidiary,
                ProductCodeList: this.props.selectedProduct,
                ConvertAll: this.props.convertAll,
                FromDate: typeof fromDate === "string" ? fromDate : fromDate.format("MM/DD/YYYY"),
                ToDate: typeof toDate === "string" ? toDate : toDate.format("MM/DD/YYYY")
            }
        }
    }

    productChangeHandler = (currentItem, selectedItemsKey) => {
        this.props.setSelectedProduct(selectedItemsKey)
    }

    subsidiaryChangeHandler = (currentItem, selectedItemsKey) => {
        this.props.setSelectedSubsidiary(selectedItemsKey)
    }

    applyDateRangeHandler = (date, isValidMonthRange = true) => {
        const { setValidDateRange, setDateRange } = this.props;

        if (!isValidMonthRange) {
            const message = LangHelper.getSingleResource("DateRangeExceedMsg").replace("{0}", RANGE_MONTH_CONSTRAINT);
            alert(message);
            setValidDateRange(false);
            return;
        }

        setValidDateRange(true);
        setDateRange(date);
    }

    _renderSubsidiarySelectList() {
        const { subsidiaryList } = this.props;

        return subsidiaryList && subsidiaryList.length ? (
            <MultipleSelect
                dataSource={this.props.subsidiaryList}
                keyField={"ID"}
                valueField={"Name"}
                statusField={"Checked"}
                onChange={this.subsidiaryChangeHandler} />
        ) : null;
    }

    _renderProductSelectList() {
        const { productOptions } = this.props;

        return productOptions && productOptions.length ? (
            <MultipleSelect
                dataSource={this.props.productOptions}
                keyField={"Code"}
                valueField={"Name"}
                statusField={"Status"}
                onChange={this.productChangeHandler} />
        ) : null;
    }

    _renderBaseCurrencySelectList() {
        const { baseCurrencyList, selectedBaseCurrency } = this.props;

        return baseCurrencyList && baseCurrencyList.length ? (
            <Select id="ddlBased"
                name="form-field-name"
                value={selectedBaseCurrency}
                options={baseCurrencyList}
                onChange={(item) => this.props.setSelectedBaseCurrency(item.value)} />
        ) : null;
    }

    _renderDateRange() {
        const { dateRange, dateConstraint } = this.props;

        return (
            <DateRange startDate={dateRange.fromDate} endDate={dateRange.toDate}
                minDate={dateConstraint.minDate} maxDate={dateConstraint.maxDate}
                limitedMonthRange={RANGE_MONTH_CONSTRAINT}
                onChange={this.applyDateRangeHandler} />
        )
    }

    render() {
        return (
            <div className="page-filter">
                <div className="row">
                    <div className="col-12 col-md-3 col-sm-3 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Subsidiary")}</label>
                            {this._renderSubsidiarySelectList()}
                        </div>
                    </div>

                    <div className="col-12 col-md-4 col-sm-4 col-lg-3 col-xl-3">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Date")}</label>
                            {this._renderDateRange()}
                        </div>
                    </div>

                    <div className="col-12 col-md-3 col-sm-3 col-lg-3 col-xl-3">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Product")}</label>
                            {this._renderProductSelectList()}
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
                    <div className="col-12 col-md-5 col-sm-5 col-lg-5 col-xl-3">
                        <div className="form-group">
                            <div className="checkbox-group">
                                <input type="checkbox" className="checkbox" checked={this.props.convertAll} id="IsConvertAll"
                                    onClick={() => this.props.changeConvertAll(!this.props.convertAll)} />
                                <label htmlFor="IsConvertAll">{LangHelper.getSingleResource("ConvertAll")}</label>
                            </div>

                            <div className="button-group">
                                <button disabled={!this.isValidModel} type="button" className="btn btn-submit" onClick={this.submit}>{LangHelper.getSingleResource("Submit")}</button>
                            </div>
                        </div>
                    </div>
                </div>

                <div className="caret-block">
                    <i className="icon-excel right" onClick={this.exportExcel} title={LangHelper.getSingleResource("ExportToExcel")}></i>
                </div>
            </div>
        );
    }
}
function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
}
function mapStateToProps(state) {
    const { containerWidth, ...rest } = state;
    return Object.assign({}, rest);
};

export default connect(mapStateToProps, mapDispatchToProps)(FormFilter);