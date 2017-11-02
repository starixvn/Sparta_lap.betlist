import React, { Component } from 'react'
import { connect } from 'react-redux'
import { ActionCreators } from '../actions/index.js'
import { bindActionCreators } from 'redux'
import { Table, Row, Cell } from '../../../../../components/fixed-table-header'
import LangHelper from '../../../../../lib/langHelper'
import ReportTitle from '../../../../../components/ReportTitle'
import NoDataTitle from '../../../../../components/NoDataTitle'
import service from '../lib/service'
import Site from '../../../../../lib/site'
import CollapseRow from '../../../../../components/CollapseRow'
import moment from 'moment'

export class DataGrid extends Component {
    constructor(props) {
        super(props);

        this.reportTitle = "";
    }

    componentWillMount() {
        const { resultAmendmentParam } = this.props;
        if (resultAmendmentParam && resultAmendmentParam.hasOwnProperty("FromDate")) {
            const dateRange = {
                fromDate: resultAmendmentParam.FromDate,
                toDate: resultAmendmentParam.ToDate
            };

            this.reportTitle = service.getReportTitle(dateRange);
        }
    }

    componentWillReceiveProps(nextProps) {
        if (this.props.resultAmendmentGridData !== nextProps.resultAmendmentGridData) {
            const { resultAmendmentDateRange } = nextProps;
            this.reportTitle = service.getReportTitle(resultAmendmentDateRange);
            this.props.toggleLoader(false);
        }
    }

    _buildHeader() {
        const headerText = service.getHeaderText();

        return (
            <Row style={{ height: 32 }}>
                <Cell colWidth={30} header={true} title="#">#</Cell>
                <Cell colWidth={150} header={true} title={headerText.dateTime}>{headerText.dateTime}</Cell>
                <Cell colWidth={340} header={true} title={headerText.content}>{headerText.content}</Cell>
                <Cell colWidth={30} header={true} title={headerText.ticketList}>{headerText.ticketList}</Cell>
            </Row>
        );
    }

    _buildBody() {
        const data = this.props.resultAmendmentGridData.Data;
        let rows = [];
        for (let i = 0; i < data.length; i++) {
            const item = data[i];
            const datetimeString = moment(item.DateTime).format("MM/DD/YYYY hh:mm:ss A");
            rows.push(
                <Row key={`tr${i}`}>
                    <Cell title={i + 1} className="center-align">{i + 1}</Cell>
                    <Cell title={datetimeString} className="center-align">{datetimeString}</Cell>
                    <Cell title={item.Subject}>{
                        Site.asNull(item.Content) === null
                            ? <div>{item.Subject}</div>
                            : <CollapseRow subject={item.Subject} content={item.Content} />
                    }
                    </Cell>
                    <Cell title={LangHelper.getSingleResource("ExportToExcel")} className="center-align"><i className="icon-excel"></i></Cell>
                </Row>)
        }

        return rows;
    }

    _renderNodata() {
        return (
            <div className="result-content">
                <ReportTitle content={this.reportTitle} />
                <NoDataTitle />
            </div>
        )
    }

    render() {
        const { resultAmendment } = this.props;
        if (!resultAmendment) {
            return null;
        }

        const { Data } = resultAmendment.resultAmendmentGridData;
        if (!Data) {
            return null;
        }

        if (!Data || !Data.length) {
            return this._renderNodata();
        }

        return (
            <div className="result-content">
                <ReportTitle content={this.reportTitle} />

                <Table
                    minWidth={550}
                    autoWidth={true}
                    bodyHeight={300}
                    header={this._buildHeader()}
                    body={this._buildBody()} />
            </div>
        );
    }
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
}

function mapStateToProps(state) {
    const { resultAmendmentGridData, resultAmendmentDateRange, resultAmendmentParam } = state.resultAmendment;
    return Object.assign({}, { resultAmendmentGridData, resultAmendmentDateRange, resultAmendmentParam });
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);