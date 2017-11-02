import React, { Component } from 'react'
import { connect } from 'react-redux'
import { ActionCreators } from '../actions/index'
import { bindActionCreators } from 'redux'
import { DateRange } from '../../../../../components/date-range'
import LangHelper from '../../../../../lib/langHelper'
import Site from '../../../../../lib/site'
import service from '../lib/service'

const RANGE_MONTH_CONSTRAINT = 3;

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
        this.props.getResultAmendmentGridData(params);
    }

    _getParams() {
        if (!this.props.resultAmendmentXsrfToken) {
            return null;
        }

        const { fromDate, toDate } = this.props.resultAmendmentDateRange;

        const paramData = {
            FromDate: Site.convertDateToString(fromDate),
            ToDate: Site.convertDateToString(toDate),
        };

        this.props.setResultAmendmentParam(paramData);

        return {
            xsrfToken: this.props.resultAmendmentXsrfToken,
            data: paramData
        }
    }

    applyDateRangeHandler = (date, isValidMonthRange = true) => {
        if (isValidMonthRange) {
            this.props.setResultAmendmentDateRange(date);
        }
        else {
            const message = LangHelper.getSingleResource("DateRangeExceedMsg").replace("{0}", RANGE_MONTH_CONSTRAINT);
            alert(message);
        }
    }

    _renderDateRange() {
        const { resultAmendmentDateRange, resultAmendmentDateConstraint } = this.props;

        return (
            <DateRange startDate={resultAmendmentDateRange.fromDate} endDate={resultAmendmentDateRange.toDate}
                minDate={resultAmendmentDateConstraint.minDate} maxDate={resultAmendmentDateConstraint.maxDate}
                today={resultAmendmentDateRange.toDate} limitedMonthRange={RANGE_MONTH_CONSTRAINT}
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
                            <div className="button-group">
                                <button type="button" className="btn btn-submit" onClick={this.submit}>{LangHelper.getSingleResource("Submit")}</button>
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
    const { resultAmendmentGridData, ...rest } = state.resultAmendment;
    return Object.assign({}, rest);
};

export default connect(mapStateToProps, mapDispatchToProps)(FormFilter);