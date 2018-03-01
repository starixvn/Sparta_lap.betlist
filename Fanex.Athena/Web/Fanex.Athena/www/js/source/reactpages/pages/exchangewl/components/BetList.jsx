import React, { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'
import PropTypes from 'prop-types'

import BetListPopupLayout from "../../../layouts/BetListPopupLayout"
import NoDataTitle from '../../../components/NoDataTitle'
import { Table, Row, Cell } from '../../../components/fixed-table-header'
import Popup from '../../../components/popup'
import BetListDetail from './BetListDetail'
import Report from '../../../lib/report'
import { Service } from '../lib/service'
import { toggleLoader } from '../../../actions/loaderAction'

export class BetList extends Component {
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
        }).isRequired,
        title: PropTypes.any
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
                        <Cell className={`center-align ${Type.toLowerCase() === "lay" && "negative"}`}>{Type}</Cell>
                        <Cell className="center-align">{BetPlaced}</Cell>
                        <Cell className="right-align">{MatchAvgOdds}</Cell>
                        <Cell className="right-align">{MatchStake}</Cell>
                        <Cell className="right-align">{TicketList[j].WinLost}</Cell>
                        {
                            isFirstRow &&
                            <Cell rowSpan={dataDetailCount} className="right-align">
                                <span className={WinLost < 0 && "negative"}>{WinLost}</span>
                                <br />
                                <span className={Commission < 0 && "negative"}>{Commission}</span>
                            </Cell>}
                    </Row>
                );

                isFirstRow = false;
            }
        }

        return rows;
    }

    _showTicketDetail = () => {
        const url = "api/ExchangeWinLossBetList/Detail";
        const params = {
            xsrfToken: this.props.xsrfToken,
            data: {}
        }

        this.props.toggleLoader(true);
        Report.fetchGridData(url, params)
            .then(response => {
                this.setState({ showDetail: true, detailData: response.data || [] })
                this.props.toggleLoader(false);
            })
            .catch(error => {
                Service.showErrorMsg();
                this.props.toggleLoader(false);
            })
    }

    closeDetailPopup = () => {
        this.setState({ showDetail: false, detailData: null })
    }

    _buildFooter() {
        const { grandTotal } = this.props.data;

        return [
            <Row key="betlist-footer">
                <Cell colSpan='9' className="right-align">Grand Total</Cell>
                <Cell className={`right-align ${grandTotal < 0 && "negative"}`}>{grandTotal}</Cell>
            </Row >
        ]
    }

    render() {
        const { showDetail, detailData } = this.state;
        const { mainData } = this.props.data;

        return (
            <div>
                <BetListPopupLayout title={this.props.title}>
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
                    showDetail && detailData &&
                    <Popup isShow={showDetail} title={"DETAIL"} width={730} height={450} onClose={this.closeDetailPopup}>
                        <BetListDetail data={detailData} />
                    </Popup>
                }
            </div>
        )
    }
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators({ toggleLoader }, dispatch);
};

function mapStateToProps(state) {
    const { params, xsrfToken } = state;
    return Object.assign({}, { params, xsrfToken });
};

export default connect(mapStateToProps, mapDispatchToProps)(BetList);