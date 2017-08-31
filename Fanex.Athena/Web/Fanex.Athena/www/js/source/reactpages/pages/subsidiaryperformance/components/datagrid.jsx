import React, { Component } from 'react';
import { connect } from 'react-redux';
import { ActionCreators } from '../actions/index.js';
import { bindActionCreators } from 'redux';
import FixedTable from 'components/fixed-table-header/FixedTable';

const columnLayout = [300, 150, 150, 150, 200, 200, 200];
const { Table, Header, Body, Footer, Row, Cell } = FixedTable;

class DataGrid extends Component {
    constructor(props) {
        super(props);
    }

    get header() {
        return (
            <Row style={{height: 40}}>
                <th>Subsidiary</th>
                <th>Currency</th>
                <th>Products</th>
                <th>Bet Count</th>
                <th>Turnover</th>
                <th>Customer Win Loss</th>
                <th>Subsidiary WL/Comm</th>
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
                rows.push(
                    <Row key={`tr${i}${j}`}>
                        {isFirstRow ? <Cell rowSpan={rowSpan}>{item.Subsidiary}</Cell> : undefined}
                        {isFirstRow ? <Cell rowSpan={rowSpan} className="center-align">{item.Currency}</Cell> : undefined}
                        <Cell className="center-align">{detail.Product}</Cell>
                        <Cell className="right-align">{detail.BetCount}</Cell>
                        <Cell className="right-align">{detail.Turnover}</Cell>
                        <Cell className="right-align">{detail.CustomerWinLoss}</Cell>
                        <Cell className="right-align">{detail.SubsidiaryWinLoss}</Cell>
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
            return (
                <Row className="fixed-table-total" key={`total${index}`}>
                    <Cell>{`Total ${item.Subsidiary}`}</Cell>
                    <Cell>{`Based ${item.Currency}`}</Cell>
                    <Cell></Cell>
                    <Cell className="right-align">{item.BetCount}</Cell>
                    <Cell className="right-align">{item.Turnover}</Cell>
                    <Cell className="right-align">{item.CustomerWinLoss}</Cell>
                    <Cell className="right-align">{item.SubsidiaryWinLoss}</Cell>
                </Row>
            )
        })
    }

    get grantTotal() {
        const grandTotal = this.props.gridData.GrandTotal;
        return (
            <Row className="fixed-table-grant-total">
                <Cell>Total</Cell>
                <Cell>{`Based ${grandTotal.Currency}`}</Cell>
                <Cell></Cell>
                <Cell className="right-align">{grandTotal.BetCount}</Cell>
                <Cell className="right-align">{grandTotal.Turnover}</Cell>
                <Cell className="right-align">{grandTotal.CustomerWinLoss}</Cell>
                <Cell className="right-align">{grandTotal.SubsidiaryWinLoss}</Cell>
            </Row>
        )
    }

    render() {
        if (!this.props.gridData.Data || !this.props.gridData.Data.length) {
            return null;
        }

        return (
            <Table
                width={1300}
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