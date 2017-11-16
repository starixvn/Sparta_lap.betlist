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
                <Cell colWidth={150} header={true} title={headerText.Product}>{headerText.Product}</Cell>
                <Cell colWidth={150} header={true} title={headerText.Currency}>{headerText.Currency}</Cell>
                <Cell colWidth={150} header={true} title={headerText.BetCount}>{headerText.BetCount}</Cell>
                <Cell colWidth={200} header={true} title={headerText.Turnover}>{headerText.Turnover}</Cell>
                <Cell colWidth={200} header={true} title={headerText.CustomerComm}>{headerText.CustomerComm}</Cell>
                <Cell colWidth={200} header={true} title={headerText.LicenseeWL}>{headerText.LicenseeWL}</Cell>
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
                    customerWinLoss = NumberFormat.formatDecimal(item.CustomerWinLoss),
                    subsidiaryWinLoss = NumberFormat.formatDecimal(item.SubsidiaryWinLoss);

                rows.push(
                    <Row key={`tr${i}`}>
                        <Cell title={item.Subsidiary}>{item.Subsidiary}</Cell>
                        <Cell title={item.CurrencyName} className="center-align">{item.CurrencyName}</Cell>
                        <Cell title={item.Product}>{item.Product}</Cell>
                        <Cell title={betcount} className="right-align">{betcount}</Cell>
                        <Cell title={turnover} className="right-align">{turnover}</Cell>
                        <Cell title={customerWinLoss} className="right-align">{customerWinLoss}</Cell>
                        <Cell title={subsidiaryWinLoss} className={`right-align ${item.SubsidiaryWinLoss < 0 ? "negative" : ""}`}>
                            {subsidiaryWinLoss}
                        </Cell>
                    </Row>
                );
        }

        return rows;
    }

    _buildFooter() {
        const item = this.props.gridData.Total;
        const betcount = NumberFormat.format(item.BetCount),
                turnover = NumberFormat.formatDecimal(item.Turnover),
                customerWinLoss = NumberFormat.formatDecimal(item.CustomerWinLoss),
                subsidiaryWinLoss = NumberFormat.formatDecimal(item.SubsidiaryWinLoss);

            return (
                <Row className="fixed-table-total">
                    <Cell title={item.Subsidiary}>{item.Subsidiary}</Cell>
                    <Cell title={item.CurrencyName}>{item.CurrencyName}</Cell>
                    <Cell></Cell>
                    <Cell title={betcount} className="right-align">{betcount}</Cell>
                    <Cell title={turnover} className="right-align">{turnover}</Cell>
                    <Cell title={customerWinLoss} className="right-align">{customerWinLoss}</Cell>
                    <Cell title={subsidiaryWinLoss} className={`right-align ${item.SubsidiaryWinLoss < 0 ? "negative" : ""}`}>
                        {subsidiaryWinLoss}
                    </Cell>
                </Row>
            )

        return footer;
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