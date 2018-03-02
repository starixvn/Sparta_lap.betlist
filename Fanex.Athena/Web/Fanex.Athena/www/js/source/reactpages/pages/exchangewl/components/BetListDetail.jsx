import React, { Component } from 'react'
import PropTypes from 'prop-types'

import NoDataTitle from '../../../components/NoDataTitle'
import { Table, Row, Cell } from '../../../components/fixed-table-header'
import LangHelper from '../../../lib/langHelper'
import NumberFormat from '../../../lib/numberFormat'

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
                <Cell colWidth={100} header={true}>{LangHelper.getSingleResource("BetTaken")}</Cell>
                <Cell colWidth={70} header={true}>{LangHelper.getSingleResource("OddsReq")}</Cell>
                <Cell colWidth={70} header={true}>{LangHelper.getSingleResource("Stake")}</Cell>
                <Cell colWidth={70} header={true}>{LangHelper.getSingleResource("Liability")}</Cell>
                <Cell colWidth={70} header={true}>{LangHelper.getSingleResource("OddsMatched")}</Cell>
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
                    <Cell className="center-align">{dataItem.BetTakenDateString}</Cell>
                    <Cell className="right-align">{NumberFormat.formatDecimal(dataItem.OddsReq)}</Cell>
                    <Cell className="right-align">{NumberFormat.formatDecimal(dataItem.Stake)}</Cell>
                    <Cell className="right-align">{dataItem.Liability}</Cell>
                    <Cell className="right-align">{NumberFormat.formatDecimal(dataItem.OddsMatched)}</Cell>
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