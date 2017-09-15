import React, { Component } from 'react'
import { connect } from 'react-redux'
import { ActionCreators } from '../actions/index.js'
import { bindActionCreators } from 'redux'
import { Table, Header, Body, Footer, Row, Cell } from 'lrfcomponents/fixed-table-header/FixedTable'
import LangHelper from 'lrflib/langHelper'
import NumberFormat from 'lrflib/numberFormat'

const columnLayout = [150, 100, 200, 150, 200, 200, 200];

class DataGrid extends Component {
    constructor(props) {
        super(props);
    }

    get header() {
        const subsidiary = LangHelper.getSingleResource("Subsidiary"),
            currency = LangHelper.getSingleResource("Currency"),
            product = LangHelper.getSingleResource("Product"),
            betcount = LangHelper.getSingleResource("BetCount"),
            turnover = LangHelper.getSingleResource("Turnover"),
            customerWL = LangHelper.getSingleResource("CustomerWinLoss"),
            subsidiaryWL = LangHelper.getSingleResource("SubsidiaryWL"),
            comm = LangHelper.getSingleResource("Comm");

        return (
            <Row style={{ height: 40 }}>
                <th title={subsidiary}>{subsidiary}</th>
                <th title={currency}>{currency}</th>
                <th title={product}>{product}</th>
                <th title={betcount}>{betcount}</th>
                <th title={turnover}>{turnover}</th>
                <th title={customerWL}>{customerWL}</th>
                <th title={`${subsidiaryWL}/ ${comm}`}>{`${subsidiaryWL}/ ${comm}`}</th>
            </Row>
        )
    }

    get body() {
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
                    subsidiaryWinLoss = NumberFormat.formatDecimal(detail.SubsidiaryWinLoss);

                rows.push(
                    <Row key={`tr${i}${j}`}>
                        {isFirstRow ? <Cell title={item.Subsidiary} rowSpan={rowSpan}>{item.Subsidiary}</Cell> : undefined}
                        {isFirstRow ? <Cell title={item.CurrencyName} rowSpan={rowSpan} className="center-align">{item.CurrencyName}</Cell> : undefined}
                        <Cell title={detail.Product}>{detail.Product}</Cell>
                        <Cell title={betcount} className="right-align">{betcount}</Cell>
                        <Cell title={turnover} className="right-align">{turnover}</Cell>
                        <Cell title={customerWinLoss} className="right-align">{customerWinLoss}</Cell>
                        <Cell title={subsidiaryWinLoss} className={`right-align ${detail.SubsidiaryWinLoss < 0 ? "negative" : ""}`}>
                            {subsidiaryWinLoss}
                        </Cell>
                    </Row>
                );

                isFirstRow = false;
            }
        }

        return rows;
    }

    get footer() {
        const totalData = this.props.gridData.Total;
        return totalData.map((item, index) => {
            const betcount = NumberFormat.format(item.BetCount),
                turnover = NumberFormat.formatDecimal(item.Turnover),
                customerWinLoss = NumberFormat.formatDecimal(item.CustomerWinLoss),
                subsidiaryWinLoss = NumberFormat.formatDecimal(item.SubsidiaryWinLoss);

            return (
                <Row className="fixed-table-total" key={`total${index}`}>
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
        })
    }

    get grantTotal() {
        const grandTotal = this.props.gridData.GrandTotal;
        if (!grandTotal) return null;

        const betcount = NumberFormat.format(grandTotal.BetCount),
            turnover = NumberFormat.formatDecimal(grandTotal.Turnover),
            customerWinLoss = NumberFormat.formatDecimal(grandTotal.CustomerWinLoss),
            subsidiaryWinLoss = NumberFormat.formatDecimal(grandTotal.SubsidiaryWinLoss);

        return (
            <Row className="fixed-table-grant-total">
                <Cell title={grandTotal.Subsidiary}>{grandTotal.Subsidiary}</Cell>
                <Cell title={grandTotal.CurrencyName}>{grandTotal.CurrencyName}</Cell>
                <Cell></Cell>
                <Cell title={betcount} className="right-align">{betcount}</Cell>
                <Cell title={turnover} className="right-align">{turnover}</Cell>
                <Cell title={customerWinLoss} className="right-align">{customerWinLoss}</Cell>
                <Cell title={subsidiaryWinLoss} className="right-align">{subsidiaryWinLoss}</Cell>
            </Row>
        )
    }

    componentDidUpdate(prevProps, prevState) {
        this.props.toggleLoader(false);
    }

    render() {
        if (!this.props.gridData.Data || !this.props.gridData.Data.length) {
            return null;
        }

        return (
            <Table
                maxWidth={this.props.containerWidth}
                autoWidth={true}
                columnLayout={columnLayout}
                {...this.props}>

                <Header>
                    {this.header}
                </Header>

                <Body maxHeight={300}>
                    {this.body}
                </Body>

                <Footer>
                    {this.footer}
                    {this.grantTotal}
                </Footer>
            </Table>
        );
    }
}
function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
}
function mapStateToProps(state) {
    const { gridData, containerWidth } = state;
    return Object.assign({}, {
        gridData,
        containerWidth
    });
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);