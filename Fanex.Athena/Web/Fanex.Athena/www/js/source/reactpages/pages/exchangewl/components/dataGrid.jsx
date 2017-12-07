import React, { Component } from 'react'
import { connect } from 'react-redux'
import { ActionCreators } from '../actions/index.js'
import { bindActionCreators } from 'redux'
import { Table, Row, Cell } from '../../../components/fixed-table-header'
import NumberFormat from '../../../lib/numberFormat'
import ReportTitle from '../../../components/ReportTitle'
import service from '../lib/service'
import { ReportLevel, ReportBy } from '../../../constants/report'
import NoDataTitle from '../../../components/NoDataTitle'
import moment from 'moment'

export class DataGrid extends Component {
    constructor(props) {
        super(props);

        this.reportTitle = '';
    }

    componentWillReceiveProps(nextProps) {
        if (this.props.gridData !== nextProps.gridData) {
            const { dateRange, selectedCurrency } = nextProps;
            this.reportTitle = service.getReportTitle(dateRange, selectedCurrency.label);
            this.props.toggleLoader(false);
        }
    }
    
    _getColSpan(level) {
        switch(level) {
            case ReportLevel.MAIN:
                return 2;
            case ReportLevel.LICENSEE:
                return 2;
            case ReportLevel.CUSTOMER:
                return 3;
            default: return 1;
        }
    }

    _buildHeader(level, reportBy) {
        const headerText = service.getHeaderText();
        let headerRows = [];

        headerRows.push(
            <Row style={{ height: 32 }} key='tr-header1'>
                <Cell colWidth={30} header={true} title={headerText.index} rowSpan='2'>{headerText.index}</Cell>
                {level === ReportLevel.MAIN ? <Cell colWidth={150} header={true} title={reportBy === ReportBy.DAILY ? headerText.date : headerText.month} rowSpan='2'>{reportBy === ReportBy.DAILY ? headerText.date : headerText.month}</Cell> : null}
                {level === ReportLevel.CUSTOMER ? <Cell colWidth={150} header={true} title={headerText.externalId} rowSpan='2'>{headerText.externalId}</Cell> : null}
                {level !== ReportLevel.MAIN ? <Cell colWidth={150} header={true} title={headerText.systemId} rowSpan='2'>{headerText.systemId}</Cell> : null}
                <Cell colWidth={90} header={true} title={headerText.currency} rowSpan='2'>{headerText.currency}</Cell>
                <Cell colWidth={150} header={true} title={headerText.turnover} rowSpan='2'>{headerText.turnover}</Cell>
                <Cell colWidth={120} header={true} title={headerText.totalTransaction} rowSpan='2'>{headerText.totalTransaction}</Cell>
                <Cell colWidth={150} header={true} title={headerText.customer} colSpan='3'>{headerText.customer}</Cell>
                <Cell colWidth={150} header={true} title={headerText.licensee} colSpan='4'>{headerText.licensee}</Cell>
            </Row>
        );

        headerRows.push(
          <Row style={{ height: 32 }} key='tr-header2'>
                <Cell colWidth={150} header={true} title={headerText.customerWL}>{headerText.customerWL}</Cell>
                <Cell colWidth={150} header={true} title={headerText.customerComm}>{headerText.customerComm}</Cell>
                <Cell colWidth={150} header={true} title={headerText.total}>{headerText.total}</Cell>
                <Cell colWidth={150} header={true} title={headerText.licenseeWL}>{headerText.licenseeWL}</Cell>
                <Cell colWidth={150} header={true} title={headerText.amountDueTo}>{headerText.amountDueTo}</Cell>
                <Cell colWidth={150} header={true} title={headerText.licenseeComm}>{headerText.licenseeComm}</Cell>
                <Cell colWidth={150} header={true} title={headerText.total}>{headerText.total}</Cell>
            </Row>
        );

        return headerRows;
    }

    _buildBody(level, reportBy) {
        const data = this.props.gridData.DataList;
        let rows = [];
        for (let i = 0; i < data.length; i++) {
            const item = data[i];
            const date = moment(item.Date).format(reportBy === ReportBy.DAILY ? "MM/DD/YYYY" : "MM/YYYY");

            rows.push(
                <Row key={`tr-main${i}`}>
                    <Cell title={i} className='center-align'>{i}</Cell>
                    {level === ReportLevel.MAIN ? <Cell title={date} className='center-align'>{date}</Cell> : null}
                    {level === ReportLevel.CUSTOMER ? <Cell title={item.ExternalId} className='center-align'>{item.ExternalId}</Cell> : null}
                    {level !== ReportLevel.MAIN ? <Cell title={item.SystemId} className='center-align'>{item.SystemId}</Cell> : null}
                    <Cell title={item.CurrencyName} className='center-align'>{item.CurrencyName}</Cell>
                    {this._buildCell(item)}
                </Row>
            );
        }

        return rows;
    }

