import React, { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import { ActionCreators } from '../../actions/product'
import { Table, Row, Cell } from '../../../../components/fixed-table-header'

export class DataGrid extends Component {
    constructor(props) {
        super(props);
    }

    _buildHeader() {
        return (
            <Row style={{ height: 32 }}>
                <Cell colWidth={80} header={true}>#</Cell>
                <Cell colWidth={100} header={true}>Platform</Cell>
                <Cell colWidth={120} header={true}>External ID</Cell>
                <Cell colWidth={120} header={true}>System ID</Cell>
                <Cell colWidth={180} header={true}>Trans Time</Cell>
                <Cell colWidth={250} header={true}>Choice</Cell>
                <Cell colWidth={100} header={true}>Odds</Cell>
                <Cell colWidth={100} header={true}>Stake</Cell>
                <Cell colWidth={120} header={true}>WinLoss/ Comm</Cell>
                <Cell colWidth={120} header={true}>Licensee WL/ Comm</Cell>
                <Cell colWidth={100} header={true}>Status</Cell>
                <Cell colWidth={100} header={true}>Live</Cell>
            </Row>
        )
    }

    _buildBody() {
        let rows = [];

        for (let i = 0; i < 5; i++) {
            let content = i + 1;
            rows.push(
                <Row key={`casinodetailed-${i}`}>
                    <Cell>{content}</Cell>
                    <Cell>Platform 1</Cell>
                    <Cell>External ID 1</Cell>
                    <Cell>System ID 1</Cell>
                    <Cell>Trans Time 1</Cell>
                    <Cell>Choice 1</Cell>
                    <Cell>Odds 1</Cell>
                    <Cell>Stake 1</Cell>
                    <Cell>WinLoss/ Comm 1</Cell>
                    <Cell>Licensee WL/ Comm 1</Cell>
                    <Cell>Status 1</Cell>
                    <Cell>Live 1</Cell>
                </Row>
            );
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
        /* const { Data } = this.props.gridData;
        if (!Data) {
            return null;
        }

        if (!Data.length) {
            return this._renderNodata();
        } */

        return (
            <div className="result-content">
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
    return Object.assign({}, {});
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);