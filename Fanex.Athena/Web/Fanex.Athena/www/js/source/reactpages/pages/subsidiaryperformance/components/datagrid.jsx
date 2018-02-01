import React, { Component } from 'react'
import { connect } from 'react-redux'
import { ActionCreators } from '../actions/index.js'
import { bindActionCreators } from 'redux'
import { Table, Row, Cell } from '../../../components/fixed-table-header'
import LangHelper from '../../../lib/langHelper'
import NumberFormat from '../../../lib/numberFormat'
import NoDataTitle from '../../../components/NoDataTitle'
import service from '../lib/service'

export class DataGrid extends Component {
    constructor(props) {
        super(props);
    }

    componentWillReceiveProps(nextProps) {
        if (this.props.gridData !== nextProps.gridData) {
            this.props.toggleLoader(false);
        }
    }

    _buildHeader() {
        const headerText = service.getHeaderText();

        return (
            <Row style={{ height: 32 }}>
                <Cell colWidth={150} header={true} title={headerText.subsidiary}>{headerText.subsidiary}</Cell>
                <Cell colWidth={150} header={true} title={headerText.currency}>{headerText.currency}</Cell>
                <Cell colWidth={200} header={true} title={headerText.product}>{headerText.product}</Cell>
                <Cell colWidth={150} header={true} title={headerText.betCount}>{headerText.betCount}</Cell>
                <Cell colWidth={200} header={true} title={headerText.turnover}>{headerText.turnover}</Cell>
                <Cell colWidth={200} header={true} title={headerText.customerWL}>{headerText.customerWL}</Cell>
                <Cell colWidth={200} header={true} title={headerText.subsidiaryWL}>{headerText.subsidiaryWL}</Cell>
                <Cell colWidth={150} header={true} title={headerText.subsidiaryComm}>{headerText.subsidiaryComm}</Cell>
            </Row>
        )
    }

    _buildBody() {
        const data = this.props.gridData.Data;
        let rows = [];
        for (let i = 0; i < data.length; i++) {
            const item = data[i];
            const rowSpan = item.Details.length;

            let isFirstRow = true;
            for (let j = 0; j < item.Details.length; j++) {
                const detail = item.Details[j];
                const betcount = NumberFormat.format(detail.BetCount),
                    turnover = NumberFormat.formatDecimal(detail.Turnover),
                    customerWinLoss = NumberFormat.formatDecimal(detail.CustomerWinLoss),
                    subsidiaryWinLoss = NumberFormat.formatDecimal(detail.SubsidiaryWinLoss),
                    subsidiaryComm = NumberFormat.formatDecimal(detail.SubsidiaryComm);

                rows.push(
                    <Row key={`tr${i}-detail${j}`}>
                        {isFirstRow ? <Cell title={item.Subsidiary} rowSpan={rowSpan}>{item.Subsidiary}</Cell> : undefined}
                        {isFirstRow ? <Cell title={item.CurrencyName} rowSpan={rowSpan} className="center-align">{item.CurrencyName}</Cell> : undefined}
                        <Cell title={detail.Product}>{detail.Product}</Cell>
                        <Cell title={betcount} className="right-align">{betcount}</Cell>
                        <Cell title={turnover} className="right-align">{turnover}</Cell>
                        <Cell title={customerWinLoss} className="right-align">{customerWinLoss}</Cell>
                        <Cell title={subsidiaryWinLoss} className={`right-align ${detail.SubsidiaryWinLoss < 0 ? "negative" : ""}`}>
                            {subsidiaryWinLoss}
                        </Cell>
                        <Cell title={subsidiaryComm} className="right-align">{subsidiaryComm}</Cell>
                    </Row>
                );

                isFirstRow = false;
            }
        }

        return rows;
    }

    _buildFooter() {
        const totalData = this.props.gridData.Total;
        const footer = totalData.map((item, index) => {
            const betcount = NumberFormat.format(item.BetCount),
                turnover = NumberFormat.formatDecimal(item.Turnover),
                customerWinLoss = NumberFormat.formatDecimal(item.CustomerWinLoss),
                subsidiaryWinLoss = NumberFormat.formatDecimal(item.SubsidiaryWinLoss),
                subsidiaryComm = NumberFormat.formatDecimal(item.SubsidiaryComm);

            return (
                <Row className="fixed-table-total" key={`total${index}`}>
                    <Cell title={item.Subsidiary}>{item.Subsidiary}</Cell>
                    <Cell title={item.CurrencyName} className="center-align" colSpan="2">{item.CurrencyName}</Cell>
                    <Cell title={betcount} className="right-align">{betcount}</Cell>
                    <Cell title={turnover} className="right-align">{turnover}</Cell>
                    <Cell title={customerWinLoss} className="right-align">{customerWinLoss}</Cell>
                    <Cell title={subsidiaryWinLoss} className={`right-align ${item.SubsidiaryWinLoss < 0 ? "negative" : ""}`}>
                        {subsidiaryWinLoss}
                    </Cell>
                    <Cell title={subsidiaryComm} className="right-align">{subsidiaryComm}</Cell>
                </Row>
            )
        })

        footer.push(this._buildGrandTotal());

        return footer;
    }

    _buildGrandTotal() {
        const grandTotal = this.props.gridData.GrandTotal;
        if (!grandTotal) {
            return null;
        }

        const betcount = NumberFormat.format(grandTotal.BetCount),
            turnover = NumberFormat.formatDecimal(grandTotal.Turnover),
            customerWinLoss = NumberFormat.formatDecimal(grandTotal.CustomerWinLoss),
            subsidiaryWinLoss = NumberFormat.formatDecimal(grandTotal.SubsidiaryWinLoss),
            subsidiaryComm = NumberFormat.formatDecimal(grandTotal.SubsidiaryComm);

        return (
            <Row key="grandTotal" className="fixed-table-grant-total">
                <Cell title={grandTotal.Subsidiary}>{grandTotal.Subsidiary}</Cell>
                <Cell title={grandTotal.CurrencyName} className="center-align" colSpan="2">{grandTotal.CurrencyName}</Cell>
                <Cell title={betcount} className="right-align">{betcount}</Cell>
                <Cell title={turnover} className="right-align">{turnover}</Cell>
                <Cell title={customerWinLoss} className="right-align">{customerWinLoss}</Cell>
                <Cell title={subsidiaryWinLoss} className={`right-align ${grandTotal.SubsidiaryWinLoss < 0 ? "negative" : ""}`}>{subsidiaryWinLoss}</Cell>
                <Cell title={subsidiaryComm} className="right-align">{subsidiaryComm}</Cell>
            </Row>
        )
    }

    render() {
        const { Data } = this.props.gridData;
        if (!Data) {
            return null;
        }

        if (!Data.length) {
            return (<NoDataTitle />);
        }

        return (
            <Table
                minWidth={600}
                autoWidth={true}
                bodyHeight={300}
                header={this._buildHeader()}
                body={this._buildBody()}
                footer={this._buildFooter()}
            />
        );
    }
}
function mapDispatchToProps(dispatch) {
    const { toggleLoader } = ActionCreators;
    return bindActionCreators({ toggleLoader }, dispatch);
}
function mapStateToProps(state) {
    const { gridData } = state;
    return { gridData };
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);