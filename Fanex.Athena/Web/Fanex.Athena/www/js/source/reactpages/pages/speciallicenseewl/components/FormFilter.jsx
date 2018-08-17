import React, { Component } from 'react'
import { connect } from 'react-redux'
import { ActionCreators } from '../actions/index.js'
import { bindActionCreators } from 'redux'
import Select from 'react-select'
import MultipleSelect from '../../../components/multiple-select'
import { DateRange } from '../../../components/date-range/index'
import LangHelper from '../../../lib/langHelper'
import Site from '../../../lib/site'
import Report from '../../../lib/report'
import { Service, Url } from '../lib/service'

export class FormFilter extends Component {
    constructor(props) {
        super(props);

        this.state = {
            expandTurnover: false,
            expandLicenseeWL: false
        }
    }

    get isValidModel() {
        return this.props.selectedProduct !== "";
    }

    submit = (event) => {
        event.preventDefault();
        if (!this.isValidModel) {
            return;
        }

        const params = this._getParams();
        if (!params) {
            return;
        }

        this.props.toggleLoader(true);
        Report.fetchGridData(Url, { xsrfToken: this.props.xsrfToken, data: params })
            .then(response => {
                this.props.setGridData(response.data);
                this.props.setReportTitle(Service.getReportTitle(params.FromDate, params.ToDate, params.CurrencyId, params.CurrencyName, params.BaseCurrency));
            })
            .catch(error => {
                console.log(error);
                Service.showErrorMsg();
                this.props.toggleLoader(false);
            });     
        this.props.setParams(params);
    }

    exportExcel = (event) => {
        event.preventDefault();
    }

    _getParams() {
        if (!this.props.xsrfToken) {
            return null;
        }

        const { fromDate, toDate } = this.props.dateRange;

        return {
            FromDate: Site.convertDateToString(fromDate),
            ToDate: Site.convertDateToString(toDate),
            CurrencyName: this.props.selectedCurrency.label,
            CurrencyId: this.props.selectedCurrency.value,
            BaseCurrency: this.props.selectedCurrency.baseCurrency,
            ProductCodeList: this.props.selectedProduct,
            ReportBy: this.props.selectedReportBy,
            IsExcludedTestData: this.props.isExcludeTest,
            DrillDownLevel: 0,
            PageIndex: 1,
            PageSize: 2
        }
    }

    productChangeHandler = (currentItem, selectedItemsKey) => {
        this.props.setSelectedProduct(selectedItemsKey);
    }

    applyDateRangeHandler = (date, isValidMonthRange = true) => {
        if (isValidMonthRange) {
            this.props.setDateRange(date);
        }
        else {
            const message = LangHelper.getSingleResource("DateRangeExceedMsg").replace("{0}", dateConstraint.limitedMonthRange);
            alert(message);
        }
    }

    _renderReportByOption() {
        const { reportByList, selectedReportBy } = this.props;

        return (
            <div className="form-group">
                <label className='label-text'>{LangHelper.getSingleResource('ReportBy')}</label>
                {
                    reportByList.map((option, i) => {
                        const key = 'ReportBy' + i;
                        return (
                            <span key={`Group${key}`}>
                                <input type='radio' value={option.value} checked={selectedReportBy === option.value} id={key}
                                    onChange={(event) => this.props.setSelectedReportBy(event.target.value)} />
                                <label htmlFor={key}>{option.label}</label>
                            </span>)
                    })
                }
            </div>
        );
    }

    _renderExcludeTestData() {
        const { isExcludeTest } = this.props;

        return (
            <div className="form-group">
                <label className='label-text'>{LangHelper.getSingleResource('ExcludeTestData')}</label>
                <span>
                    <input type='radio' value={true} checked={isExcludeTest === true} id='ExcludeTestData'
                        onChange={() => this.props.changeIsExcludeTest(true)} />
                    <label htmlFor='ExcludeTestData'>{LangHelper.getSingleResource('Yes')}</label>
                </span>
                <span>
                    <input type='radio' value={false} checked={isExcludeTest === false} id='IncludeTestData'
                        onChange={() => this.props.changeIsExcludeTest(false)} />
                    <label htmlFor='IncludeTestData'>{LangHelper.getSingleResource('No')}</label>
                </span>
            </div>
        );
    }

    _renderProductSelectList() {
        const { productOptions } = this.props;
        return productOptions && productOptions.length && (
            <MultipleSelect id="ProductList"
                dataSource={productOptions}
                keyField={"Code"}
                valueField={"Name"}
                statusField={"Status"}
                optionAllLabel={LangHelper.getSingleResource("All")}
                noneSelectedLabel={LangHelper.getSingleResource("SelectOptions")}
                onChange={this.productChangeHandler} />
        );
    }

    _renderCurrencySelectList() {
        const { currencyList, selectedCurrency } = this.props;

        return <Select instanceId="ddlCurrency"
                value={selectedCurrency.value}
                options={currencyList}
                onChange={(item) => this.props.setSelectedCurrency(item)} />
    }

    _renderDateRange() {
        const { dateRange, dateConstraint } = this.props;

        return (
            <DateRange startDate={dateRange.fromDate} endDate={dateRange.toDate}
                minDate={dateConstraint.minDate} maxDate={dateConstraint.maxDate}
                today={dateRange.toDate} limitedMonthRange={dateConstraint.limitedMonthRange}
                onChange={this.applyDateRangeHandler} />
        );
    }

    render() {
        return (
            <div className="page-filter">
                <div className="row">
                    <div className="col-12 col-md-5 col-sm-12 col-lg-4 col-xl-3">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Date")}</label>
                            {this._renderDateRange()}
                        </div>
                    </div>
                    <div className="col-12 col-md-3 col-sm-12 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Currency")}</label>
                            {this._renderCurrencySelectList()}
                        </div>
                    </div>
                    <div className="col-12 col-md-4 col-sm-12 col-lg-3 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Product")}</label>
                            {this._renderProductSelectList()}
                        </div>
                    </div>
                </div>

                <div className="row">
                    <div className='col-12 col-md-5 col-sm-12 col-lg-4 col-xl-3'>
                        {this._renderReportByOption()}
                    </div>
                    <div className="col-12 col-md-3 col-sm-12 col-lg-2 col-xl-2">
                        {this._renderExcludeTestData()}
                    </div>
                    <div className="col-12 col-md-3 col-sm-12 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <div className="button-group">
                            <button disabled={!this.isValidModel} type="button" className="btn btn-submit" onClick={this.submit}>{LangHelper.getSingleResource("Submit")}</button>
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
        );
    }
}
function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
}
function mapStateToProps(state) {
    const { gridData, reportTitle, params, isLoading, setting, ...rest } = state;
    return Object.assign({}, rest);
};

export default connect(mapStateToProps, mapDispatchToProps)(FormFilter);