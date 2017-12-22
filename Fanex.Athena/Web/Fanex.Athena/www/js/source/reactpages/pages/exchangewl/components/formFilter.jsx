import React, { Component } from 'react'
import { connect } from 'react-redux'
import { ActionCreators } from '../actions/index.js'
import { bindActionCreators } from 'redux'
import Select from 'react-select'
import { DateRange } from '../../../components/date-range/index'
import LangHelper from '../../../lib/langHelper'
import Site from '../../../lib/site'
import { ReportLevel } from '../../../constants/report'
import ReportExcel from '../lib/reportExcel'
import service from '../lib/service'

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

        let paramArr = this.props.params;
        paramArr[0] = params.data;       
        this.props.setParams(paramArr);
    }

    exportExcel = (event) => {
        event.preventDefault();

        const level = this.props.gridData.level || 0;

        const isAllowedCustomerComm = this.props.isAllowedCustomerComm;
        const params = this._getParams(level);
        if (!params) {
            return;
        }
     
        this.props.toggleLoader(true);
        if (level === 0) {           
            this.props.exportExcel(params).then(response => this._doExportExcel(params.data, isAllowedCustomerComm, response.data));
        }
        else {
            this._doExportExcel(params.data, isAllowedCustomerComm, this.props.gridData.data);
        }
    }

    _doExportExcel(param, isAllowedCustomerComm, data) {
        const title = service.getReportTitle({ fromDate: param.FromDate, toDate: param.ToDate }, param.CurrencyName, param.CurrencyId === 0);
        const excel = new ReportExcel(data, title, param.Level, isAllowedCustomerComm, param.ReportBy);
        excel.export();
        this.props.toggleLoader(false);
    }

    _getParams(level) {
        if (!this.props.xsrfToken) {
            return null;
        }

        let param = {};
        if (level === 0) {
            const { fromDate, toDate } = this.props.dateRange;

            param = {
                CurrencyId: this.props.selectedCurrency.value,
                CurrencyName: this.props.selectedCurrency.label,
                FromDate: Site.convertDateToString(fromDate),
                ToDate: Site.convertDateToString(toDate),
                ReportBy: this.props.selectedReportBy,
                ExcludeTestData: this.props.isExcludeTest,
                Level: level
            };
        }
        else {
            let params = this.props.params;
            param = params[level];
            if (!param) {
                param = params[0];
            }
        }

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
            const message = LangHelper.getSingleResource('DateRangeExceedMsg').replace('{0}', RANGE_MONTH_CONSTRAINT);
            alert(message);
        }
    }

    changeReportBy = (event) => {
        const newValue = parseInt(event.target.value);
        if (!isNaN(newValue) && this.props.selectedReportBy !== newValue) {
            this.props.setReportBy(newValue);
        }
    }

    changeExcludeTestData = (event) => {
        const newValue = event.target.value === 'true';
        if (this.props.selectedExcludeTestData !== newValue) {
            this.props.setExcludeTestData(newValue);
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
        
        return <Select id='ddlCurrency'
            name='form-field-name'
            value={selectedCurrency}
            options={currencyList}
            onChange={(item) => this.props.setSelectedCurrency(item)} />
    }

    _renderReportByOption() {
        const { reportByOption, selectedReportBy } = this.props;

        return (<div className='form-group'>
            <label className='label-text'>{LangHelper.getSingleResource('ReportBy')}</label>
            {   
                reportByOption.map((option, i) => {
                    const key = 'ReportBy' + i;
                    return (<span key={`Group${key}`}>
                        <input type='radio' value={option.value} checked={selectedReportBy === option.value} id={key}
                            onChange={this.changeReportBy} />
                        <label htmlFor={key}>{option.label}</label>
                    </span>)  
                })
            }
        </div>);
    }

    _renderExcludeTestDataCheckBox() {
        return (<div className="checkbox-group">
            <input type="checkbox" className="checkbox" checked={this.props.isExcludeTest} id="ExcludeTest"
                   onChange={() => this.props.changeIsExcludeTest(!this.props.isExcludeTest)} />
            <label style={{marginLeft: "0px"}} htmlFor="ExcludeTest">{LangHelper.getSingleResource("ExcludeTestData")}</label>
        </div>);
    }

    render() {
        return (
            <div className='page-filter'>
                <div className='row'>
                    <div className='col-12 col-sm-12 col-md-4 col-xl-3'>
                        <div className='form-group'>
                            <label className='label-text'>{LangHelper.getSingleResource('Date')}</label>
                            {this._renderDateRange()}
                        </div>
                    </div>

                     <div className='col-12 col-sm-12 col-md-2 col-xl-2'>
                        <div className='form-group'>
                            <label className='label-text'>{LangHelper.getSingleResource('Currency')}</label>
                            {this._renderCurrencySelectList()}
                        </div>
                    </div>

                    <div className='col-12 col-sm-12 col-md-2 col-xl-2'>
                        {this._renderReportByOption()}
                    </div>

                    <div className='col-12 col-sm-12 col-md-2 col-xl-2'>
                        <div className="form-group">
                           {this._renderExcludeTestDataCheckBox()}
                        </div>
                    </div>

                    <div className='col-12 col-sm-12 col-md-2 col-xl-2'>
                        <div className='form-group button-group'>
                            <button type='button' className='btn btn-submit' onClick={this.submit}>{LangHelper.getSingleResource('Submit')}</button>
                        </div>
                    </div>
                </div>
                <div className='caret-block'>
                    <i className='icon-excel right' onClick={this.exportExcel} title={LangHelper.getSingleResource('ExportToExcel')}></i>
                </div> 
            </div>
        );
    }
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
};

function mapStateToProps(state) {
    const { licenseeId, levelGridData, ...rest } = state;
    return Object.assign({}, rest);
};

export default connect(mapStateToProps, mapDispatchToProps)(FormFilter);