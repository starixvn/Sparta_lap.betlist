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
import LangHelper from '../../../lib/langHelper'
import NumberFormat from '../../../lib/numberFormat'

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
            MainData: PropTypes.arrayOf(PropTypes.object),
            GrandTotal: PropTypes.number
        }),
        title: PropTypes.any,
        params: PropTypes.object.isRequired
    }

    _buildHeader() {
        return (
            <Row style={{ height: 32 }}>
                <Cell colWidth={40} header={true}>{"#"}</Cell>
                <Cell colWidth={300} header={true}>{LangHelper.getSingleResource("Market")}</Cell>
                <Cell colWidth={70} header={true}>{LangHelper.getSingleResource("BetId")}</Cell>
                <Cell colWidth={120} header={true}>{LangHelper.getSingleResource("Selection")}</Cell>
                <Cell colWidth={50} header={true}>{LangHelper.getSingleResource("Type")}</Cell>
                <Cell colWidth={100} header={true}>{LangHelper.getSingleResource("BetPlaced")}</Cell>
                <Cell colWidth={60} header={true}>{LangHelper.getSingleResource("OddsAvg")}</Cell>
                <Cell colWidth={80} header={true}>{LangHelper.getSingleResource("Stake")}</Cell>
                <Cell colWidth={80} header={true}>{LangHelper.getSingleResource("WinLoss")}</Cell>
                <Cell colWidth={100} header={true}>{LangHelper.getSingleResource("WL")}/{LangHelper.getSingleResource("Comm")}</Cell>
            </Row>
        )
    }

    _buildBody() {
        const { MainData } = this.props.data;
        const rows = [];
        for (let i = 0; i < MainData.length; i++) {
            const { Ticket, Details } = MainData[i];
            const { MarketName, EventTypeName, EventName, WinLost, Commission } = Ticket;
            const dataDetailCount = Details.length;
            const WinLostSumString = NumberFormat.formatDecimal(WinLost);
            const CommissionString = NumberFormat.formatDecimal(Commission);
            let isFirstRow = true;

            const marketText = (<p>{EventTypeName} > <span className="text-bold">{EventName}</span> > {MarketName}</p>);
            for (let j = 0; j < Details.length; j++) {
                const { BetId, SelectionName, Type, BetPlacedDateString, OddsAvg, Stake } = Details[j];
                const OddsAvgString = NumberFormat.formatDecimal(OddsAvg);
                const StakeString = NumberFormat.formatDecimal(Stake);
                const WinLostDetailString = NumberFormat.formatDecimal(Details[j].WinLost);                

                rows.push(
                    <Row key={`row-${i}-${j}`}>
                        {isFirstRow && <Cell title={i + 1} rowSpan={dataDetailCount} className="center-align">{i + 1}</Cell>}
                        {isFirstRow && <Cell title={`${EventTypeName} > ${EventName} > ${MarketName}`} rowSpan={dataDetailCount}>{marketText}</Cell>}
                        <Cell title={BetId} className="center-align">
                            <a href="javascript:;" onClick={() => this._showTicketDetail(BetId, Type)}>{BetId}</a>
                        </Cell>
                        <Cell title={SelectionName} className="center-align">{SelectionName}</Cell>
                        <Cell title={Type} className={`center-align ${Type.toLowerCase() === "lay" && "negative"}`}>{Type}</Cell>
                        <Cell title={BetPlacedDateString} className="center-align">{BetPlacedDateString}</Cell>
                        <Cell title={OddsAvgString} className="right-align">{OddsAvgString}</Cell>
                        <Cell title={StakeString} className="right-align">{NumberFormat.formatDecimal(Stake)}</Cell>
                        <Cell title={WinLostDetailString} className="right-align">{WinLostDetailString}</Cell>
                        {
                            isFirstRow &&
                            <Cell title={`${WinLostSumString}/${CommissionString}`} rowSpan={dataDetailCount} className="right-align">
                                <span className={WinLost < 0 && "negative"}>{WinLostSumString}</span>
                                <br />
                                <span className={Commission < 0 && "negative"}>{CommissionString}</span>
                            </Cell>}
                    </Row>
                );

                isFirstRow = false;
            }
        }

        return rows;
    }

    _showTicketDetail = (betId, type) => {
        const { CustId, CurrencyId, BaseCurrency, ExcludeTestData } = this.props.params;
        const url = "api/ExchangeWinLossBetList/Detail";
        const params = {
            xsrfToken: this.props.xsrfToken,
            data: { CustId, CurrencyId, BaseCurrency, ExcludeTestData, betId, type }
        }

        this.props.toggleLoader(true);
        Report.fetchGridData(url, params)
            .then(response => {
                this.setState({ showDetail: true, detailData: response.data })
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
        const GrandTotalString = NumberFormat.formatDecimal(this.props.data.GrandTotal);

        return [
            <Row key="betlist-footer">
                <Cell colSpan='9' className="right-align">{LangHelper.getSingleResource("GrandTotal")}</Cell>
                <Cell title={GrandTotalString} className={`right-align ${this.props.data.GrandTotal < 0 && "negative"}`}>{GrandTotalString}</Cell>
            </Row >
        ]
    }

    render() {
        const { showDetail, detailData } = this.state;
        const { MainData } = this.props.data;

        return (
            <div>
                <BetListPopupLayout title={this.props.title}>
                    {
                        MainData && MainData.length ?
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
                    showDetail &&
                    <Popup isShow={showDetail} title={LangHelper.getSingleResource("Details")} width={730} height={450} onClose={this.closeDetailPopup}>
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
    const { xsrfToken } = state;
    return Object.assign({}, { xsrfToken });
};

export default connect(mapStateToProps, mapDispatchToProps)(BetList);