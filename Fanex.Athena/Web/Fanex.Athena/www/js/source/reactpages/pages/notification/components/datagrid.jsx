import React, { Component } from 'react'
import { connect } from 'react-redux'
import { ActionCreators } from '../actions/index.js'
import { bindActionCreators } from 'redux'
import { Table, Row, Cell } from '../../../components/fixed-table-header'
import LangHelper from '../../../lib/langHelper'
import NumberFormat from '../../../lib/numberFormat'
import ReportTitle from '../../../components/ReportTitle'
import NoDataTitle from '../../../components/NoDataTitle'
import service from '../lib/service'

export class DataGrid extends Component {
    constructor(props) {
        super(props);

        this.reportTitle = "";
    }

    componentWillReceiveProps(nextProps) {
        if (this.props.gridData !== nextProps.gridData) {
            const { dateRange } = nextProps;
            this.reportTitle = service.getReportTitle(dateRange);
            this.props.toggleLoader(false);
        }
    }

    _buildHeader() {
        const headerText = service.getHeaderText();

        return (
            <Row style={{ height: 32 }}>
                <Cell colWidth={150} header={true} title="#">#</Cell>
                <Cell colWidth={150} header={true} title={headerText.type}>{headerText.type}</Cell>
                <Cell colWidth={200} header={true} title={headerText.dateTime}>{headerText.dateTime}</Cell>
                <Cell colWidth={150} header={true} title={headerText.content}>{headerText.content}</Cell>
            </Row>
        )
    }

    _buildBody() {
        const data = this.props.gridData.Data;
        let rows = [];
        for (let i = 0; i < data.length; i++) {
            const item = data[i];
            rows.push(
                <Row key={`tr${i}`}>
                    <Cell title={i+1} className="center-align">{i+1}</Cell>
                    <Cell title={item.Type}>{item.Type}</Cell>
                    <Cell title={item.DateTime} className="center-align">{item.DateTime}</Cell>
                    <Cell title={item.Content}>{item.Subject}</Cell>
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
        const { Data } = this.props.gridData;
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
                    minWidth={600}
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
    const { gridData, dateRange, selectedBaseCurrency, isLoading } = state;
    return Object.assign({}, { gridData, dateRange, selectedBaseCurrency, isLoading });
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);