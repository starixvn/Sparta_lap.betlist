import React, { Component } from 'react'
import PropTypes from 'prop-types'

import BetListPopupLayout from "../../../layouts/BetListPopupLayout"
import NoDataTitle from '../../../components/NoDataTitle'
import { Table, Row, Cell } from '../../../components/fixed-table-header'
import Popup from "../../../components/popup/Popup"
import BetListDetail from './BetListDetail'

const FakeDetailData = [
    {
        BetTaken: "10/23/2017 10:55:15",
        OddsReq: 6.80,
        Stake: 50.00,
        Liability: "-",
        OddsMacthed: 7.80
    }
]

export default class BetList extends Component {
    constructor(props) {
        super(props);

        this.state = {
            showDetail: false,
            detailData: null
        }
    }

    static propTypes = {
        data: PropTypes.arrayOf(PropTypes.object).isRequired
    }

    _buildHeader() {
        return (
            <Row style={{ height: 32 }}>
                <Cell colWidth={40} header={true}>{"#"}</Cell>
                <Cell colWidth={300} header={true}>{"Market"}</Cell>
                <Cell colWidth={70} header={true}>{"Bet ID"}</Cell>
                <Cell colWidth={120} header={true}>{"Selection"}</Cell>
                <Cell colWidth={50} header={true}>{"Type"}</Cell>
                <Cell colWidth={100} header={true}>{"Bet Placed"}</Cell>
                <Cell colWidth={60} header={true}>{"Odds avg."}</Cell>
                <Cell colWidth={80} header={true}>{"Stake"}</Cell>
                <Cell colWidth={80} header={true}>{"Winloss"}</Cell>
                <Cell colWidth={100} header={true}>{"WL /Comm"}</Cell>
            </Row>
        )
    }

    _buildBody() {
        const { data } = this.props;
        const rows = [];
        for (let i = 0; i < data.length; i++) {
            const dataItem = data[i];
            const dataDetails = dataItem.Details;
            const dataDetailCount = dataDetails.length;
            let isFirstRow = true;

            for (let j = 0; j < dataDetails.length; j++) {
                const detailItem = dataDetails[j];

                rows.push(
                    <Row key={`row-${i}-${j}`}>
                        {isFirstRow && <Cell rowSpan={dataDetailCount}>{i + 1}</Cell>}
                        {isFirstRow && <Cell rowSpan={dataDetailCount}>{dataItem.Market}</Cell>}
                        <Cell>{detailItem.BetId}</Cell>
                        <Cell>{detailItem.Selection}</Cell>
                        <Cell><a href="javascript:;" onClick={this._showTicketDetail}>{detailItem.Type}</a></Cell>
                        <Cell>{detailItem.BetPlaced}</Cell>
                        <Cell>{detailItem.OddAvg}</Cell>
                        <Cell>{detailItem.Stake}</Cell>
                        <Cell>{detailItem.WinLoss}</Cell>
                        {isFirstRow && <Cell rowSpan={dataDetailCount}>{dataItem.WinLoss}<br />{dataItem.WlComm}</Cell>}
                    </Row>
                );

                isFirstRow = false;
            }
        }

        return rows;
    }

    _showTicketDetail = () => {
        this.setState({ showDetail: true, detailData: FakeDetailData })
    }

    closeDetailPopup = () => {
        this.setState({ showDetail: false, detailData: [] })
    }

    _buildFooter() {
        return [
            <Row key="betlist-footer">
                <Cell colSpan='8'>Grand Total</Cell>
                <Cell>-55.45</Cell>
                <Cell>33.15</Cell>
            </Row>
        ]
    }

    render() {
        const { showDetail, detailData } = this.state;
        const { data } = this.props;
        const title = (<p>Customer Bet List 2/19/2018 - 2/19/2018 - testABC(12BETMR010101) - <span className="base-currency-title">View in MYR</span></p>);

        return (
            <div>
                <BetListPopupLayout title={title}>
                    {
                        data && data.length ?
                            (<Table
                                autoWidth={false}
                                autoHeight={false}
                                width={870}
                                bodyHeight={430}
                                header={this._buildHeader()}
                                body={this._buildBody()}
                                footer={this._buildFooter()}
                            />) :
                            <NoDataTitle />
                    }
                </BetListPopupLayout>
                {
                    showDetail && detailData && detailData.length &&
                    <Popup isShow={showDetail} title={"DETAIL"} width={730} height={450} onClose={this.closeDetailPopup}>
                        <BetListDetail data={detailData} />
                    </Popup>
                }
            </div>
        )
    }

}