    _buildFooter(level) {       
        return this._buildTotal(level);
    }
     
    _buildTotal(level) {
        let totalRows = [];

        const data = this.props.gridData.TotalDataList,
            totalText = service.getTotalText().total,
            colSpan = this._getColSpan(level),
            rowSpan = data.length;

        for (let i = 0; i < rowSpan; i++) {
            const item = data[i];
           
            totalRows.push(
                <Row key={`tr-total${i}`} className="fixed-table-total">
                    {i === 0 ? <Cell title={totalText} className='center-align' rowSpan={rowSpan} colSpan={colSpan}>{totalText}</Cell> : undefined}
                    <Cell title={item.CurrencyName} className='center-align'>{item.CurrencyName}</Cell>
                    {this._buildCell(item)}
                </Row>
            );
        }

        const dataSummary = this.props.gridData.SummaryTotal,
            baseCurrencyList = Object.keys(dataSummary),
            totalInText = service.getTotalText().totalIn,
            colSummarySpan = this._getColSpan(level) + 1;

        for (let i = 0; i < baseCurrencyList.length; i++) {
            const currency = baseCurrencyList[i],
                item = dataSummary[currency];
           
            totalRows.push(
                <Row key={`tr-grandtotal${i}`} className="fixed-table-grant-total">
                    <Cell title={`${totalInText} ${currency}`} className='center-align' colSpan={colSummarySpan}>{`${totalInText} ${currency}`}</Cell>
                    {this._buildCell(item)}
                </Row>
            );
        }

        return totalRows;
    }

    _buildCell(item) {
        const turnover = NumberFormat.formatDecimal(item.Turnover),
            totalTransaction = NumberFormat.format(item.TotalTransaction),
            customerWL = NumberFormat.formatDecimal(item.CustomerWL),
            customerComm = NumberFormat.formatDecimal(item.CustomerComm),
            customerWLTotal = NumberFormat.formatDecimal(item.CustomerWLTotal),
            licenseeWL = NumberFormat.formatDecimal(item.LicenseeWL),
            amountDueTo = NumberFormat.formatDecimal(item.AmountDueTo),
            licenseeComm = NumberFormat.formatDecimal(item.LicenseeComm),
            licenseeWLTotal = NumberFormat.formatDecimal(item.LicenseeWLTotal);

        var cells = 
            [<Cell key='turnover' title={turnover} className='right-align'>{turnover}</Cell>,
            <Cell key='totalTransaction' title={totalTransaction} className='right-align'>{totalTransaction}</Cell>,
            <Cell key='customerWL' title={customerWL} className='right-align'>{customerWL}</Cell>,
            <Cell key='customerComm' title={customerComm} className='right-align'>{customerComm}</Cell>,
            <Cell key='customerWLTotal' title={customerWLTotal} className='right-align'>{customerWLTotal}</Cell>,
            <Cell key='licenseeWL' title={licenseeWL} className={`right-align ${item.LicenseeWL < 0 ? 'negative' : ''}`}>{licenseeWL}</Cell>,
            <Cell key='amountDueTo' title={amountDueTo} className='right-align'>{amountDueTo}</Cell>,
            <Cell key='licenseeComm' title={licenseeComm} className='right-align'>{licenseeComm}</Cell>,
            <Cell key='licenseeWLTotal' title={licenseeWLTotal} className={`right-align ${item.LicenseeWLTotal < 0 ? 'negative' : ''}`}>{licenseeWLTotal}</Cell>]
        ;

        return cells;
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
        const { DataList } = this.props.gridData;
        if (!DataList) {
            return null;
            }

        if (!DataList.length) {
            return this._renderNodata();
            }

        const reportLevel = this.props.reportLevel;
        const reportBy = this.props.params[ReportLevel.MAIN].ReportBy;

        return (
            <div className='result-content'>
                <ReportTitle content={this.reportTitle} />

                <Table
                    minWidth={600}
                    autoWidth={true}
                    bodyHeight={300}
                    header={this._buildHeader(reportLevel, reportBy)}
                    body={this._buildBody(reportLevel, reportBy)}
                    footer={this._buildFooter(reportLevel)}
                />
            </div>
        );
    }
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
};

function mapStateToProps(state) {
    const { gridData, dateRange, selectedCurrency, selectedReportBy, excludeTestDataOption, isLoading, params, reportLevel } = state;
    return Object.assign({}, { gridData, dateRange, selectedCurrency, selectedReportBy, excludeTestDataOption, isLoading, params, reportLevel });
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);