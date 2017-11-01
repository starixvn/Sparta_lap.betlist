import React, { Component } from 'react'
import { connect } from 'react-redux'
import { ActionCreators } from '../actions/index'
import { bindActionCreators } from 'redux'
import { Table, Row, Cell } from '../../../../../components/fixed-table-header'
import ReportTitle from '../../../../../components/ReportTitle'
import NoDataTitle from '../../../../../components/NoDataTitle'
import service from '../lib/service'
import CollapseRow from '../../../../../components/CollapseRow'
import Site from '../../../../../lib/site'
import moment from 'moment'

export class DataGrid extends Component {
    constructor(props) {
        super(props);

        this.reportTitle = "";
    }

    componentWillMount() {
        const { notificationParam } = this.props;
        if(notificationParam !== null && notificationParam.hasOwnProperty("FromDate")) {
            const dateRange = {
                fromDate: notificationParam.FromDate,
                toDate: notificationParam.ToDate
            };

            this.reportTitle = service.getReportTitle(dateRange);
        }
    }

    componentWillReceiveProps(nextProps) {
        if (this.props.notificationGridData !== nextProps.notificationGridData) {
            const { notificationDateRange } = nextProps;
            this.reportTitle = service.getReportTitle(notificationDateRange);
            this.props.toggleLoader(false);
        }
    }

    _buildHeader() {
        const headerText = service.getHeaderText();

        return (
            <Row style={{ height: 32 }}>
                <Cell colWidth={30} header={true} title="#">#</Cell>
                <Cell colWidth={150} header={true} title={headerText.type}>{headerText.type}</Cell>
                <Cell colWidth={150} header={true} title={headerText.dateTime}>{headerText.dateTime}</Cell>
                <Cell colWidth={320} header={true} title={headerText.content}>{headerText.content}</Cell>
            </Row>
        )
    }

    _buildBody() {
        const data = this.props.notificationGridData.Data;
        let rows = [];
        for (let i = 0; i < data.length; i++) {
            const item = data[i];
            const datetimeString = moment(item.DateTime).format("MM/DD/YYYY hh:mm:ss A");
            rows.push(
                <Row key={`tr${i}`}>
                    <Cell title={i+1} className="center-align">{i+1}</Cell>
                    <Cell title={item.Type}>{item.Type}</Cell>
                    <Cell title={datetimeString} className="center-align">{datetimeString}</Cell>
                    <Cell title={item.Subject}>{
                        Site.asNull(item.Content) === null
                        ? <div>{item.Subject}</div>
                        : <CollapseRow subject={item.Subject} content={item.Content} />
                    }
                    </Cell>
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
        const { Data } = this.props.notificationGridData;
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
                    minWidth={650}
                    autoWidth={true}
                    bodyHeight={300}
                    header={this._buildHeader()}
                    body={this._buildBody()}
                />
            </div>
        );
    }
}
function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
}
function mapStateToProps(state) {
    const { notificationGridData, notificationDateRange, notificationParam } = state;
    return Object.assign({}, { notificationGridData, notificationDateRange, notificationParam });
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);