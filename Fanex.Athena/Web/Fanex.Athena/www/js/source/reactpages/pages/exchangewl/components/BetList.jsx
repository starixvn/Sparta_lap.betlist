import React, { Component } from 'react'
import { Table, Row, Cell } from '../../../components/fixed-table-header'

const FakeData = [
    {
        Market: "CRICKET > Ireland A vs Bangladesh A > Match Odds",
        WinLoss: 15,
        WlComm: -0.45,
        LicWl: -9,
        LicWlComm: 0.15,
        Details: [
            {
                BetId: 43154,
                Selection: "Bangladesh A",
                Type: "Back",
                BetPlaced: "10/25/2017 01:05:46",
                OddAvg: 1.5,
                Stake: 30,
                WinLoss: 15
            }
        ]
    },
    {
        Market: "CRICKET > Zimbabwe vs West Indies > Match Odds",
        WinLoss: 30,
        WlComm: 0,
        LicWl: 42,
        LicWlComm: 0,
        Details: [
            {
                BetId: 43116,
                Selection: "Zimbabwe",
                Type: "Lay",
                BetPlaced: "10/24/2017 05:45:14",
                OddAvg: 7,
                Stake: 30,
                WinLoss: 30
            },
            {
                BetId: 43112,
                Selection: "Zimbabwe",
                Type: "Back",
                BetPlaced: "10/24/2017 05:45:14",
                OddAvg: 9.8,
                Stake: 50,
                WinLoss: -50
            },
            {
                BetId: 43118,
                Selection: "Zimbabwe",
                Type: "Back",
                BetPlaced: "10/24/2017 05:45:14",
                OddAvg: 7.8,
                Stake: 50,
                WinLoss: -50
            }
        ]
    }
]

export default class BetList extends Component {
    constructor(props) {
        super(props);
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
                <Cell colWidth={100} header={true}>{"Lic WL /Comm"}</Cell>
            </Row>
        )
    }

    _buildBody() {
        const rows = [];
        for (let i = 0; i < FakeData.length; i++) {
            const dataItem = FakeData[i];
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
                        <Cell>{detailItem.Type}</Cell>
                        <Cell>{detailItem.BetPlaced}</Cell>
                        <Cell>{detailItem.OddAvg}</Cell>
                        <Cell>{detailItem.Stake}</Cell>
                        <Cell>{detailItem.WinLoss}</Cell>
                        {isFirstRow && <Cell rowSpan={dataDetailCount}>{dataItem.WinLoss}<br />{dataItem.WlComm}</Cell>}
                        {isFirstRow && <Cell rowSpan={dataDetailCount}>{dataItem.LicWl}<br />{dataItem.LicWlComm}</Cell>}
                    </Row>
                );

                isFirstRow = false;
            }
        }

        return rows;
    }

    _buildFooter() {
        return [
            <Row key="betlist-footer">
                <Cell colSpan='9'>Grand Total</Cell>
                <Cell>-55.45</Cell>
                <Cell>33.15</Cell>
            </Row>
        ]
    }

    render() {
        return (
            <div className="betlist-container">
                <div className="betlist-title">
                    <p>Customer Bet List 2/19/2018 - 2/19/2018 - testABC(12BETMR010101) - <span className="base-currency-title">View in MYR</span></p>
                </div>
                <div className="betlist-content">
                    <Table
                        autoWidth={false}
                        width={870}
                        header={this._buildHeader()}
                        body={this._buildBody()}
                        footer={this._buildFooter()}
                    />
                </div>
            </div>
        )
    }

}