import React, { Component } from 'react'
import { connect } from 'react-redux'
import { ActionCreators } from '../actions/index.js'
import { bindActionCreators } from 'redux'
import { Table, Row, Cell } from '../../../components/fixed-table-header'
import LangHelper from '../../../lib/langHelper'
import NumberFormat from '../../../lib/numberFormat'
import ReportTitle from '../../../components/ReportTitle'
import NoDataTitle from '../../../components/NoDataTitle'

export class DataGrid extends Component {
    constructor(props) {
        super(props);

        this.reportTitle = "";
    }

    render() {
      
        return (
            <div className="result-content">

            </div>
        );
    }
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
};

function mapStateToProps(state) {
    const { gridData, dateRange, selectedCurrency, selectedReportBy, excludeTestDataOption, isLoading } = state;
    return Object.assign({}, { gridData, dateRange, selectedCurrency, selectedReportBy, excludeTestDataOption, isLoading });
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);