import React, { Component } from 'react'
import PropTypes from 'prop-types'

import NoDataTitle from '../../../components/NoDataTitle'
import { Table, Row, Cell } from '../../../components/fixed-table-header'

export default class BetListDetail extends Component {
    constructor(props) {
        super(props);
    }

    static propTypes = {
        data: PropTypes.arrayOf(PropTypes.object).isRequired
    }

    _buildHeader() {
        return (
            <Row style={{ height: 32 }}>
                <Cell colWidth={100} header={true}>{"Bet taken"}</Cell>
                <Cell colWidth={70} header={true}>{"Odds req."}</Cell>
                <Cell colWidth={70} header={true}>{"Stake"}</Cell>
                <Cell colWidth={70} header={true}>{"Liability"}</Cell>
                <Cell colWidth={70} header={true}>{"Odds matched"}</Cell>
            </Row>
        )
    }

    _buildBody() {
        const { data } = this.props;
        const rows = [];
        for (let i = 0; i < data.length; i++) {
            const dataItem = data[i];

            rows.push(
                <Row key={`rowDetail-${i}`}>
                    <Cell>{dataItem.BetTaken}</Cell>
                    <Cell>{dataItem.OddsReq}</Cell>
                    <Cell>{dataItem.Stake}</Cell>
                    <Cell>{dataItem.Liability}</Cell>
                    <Cell>{dataItem.OddsMacthed}</Cell>
                </Row>
            );
        }

        return rows;
    }

    render() {
        const { data } = this.props;

        return data && data.length ?
            (
                <div className="popup-content-default popup-detail-default">
                    <Table
                    autoWidth={false}
                    autoHeight={false}
                    width={700}
                    bodyHeight={350}
                    header={this._buildHeader()}
                    body={this._buildBody()}
                />
                </div>) :
            <NoDataTitle />
    }
}