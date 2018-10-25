import moment from 'moment'
import React, { Component } from 'react'
import { connect } from 'react-redux'
import { ActionCreators } from '../actions/index.js'
import { bindActionCreators } from 'redux'
import { Table, Row, Cell } from '@jsroot/components/fixed-table-header'
import LangHelper from '@jsroot/lib/langHelper'
import NoDataTitle from '@jsroot/components/NoDataTitle'
import NumberFormat from '@jsroot/lib/numberFormat'
import { HeaderText, Service } from '../lib/service'
import DataService from '@jsroot/lib/DataService.js'

export class DataGrid extends Component {
    constructor(props) {
        super(props);

        this.state = {
            gridData: { 
                Data: [{
                        Date: '01/01/2018',
                        SystemId: 'SystemId',
                        ExternalId: 'ExternalId',
                        Product: 'Product',
                        BetCount: 1000,
                        Turnover: 125874,
                        ValidBetAmount: 75,
                        CustomerWL: 164646.844
                }],
                Total: {
                    BetCount: 1000,
                    Turnover: 125874,
                    ValidBetAmount: 75,
                    CustomerWL: 164646.844
                }}
        }
    }

    componentDidUpdate(prevProps) {
        if (prevProps.params.Timestamp !== this.props.params.Timestamp) {
            this._getGridData();
            this.props.setReportTitle(Service.getReportTitle(
                this.props.params.FromDate,
                this.props.params.ToDate,
                this.props.params.CurrencyName,
                this.props.params.BaseCurrency));
        }
    }
    
    _getGridData() {
        //this.props.toggleLoader(true);
        //DataService.postData(Url, this.props.params)
        //    .then(response => {
        //        this.setState({ gridData: response.data });
        //        this.props.toggleLoader(false);
        //    })
        //    .catch(error => {
        //        Service.showErrorMsg();
        //        this.props.toggleLoader(false);
        //});
    }

    _buildHeader() {
        return (
            <Row style={{ height: 32 }}>
                <Cell colWidth={50} header={true} title="#">{HeaderText.Index}</Cell>
                <Cell colWidth={150} header={true} title={HeaderText.Date}>{HeaderText.Date}</Cell>
                <Cell colWidth={150} header={true} title={HeaderText.SystemId}>{HeaderText.SystemId}</Cell>
                <Cell colWidth={200} header={true} title={HeaderText.ExternalId}>{HeaderText.ExternalId}</Cell>
                <Cell colWidth={200} header={true} title={HeaderText.Product}>{HeaderText.Product}</Cell>
                <Cell colWidth={200} header={true} title={HeaderText.TotalTransaction}>{HeaderText.TotalTransaction}</Cell>
                <Cell colWidth={200} header={true} title={HeaderText.Turnover}>{HeaderText.Turnover}</Cell>
                <Cell colWidth={200} header={true} title={HeaderText.ValidBetAmount}>{HeaderText.ValidBetAmount}</Cell>
                <Cell colWidth={200} header={true} title={HeaderText.CustomerWL}>{HeaderText.CustomerWL}</Cell>
            </Row>
        )
    }

    _buildBody(data) {
        let rows = [];
        for (let i = 0; i < data.length; i++) {
            const item = data[i];
            const reportBy = moment(item.ReportBy).format("MM/DD/YYYY"),
                    betcount = NumberFormat.format(item.BetCount),
                    turnover = NumberFormat.formatDecimal(item.Turnover),
                    validBetAmount= NumberFormat.formatDecimal(item.ValidBetAmount),
                    customerWL = NumberFormat.formatDecimal(item.CustomerWL);

            rows.push(
                <Row key={`tr${i}`}>
                    <Cell title={i + 1} className="center-align">{i + 1}</Cell>
                    <Cell title={reportBy} className='center-align'>{reportBy}</Cell>
                    <Cell title={item.SystemId} className='center-align'>{item.SystemId}</Cell>
                    <Cell title={item.ExternalId} className="center-align">{item.ExternalId}</Cell>
                    <Cell title={item.Product}>{item.Product}</Cell>
                    <Cell title={betcount} className='right-align'>{betcount}</Cell>
                    <Cell title={turnover} className='right-align'>{turnover}</Cell>
                    <Cell title={validBetAmount} className="right-align">{validBetAmount}</Cell>
                    <Cell title={customerWL} className="right-align">{customerWL}</Cell>
                </Row>
            );
        }

        return rows;
    }

    _buildFooter(data, baseCurrency) {
        const item = data;

        const betcount = NumberFormat.format(item.BetCount),
                    turnover = NumberFormat.formatDecimal(item.Turnover),
                    validBetAmount= NumberFormat.formatDecimal(item.ValidBetAmount),
                    customerWL = NumberFormat.formatDecimal(item.CustomerWL);

        return [(
            <Row className="fixed-table-grant-total" key={1}>
                <Cell className="center-align" colSpan='5'>{LangHelper.getSingleResource("Totalin") + " " + baseCurrency}</Cell>
                <Cell title={betcount} className="right-align">{betcount}</Cell>
                <Cell title={turnover} className="right-align">{turnover}</Cell>
                <Cell title={validBetAmount} className="right-align">{validBetAmount}</Cell>
                <Cell title={customerWL} className={"right-align"}>{customerWL}</Cell>
            </Row>
        )]
    }

    render() {
        const data = this.state.gridData;
        if (!data.Data) {
            return null;
        }

        if (!data.Data.length) {
            return <NoDataTitle />;
        }

        return (
            <Table
                minWidth={800}
                autoWidth={true}
                bodyHeight={300}
                header={this._buildHeader()}
                body={this._buildBody(data.Data)}
                footer={this._buildFooter(data.Total, this.props.params.BaseCurrency)}
            />
        );
    }
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
}

function mapStateToProps(state) {
    const { params } = state;
    return Object.assign({}, { params });
}

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);