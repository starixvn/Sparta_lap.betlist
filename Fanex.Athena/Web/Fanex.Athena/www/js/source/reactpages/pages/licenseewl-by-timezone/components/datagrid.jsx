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
            const { dateRange, selectedBaseCurrency } = nextProps;
            this.reportTitle = service.getReportTitle(dateRange, selectedBaseCurrency);
            this.props.toggleLoader(false);
        }
    }

    _buildHeader() {
        const headerText = service.getHeaderText();

        return (
            <Row style={{ height: 32 }}>
                <Cell colWidth={50} header={true} title="#">{headerText.Index}</Cell>
                <Cell colWidth={150} header={true} title={headerText.Product}>{headerText.Product}</Cell>
                <Cell colWidth={150} header={true} title={headerText.Currency}>{headerText.Currency}</Cell>
                <Cell colWidth={150} header={true} title={headerText.BetCount}>{headerText.BetCount}</Cell>
                <Cell colWidth={200} header={true} title={headerText.Turnover}>{headerText.Turnover}</Cell>
                <Cell colWidth={200} header={true} title={headerText.CustomerComm}>{headerText.CustomerComm}</Cell>
                <Cell colWidth={200} header={true} title={headerText.LicenseeWL}>{headerText.LicenseeWL}</Cell>
                <Cell colWidth={200} header={true} title={headerText.LicenseeComm}>{headerText.LicenseeComm}</Cell>
                <Cell colWidth={200} header={true} title={headerText.LicenseeNetWL}>{headerText.LicenseeNetWL}</Cell>
                <Cell colWidth={200} header={true} title={headerText.Margin}>{headerText.Margin}</Cell>
            </Row>
        )
    }

    _buildBody() {
                    const data = this.props.gridData.Data;
                    let rows = [];
                    for (let i = 0; i < data.length; i++) {
                        const item = data[i];
                        const betcount = NumberFormat.format(item.BetCount),
                                turnover = NumberFormat.formatDecimal(item.Turnover),
                                customerComm= NumberFormat.formatDecimal(item.CustomerComm),
                                licenseeWL = NumberFormat.formatDecimal(item.LicenseeWL),
                                licenseeComm = NumberFormat.formatDecimal(item.LicenseeComm),
                                licenseeNetWL = NumberFormat.formatDecimal(item.LicenseeNetWL),
                                margin = NumberFormat.formatDecimal(item.Margin);

                        rows.push(
                            <Row key={`tr${i}`}>
                        <Cell className="center-align">{i + 1}</Cell>
                        <Cell title={item.Product}>{item.Product}</Cell>
                        <Cell title={item.CurrencyName} className="center-align">{item.CurrencyName}</Cell>
                        <Cell title={betcount} className="right-align">{betcount}</Cell>
                        <Cell title={turnover} className="right-align">{turnover}</Cell>
                        <Cell title={customerComm} className="right-align">{customerComm}</Cell>
                        <Cell title={licenseeWL} className={"right-align"}>{licenseeWL}</Cell>
                        <Cell title={licenseeComm} className={"right-align"}>{licenseeComm}</Cell>
                        <Cell title={licenseeNetWL} className={"right-align"}>{licenseeNetWL}</Cell>
                        <Cell title={margin} className={"right-align"}>{margin}</Cell>
                    </Row>
                );
                }

        return rows;
                }

    _buildFooter() {
        const item = this.props.gridData.Total;
        const betcount = NumberFormat.format(item.BetCount),
                    turnover = NumberFormat.formatDecimal(item.Turnover),
                    customerComm= NumberFormat.formatDecimal(item.CustomerComm),
                    licenseeWL = NumberFormat.formatDecimal(item.LicenseeWL),
                    licenseeComm = NumberFormat.formatDecimal(item.LicenseeComm),
                    licenseeNetWL = NumberFormat.formatDecimal(item.LicenseeNetWL),
                    margin = NumberFormat.formatDecimal(item.Margin);

            return [(
                <Row className="fixed-table-grant-total" key={1}>
                   <Cell className="center-align" colSpan={3}>{LangHelper.getSingleResource("Totalin") + "EUR"}</Cell>
                   <Cell title={betcount} className="right-align">{betcount}</Cell>
                   <Cell title={turnover} className="right-align">{turnover}</Cell>
                   <Cell title={customerComm} className="right-align">{customerComm}</Cell>
                   <Cell title={licenseeWL} className={"right-align"}>{licenseeWL}</Cell>
                   <Cell title={licenseeComm} className={"right-align"}>{licenseeComm}</Cell>
                   <Cell title={licenseeNetWL} className={"right-align"}>{licenseeNetWL}</Cell>
                   <Cell title={margin} className={"right-align"}>{margin}</Cell>
                </Row>
            )]
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

        if (!Data.length) {
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
                    footer={this._buildFooter()}
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