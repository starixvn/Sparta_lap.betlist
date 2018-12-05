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
import Site from '@jsroot/lib/site'

const URL = Site.resolveClientUrl("api/CasinoTurnoverReport/GetData");

const DEFAULT_PAGE_SIZE = 1;

export class DataGrid extends Component {
    constructor(props) {
        super(props);

        this.state = {
            gridData: {}
        }
    }

    componentDidMount() {
        const param = this.props.requestParam;
        param.PageSize = DEFAULT_PAGE_SIZE;
        this.props.setRequestParam(param);
    }

    componentDidUpdate(prevProps) {
        if (prevProps.gridData.timestamp !== this.props.gridData.timestamp) {
            this.props.toggleLoader(false);
        }
    }

    onPaging = (index, size) => {
        let param = this.props.requestParam;
        param.PageIndex = index;
        param.PageSize = size;
        this._getData(param);
    }

    _getData(param){
        this.props.toggleLoader(true);
        this.props.getGridData(URL, param);
        this.props.setRequestParam(param);
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
                const reportBy = moment(item.Date).format("MM/DD/YYYY"),
                        betcount = NumberFormat.format(item.BetCount),
                        turnover = NumberFormat.formatDecimal(item.Turnover),
                        validBetAmount= NumberFormat.formatDecimal(item.ValidBetAmount),
                        customerWL = NumberFormat.formatDecimal(item.CustomerWL);
                let indexCell = null;
                let dateCell = null;
                let systemIdCell = null;
                let externalIdCell = null;
                if(item.RowSpan > 0)
                {
                    indexCell = <Cell title={i + 1} rowSpan={item.RowSpan} className="center-align">{i + 1}</Cell>;
                    dateCell = <Cell title={reportBy} rowSpan={item.RowSpan} className='center-align'>{reportBy}</Cell>;
                    systemIdCell = <Cell title={item.SystemId} rowSpan={item.RowSpan} className='center-align'>{item.SystemId}</Cell>;
                    externalIdCell = <Cell title={item.ExternalId} rowSpan={item.RowSpan} className="center-align">{item.ExternalId}</Cell>;
                }
                    
                rows.push(
                    <Row key={`tr${i}`}>
        {indexCell}
        {dateCell}
        {systemIdCell}
        {externalIdCell}
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

    _renderNoData(){
        return (
            <div className='result-content'>
           
            
            </div>
            )
    }

    render() {
        if (!this.props.gridData || !this.props.gridData.Data) {
            return null;
        }

        const { Data } = this.props.gridData;
        
        if (!Data) {
            return null;
        }

        if (!Data.length) {
            return <NoDataTitle />
        }

        const param = this.props.requestParam;
        const pageOption = {
            PageIndex: param.PageIndex,
            PageSize: param.PageSize,
            TotalItem: this.props.gridData.TotalRows,
            PageList: [DEFAULT_PAGE_SIZE, 100, 200, 300, 400, 500, 1000, 2000]
        };

        return (
             <Table
                minWidth={800}
                autoWidth={true}
                bodyHeight={300}
                header={this._buildHeader()}
                body={this._buildBody(Data)}
                footer={this._buildFooter(this.props.gridData.Total, this.props.requestParam.BaseCurrency)}
                isPaging={true}
                pageOption={pageOption}
                onPaging={this.onPaging}
            />
        );
      }
      }

function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
                }

function mapStateToProps(state) {
    const { requestParam, gridData } = state;
    return Object.assign({}, { requestParam, gridData });
                }

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);