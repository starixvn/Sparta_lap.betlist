import React, { Component } from 'react'
import { connect } from 'react-redux'
import { ActionCreators } from '../actions/index.js'
import { bindActionCreators } from 'redux'
import Select from 'react-select'
import { DateRange } from '../../../components/date-range/index'
import LangHelper from '../../../lib/langHelper'
import Site from '../../../lib/site'

const RANGE_MONTH_CONSTRAINT = 3;

export class FormFilter extends Component {
    constructor(props) {
        super(props);

        this.props.setParams([]);
    }

    submit = (event) => {
        event.preventDefault();
        const params = this._getParams(0);
        if (!params) {
            return;
        }

        this.props.toggleLoader(true);
        this.props.getGridData(params);
    }

    _getParams(level, currencyId, fromDate, toDate, custId) {
        if (!this.props.xsrfToken) {
            return null;
        }

        let params = this.props.params;
        let param = {};

        if (level === 0) {
            const { fromDate, toDate } = this.props.dateRange;

            param = {
                CurrencyId: this.props.selectedCurrency.value,
                FromDate: Site.convertDateToString(fromDate),
                ToDate: Site.convertDateToString(toDate),
                ReportBy: this.props.selectedReportBy,
                ExcludeTestData: this.props.selectedExcludeTestData,
                Level: level
            };
        }
        else {
            const param0 = params[0];
            param = {
                CurrencyId: currencyId,
                FromDate: Site.convertDateToString(fromDate),
                ToDate: Site.convertDateToString(toDate),
                ReportBy: param0.ReportBy,
                ExcludeTestData: param0.ExcludeTestData,
                Level: level,
                CustId: custId
            };
        }

        params[level] = param;       
        this.props.setParams(params);

        return {
            xsrfToken: this.props.xsrfToken,
            data: param
        };

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

    changeReportBy = (event) => {
        if (this.props.selectedReportBy !== event.target.value) {
            this.props.setReportBy(parseInt(event.target.value));
        }
    }

    changeExcludeTestData = (event) => {
        if (this.props.selectedExcludeTestData !== event.target.value) {
            this.props.setExcludeTestData(event.target.value === 'true');
        }
    }

    _renderDateRange() {
        const { dateRange, dateConstraint } = this.props;

        return (
            <DateRange startDate={dateRange.fromDate} endDate={dateRange.toDate}
                minDate={dateConstraint.minDate} maxDate={dateConstraint.maxDate}
                today={dateRange.toDate} limitedMonthRange={RANGE_MONTH_CONSTRAINT}
                onChange={this.applyDateRangeHandler} />
        );
    }

    _renderCurrencySelectList() {
        const { currencyList, selectedCurrency } = this.props;
        
        return <Select id="ddlCurrency"
            name="form-field-name"
            value={selectedCurrency}
            options={currencyList}
            onChange={(item) => this.props.setSelectedCurrency(item.value)} />
    }    

    _renderReportByOption() {
        const { reportByOption, selectedReportBy } = this.props;

        return (<div className="form-group">
            <label className="label-text">{LangHelper.getSingleResource("ReportBy")}</label>
            {   
                reportByOption.map((option, i) => {
                    const key = 'ReportBy' + i;
                    return (<span key={`Group${key}`}>
                        <input type="radio" value={option.value} checked={selectedReportBy === option.value} id={key}
                            onChange={this.changeReportBy} />
                        <label htmlFor={key}>{option.label}</label>
                    </span>)  
                })
            }
        </div>);
    }

    _renderExcludeTestDataOption() {
        const { excludeTestDataOption, selectedExcludeTestData } = this.props;

        return (<div className="form-group"> 
            <label className="label-text">{LangHelper.getSingleResource("ExcludeTestData")}</label>
            {   
                excludeTestDataOption.map((option, i) => {
                    const key = 'ExcludeTestData' + i;
                    return (<span key={`Group${key}`}>
                        <input type="radio" value={option.value} checked={selectedExcludeTestData === option.value} id={key} key={key}
                            onChange={this.changeExcludeTestData} />
                        <label htmlFor={key} key={`Label${key}`}>{option.label}</label>
                    </span>)   
                })
            }
        </div>);
    }

    render() {
        return (
            <div className="page-filter">
                <div className="row">
                    <div className="col-12 col-sm-12 col-md-4 col-xl-3">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Date")}</label>
                            {this._renderDateRange()}
                        </div>
                    </div>

                     <div className="col-12 col-sm-12 col-md-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Currency")}</label>
                            {this._renderCurrencySelectList()}
                        </div>
                    </div>

                    <div className="col-12 col-sm-12 col-md-2 col-xl-2">
                        {this._renderReportByOption()}
                    </div>

                    <div className="col-12 col-sm-12 col-md-2 col-xl-2">
                        {this._renderExcludeTestDataOption()}
                    </div>

                    <div className="col-12 col-sm-12 col-md-2 col-xl-2">
                        <div className="form-group button-group">
                            <button type="button" className="btn btn-submit" onClick={this.submit}>{LangHelper.getSingleResource("Submit")}</button>
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
};

function mapStateToProps(state) {
    const { gridData, ...rest } = state;
    return Object.assign({}, rest);
};

export default connect(mapStateToProps, mapDispatchToProps)(FormFilter);