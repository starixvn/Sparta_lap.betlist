import React, { Component } from 'react'
import PropTypes from 'prop-types'

import BetListPopupLayout from "../../../layouts/BetListPopupLayout"
import NoDataTitle from '../../../components/NoDataTitle'
import { Table, Row, Cell } from '../../../components/fixed-table-header'
import Popup from "../../../components/popup"
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
        data: PropTypes.shape({
            mainData: PropTypes.arrayOf(PropTypes.object),
            grandTotal: PropTypes.number
        }).isRequired
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
        const { mainData } = this.props.data;
        const rows = [];
        for (let i = 0; i < mainData.length; i++) {
            const { MarketName, EventTypeName, EventName, WinLost, Commission, TicketList } = mainData[i];
            const dataDetailCount = TicketList.length;
            let isFirstRow = true;

            const marketText = (<p>{EventTypeName} > <span className="text-bold">{EventName}</span> > {MarketName}</p>);
            for (let j = 0; j < TicketList.length; j++) {
                const { BetId, Selection, Type, BetPlaced, MatchAvgOdds, MatchStake } = TicketList[j];
                rows.push(
                    <Row key={`row-${i}-${j}`}>
                        {isFirstRow && <Cell rowSpan={dataDetailCount} className="center-align">{i + 1}</Cell>}
                        {isFirstRow && <Cell rowSpan={dataDetailCount}>{marketText}</Cell>}
                        <Cell className="center-align"><a href="javascript:;" onClick={this._showTicketDetail}>{BetId}</a></Cell>
                        <Cell className="center-align">{Selection}</Cell>
                        <Cell className="center-align">{Type}</Cell>
                        <Cell className="center-align">{BetPlaced}</Cell>
                        <Cell className="right-align">{MatchAvgOdds}</Cell>
                        <Cell className="right-align">{MatchStake}</Cell>
                        <Cell className="right-align">{TicketList[j].WinLost}</Cell>
                        {
                            isFirstRow &&
                            <Cell rowSpan={dataDetailCount} className="right-align">
                                <span className={WinLost < 0 ? "negative" : null}>{WinLost}</span>
                                <br />
                                <span className={Commission < 0 ? "negative" : null}>{Commission}</span>
                            </Cell>}
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
        const { grandTotal } = this.props.data;

        return [
            <Row key="betlist-footer">
                <Cell colSpan='9' className="right-align">Grand Total</Cell>
                <Cell className={`right-align ${grandTotal < 0 ? "negative" : null}`}>{grandTotal}</Cell>
            </Row >
        ]
    }

    render() {
        const { showDetail, detailData } = this.state;
        const { mainData } = this.props.data;
        const title = (<p>Customer Bet List 2/19/2018 - 2/19/2018 - testABC(12BETMR010101) - <span className="base-currency-title">View in MYR</span></p>);

        return (
            <div>
                <BetListPopupLayout title={title}>
                    {
                        mainData && mainData.length ?
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