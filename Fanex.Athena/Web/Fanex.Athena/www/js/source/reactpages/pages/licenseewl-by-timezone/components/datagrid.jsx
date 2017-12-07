import moment from 'moment'
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
import { SettingConst } from '../constants/settingConst'

export class DataGrid extends Component {
    constructor(props) {
        super(props);

        this.reportTitle = "";
        this.groupBy = 0;
        this.selectedBaseCurrency = "";
        this.selectedReportBy = 0;
    }

    componentWillReceiveProps(nextProps) {
        if (this.props.gridData !== nextProps.gridData) {
            const { groupBy, selectedReportBy, selectedTimeZone, dateRange, selectedCurrency, selectedBaseCurrency } = nextProps;
            this.groupBy = groupBy;
            this.selectedReportBy = selectedReportBy;
            this.selectedBaseCurrency = selectedBaseCurrency;
            this.reportTitle = service.getReportTitle(selectedTimeZone, dateRange, selectedCurrency.label, selectedBaseCurrency);
            this.props.toggleLoader(false);
        }
    }

    _buildHeader() {
        const headerText = service.getHeaderText(this.selectedReportBy, this.groupBy);
        return (
            <Row style={{ height: 32 }}>
                <Cell colWidth={50} header={true} title="#">{headerText.Index}</Cell>
                { this.selectedReportBy === SettingConst.ReportBySummary ? null :
                    <Cell colWidth={100} header={true} title={headerText.ReportBy}>{headerText.ReportBy}</Cell> 
                }
                <Cell colWidth={150} header={true} title={headerText.GroupBy}>{headerText.GroupBy}</Cell>
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
                   
                    const groupByClassName = this.groupBy === SettingConst.GroupByProduct ? "left-align" : "center-align";
                    
                    let rows = [];
                    for (let i = 0; i < data.length; i++) {
                        const item = data[i];
                        const reportBy = moment(item.ReportBy).format(this.selectedReportBy === 2 ? "MM/DD/YYYY" : "MM/YYYY"),
                                betcount = NumberFormat.format(item.BetCount),
                                turnover = NumberFormat.formatDecimal(item.Turnover),
                                customerComm= NumberFormat.formatDecimal(item.CustomerComm),
                                licenseeWL = NumberFormat.formatDecimal(item.LicenseeWinLoss),
                                licenseeComm = NumberFormat.formatDecimal(item.LicenseeComm),
                                licenseeNetWL = NumberFormat.formatDecimal(item.LicenseeNetWinLoss),
                                margin = NumberFormat.formatDecimal(item.Margin);
                        
                        rows.push(
                            <Row key={`tr${i}`}>
                    <Cell className="center-align">{i + 1}</Cell>
                { this.selectedReportBy === SettingConst.ReportBySummary  ? null :
                        <Cell title={reportBy} className="center-align">{reportBy}</Cell>
                }
                    <Cell title={item.GroupBy} className={groupByClassName}>{item.GroupBy}</Cell>
                    <Cell title={betcount} className="right-align">{betcount}</Cell>
                    <Cell title={turnover} className="right-align">{turnover}</Cell>
                    <Cell title={customerComm} className="right-align">{customerComm}</Cell>
                    <Cell title={licenseeWL} className="right-align">{licenseeWL}</Cell>
                    <Cell title={licenseeComm} className="right-align">{licenseeComm}</Cell>
                    <Cell title={licenseeNetWL} className="right-align">{licenseeNetWL}</Cell>
                    <Cell title={margin} className="right-align">{margin}</Cell>
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
                    licenseeWL = NumberFormat.formatDecimal(item.LicenseeWinLoss),
                    licenseeComm = NumberFormat.formatDecimal(item.LicenseeComm),
                    licenseeNetWL = NumberFormat.formatDecimal(item.LicenseeNetWinLoss),
                    margin = NumberFormat.formatDecimal(item.Margin);
            const colSpan = this.selectedReportBy === SettingConst.ReportBySummary ? 2 : 3;
            return [(
                <Row className="fixed-table-grant-total" key={1}>
                   <Cell className="center-align" colSpan={colSpan}>{LangHelper.getSingleResource("Totalin") + " " + this.selectedBaseCurrency}</Cell>
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
    const { gridData, groupBy, selectedReportBy, selectedTimeZone, dateRange, selectedCurrency, selectedBaseCurrency, isLoading } = state;
    return Object.assign({}, { gridData, groupBy, selectedReportBy, selectedTimeZone, dateRange, selectedCurrency, selectedBaseCurrency, isLoading });
                };

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);