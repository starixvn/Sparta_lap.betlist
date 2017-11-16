import React, { Component } from 'react'
import { connect } from 'react-redux'
import { ActionCreators } from '../../actions/general'
import { bindActionCreators } from 'redux'
import Select from 'react-select'
import { DateRange } from '../../../../components/date-range'
import LangHelper from '../../../../lib/langHelper'
import Site from '../../../../lib/site'
import service from '../../lib/generalService'

const RANGE_MONTH_CONSTRAINT = 6;

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
        if (!this.props.xsrfToken) {
            return null;
        }

        let pageSize = 10;
        const param = this.props.param;
        if (param && param.hasOwnProperty("PageSize")) {
            pageSize = param.PageSize;
        }

        const { fromDate, toDate } = this.props.dateRange;

        const paramData = {
            FromDate: Site.convertDateToString(fromDate),
            ToDate: Site.convertDateToString(toDate),
            NotificationTypes: this.props.selectedType,
            PageSize: pageSize,
            PageIndex: 1
        };

        this.props.setParam(paramData);

        return {
            xsrfToken: this.props.xsrfToken,
            data: paramData
        }
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
    
    exportExcel = () => {

    }

    _renderNotificationTypeSelectList() {
        const { notificationTypes, selectedType } = this.props;

        return notificationTypes && notificationTypes.length ? (
            <Select id="ddlNotificationType"
                name="form-field-name"
                value={selectedType}
                options={notificationTypes}
                onChange={(item) => this.props.setSelectedType(item.value)} />
        ) : (
                <Select id="ddlNotificationType"
                    name="form-field-name"
                    value={0}
                    options={[]} />
            );
    }

    _renderDateRange() {
        const { dateRange, dateConstraint } = this.props;

        return (
            <DateRange startDate={dateRange.fromDate} endDate={dateRange.toDate}
                minDate={dateConstraint.minDate} maxDate={dateConstraint.maxDate}
                today={dateRange.toDate} limitedMonthRange={RANGE_MONTH_CONSTRAINT}
                onChange={this.applyDateRangeHandler} />
        )
    }

    render() {
        return (
            <div className="page-filter">
                <div className="row">
                    <div className="col-12 col-md-4 col-sm-4 col-lg-3 col-xl-3">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Date")}</label>
                            {this._renderDateRange()}
                        </div>
                    </div>

                    <div className="col-12 col-md-2 col-sm-3 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <label className="label-text">{LangHelper.getSingleResource("Type")}</label>
                            {this._renderNotificationTypeSelectList()}
                        </div>
                    </div>

                    <div className="col-12 col-md-2 col-sm-3 col-lg-2 col-xl-2">
                        <div className="form-group">
                            <div className="button-group">
                                <button type="button" className="btn btn-submit" onClick={this.submit}>{LangHelper.getSingleResource("Submit")}</button>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-12">
                        <span className="circle"></span>
                        {LangHelper.getSingleResource("NotificationDateRangeLimitNote")}
                    </div>
                </div>
                <div className="caret-block">
                    <i className="icon-excel right" onClick={this.exportExcel} title={LangHelper.getSingleResource("ExportToExcel")}></i>
                    <i className="icon-book right" title={LangHelper.getSingleResource("UserGuide")}></i>                 
                </div>
            </div>
        );
    }
}
function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
}
function mapStateToProps(state) {
    const { gridData, ...rest } = state.general;
    return Object.assign({}, rest, { xsrfToken: state.xsrfToken });
};

export default connect(mapStateToProps, mapDispatchToProps)(FormFilter);