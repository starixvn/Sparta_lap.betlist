import React, { Component } from 'react'
import { connect } from 'react-redux'
import { ActionCreators } from '../actions/index.js'
import { bindActionCreators } from 'redux'
import { Table, Row, Cell } from '../../../components/fixed-table-header'
import NumberFormat from '../../../lib/numberFormat'
import ReportTitle from '../../../components/ReportTitle'
import service from '../lib/service'
import { ReportLevel, ReportBy } from '../../../constants/report'
import { Licensee } from '../../../constants/licensee'
import NoDataTitle from '../../../components/NoDataTitle'
import moment from 'moment'
import Breadcrumb from '../../../components/breadcrumb/Breadcrumb'
import Site from '../../../lib/site'
import Report from '../../../lib/report'

export class DataGrid extends Component {
    constructor(props) {
        super(props);

        this.reportTitle = '';
        this.isAllowedCustomerComm = this.props.isAllowedCustomerComm;
        this.licenseeId = this.props.licenseeId;
    }

    componentWillReceiveProps(nextProps) {
        if (this.props.gridData !== nextProps.gridData) {
            const param = this.props.params[nextProps.gridData.level];
            this.reportTitle = service.getReportTitle({ fromDate: param.FromDate, toDate: param.ToDate }, param.CurrencyName, param.CurrencyId === 0);
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

    _getDrillDownParams(level, date, currencyId, currencyName, custId) {
        if (!this.props.xsrfToken) {
            return null;
        }

        let param = this.props.params[ReportLevel.MAIN];
        if (!param) {
            return null;
        }

        const drillDownDate = Report.getDrilldownDateRange(param.ReportBy, param.FromDate, param.ToDate, date);
        const drillDownData = { 
            ...param,
            CurrencyId: currencyId,
            CurrencyName: currencyName,
            FromDate: Site.convertDateToString(moment(drillDownDate.fromDate)),
            ToDate: Site.convertDateToString(moment(drillDownDate.toDate)),
            Level: level,
            CustId: custId
        };

        let params = this.props.params;
        params[level] = drillDownData;       
        this.props.setParams(params);

        return {
            xsrfToken: this.props.xsrfToken,
            data: drillDownData
        };
    }

    _drillDownData(level, date, currencyId, currencyName, custId) {
        const currentLevel = this.props.gridData.level;

        if (currentLevel === level || level < ReportLevel.MAIN || level > ReportLevel.CUSTOMER) {
            return;
        }

        if (level > currentLevel) {
            const params = this._getDrillDownParams(level, date, currencyId, currencyName, custId);
            if (!params) {
                return;
            }

            this.props.toggleLoader(true);

            let levelGridData = this.props.levelGridData;
            if (!levelGridData) {
                levelGridData = [];
            }
            levelGridData[currentLevel] = this.props.gridData.data;
            this.props.setLevelGridData(levelGridData);

            this.props.getGridData(params);
        }
        else {
            const data = this.props.levelGridData[level];
            if (!data) {
                return;
            }

            this.props.toggleLoader(true);
            this.props.setGridData({
                data: data,
                level: level
            });
        }
    }

    _renderDrillDownLink(text, level, date, currencyId, currencyName, custId) {
        return <a onClick={()=>this._drillDownData(level, date, currencyId, currencyName, custId)} href='javascript:;'>{text}</a>
   }

    _buildHeader(level, reportBy) {
        const headerText = service.getHeaderText();
        let headerRows = [];

        headerRows.push(
            <Row style={{ height: 32 }} key='tr-header1'>
                <Cell colWidth={30} header={true} title={headerText.index} rowSpan='2'>{headerText.index}</Cell>
                {level === ReportLevel.MAIN ? <Cell colWidth={100} header={true} title={reportBy === ReportBy.DAILY ? headerText.date : headerText.month} rowSpan='2'>{reportBy === ReportBy.DAILY ? headerText.date : headerText.month}</Cell> : undefined}
                {level === ReportLevel.CUSTOMER ? <Cell colWidth={100} header={true} title={headerText.externalId} rowSpan='2'>{headerText.externalId}</Cell> : undefined}
                {level !== ReportLevel.MAIN ? <Cell colWidth={100} header={true} title={headerText.systemId} rowSpan='2'>{headerText.systemId}</Cell> : undefined}
                <Cell colWidth={70} header={true} title={headerText.currency} rowSpan='2'>{headerText.currency}</Cell>
                <Cell colWidth={100} header={true} title={headerText.turnover} rowSpan='2'>{headerText.turnover}</Cell>
                <Cell colWidth={100} header={true} title={headerText.netTurnover} rowSpan='2'>{headerText.netTurnover}</Cell>
                <Cell colWidth={80} header={true} title={headerText.totalTransaction} rowSpan='2'>{headerText.totalTransaction}</Cell>
                {
                    this.isAllowedCustomerComm
                    ? <Cell colWidth={100} header={true} title={headerText.customer} colSpan='3'>{headerText.customer}</Cell>
                    : <Cell colWidth={100} header={true} title={headerText.customerWL} rowSpan='2'>{headerText.customerWL}</Cell>
                }
                <Cell colWidth={100} header={true} title={headerText.licensee} colSpan='4'>{headerText.licensee}</Cell>
            </Row>
        );

        if (this.isAllowedCustomerComm) {
            headerRows.push(
                <Row style={{ height: 32 }} key='tr-header2'>
                    <Cell colWidth={100} header={true} title={headerText.customerWL}>{headerText.customerWL}</Cell>
                    <Cell colWidth={100} header={true} title={headerText.customerComm}>{headerText.customerComm}</Cell>
                    <Cell colWidth={100} header={true} title={headerText.total}>{headerText.total}</Cell>
                    <Cell colWidth={100} header={true} title={headerText.licenseeWL}>{headerText.licenseeWL}</Cell>
                    <Cell colWidth={100} header={true} title={headerText.amountDueTo}>{headerText.amountDueTo}</Cell>
                    <Cell colWidth={100} header={true} title={headerText.licenseeComm}>{headerText.licenseeComm}</Cell>
                    <Cell colWidth={100} header={true} title={headerText.total}>{headerText.total}</Cell>
                </Row>
            );
        }
        else {
            headerRows.push(
                <Row style={{ height: 32 }} key='tr-header2'>
                    <Cell colWidth={100} header={true} title={headerText.licenseeWL}>{headerText.licenseeWL}</Cell>
                    <Cell colWidth={100} header={true} title={headerText.amountDueTo}>{headerText.amountDueTo}</Cell>
                    <Cell colWidth={100} header={true} title={headerText.licenseeComm}>{headerText.licenseeComm}</Cell>
                    <Cell colWidth={100} header={true} title={headerText.total}>{headerText.total}</Cell>
                </Row>
            );
        }

        return headerRows;
    }

    _buildBody(level, reportBy) {
        const data = this.props.gridData.data.DataList;
        let rows = [];
        for (let i = 0; i < data.length; i++) {
            const item = data[i];
            const date = moment(item.Date).format(reportBy === ReportBy.DAILY ? "MM/DD/YYYY" : "MM/YYYY");

            rows.push(
                <Row key={`tr-main${i}`}>
                    <Cell title={i+1} className='center-align'>{i+1}</Cell>
                    {level === ReportLevel.MAIN ? <Cell title={date} className='center-align'>{date}</Cell> : undefined}
                    {level === ReportLevel.CUSTOMER ? <Cell title={item.ExternalId} className='center-align'>{item.ExternalId}</Cell> : undefined}
                    {
                        level !== ReportLevel.MAIN
                            ? <Cell title={item.SystemId} className='center-align'>
                                {level === ReportLevel.LICENSEE ? this._renderDrillDownLink(item.SystemId, ReportLevel.CUSTOMER, item.Date, item.CurrencyId, item.CurrencyName) : item.SystemId}
                            </Cell>
                            : undefined
                    }
                    <Cell title={item.CurrencyName} className='center-align'>
                    {
                        level === ReportLevel.MAIN 
                        ? this._renderDrillDownLink(item.CurrencyName, this.licenseeId === Licensee.ONE_TWO_BET ? ReportLevel.LICENSEE : ReportLevel.CUSTOMER, item.Date, item.CurrencyId, item.CurrencyName, item.CustId) 
                        : item.CurrencyName
                    }
                    </Cell>
                    {this._buildCell(item)}
                </Row>
            );
        }

        return rows;
    }
     
    _buildFooter(level) {
        let totalRows = [];

        const data = this.props.gridData.data.TotalDataList,
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

        const dataSummary = this.props.gridData.data.SummaryTotal,
            baseCurrencyList = Object.keys(dataSummary),
            totalInText = service.getTotalText().totalIn,
            colSummarySpan = this._getColSpan(level) + 1;

        for (let i = baseCurrencyList.length - 1; i >= 0; i--) {
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
            netTurnover = NumberFormat.formatDecimal(item.NetTurnover),
            totalTransaction = NumberFormat.format(item.TotalTransaction),
            customerWL = NumberFormat.formatDecimal(item.CustomerWL),
            customerComm = NumberFormat.formatDecimal(item.CustomerComm),
            customerWLTotal = NumberFormat.formatDecimal(item.CustomerWLTotal),
            licenseeWL = NumberFormat.formatDecimal(item.LicenseeWL),
            amountDueTo = NumberFormat.formatDecimal(item.AmountDueTo),
            licenseeComm = NumberFormat.formatDecimal(item.LicenseeComm),
            licenseeWLTotal = NumberFormat.formatDecimal(item.LicenseeWLTotal);

        let cells = 
            [<Cell key='turnover' title={turnover} className='right-align'>{turnover}</Cell>,
            <Cell key='netturnover' title={netTurnover} className='right-align'>{netTurnover}</Cell>,
            <Cell key='totalTransaction' title={totalTransaction} className='right-align'>{totalTransaction}</Cell>,
            <Cell key='customerWL' title={customerWL} className='right-align'>{customerWL}</Cell>,
            this.isAllowedCustomerComm ? <Cell key='customerComm' title={customerComm} className='right-align'>{customerComm}</Cell> : undefined,
            this.isAllowedCustomerComm ? <Cell key='customerWLTotal' title={customerWLTotal} className='right-align'>{customerWLTotal}</Cell> : undefined,
            <Cell key='licenseeWL' title={licenseeWL} className={`right-align ${item.LicenseeWL < 0 ? 'negative' : ''}`}>{licenseeWL}</Cell>,
            <Cell key='amountDueTo' title={amountDueTo} className='right-align'>{amountDueTo}</Cell>,
            <Cell key='licenseeComm' title={licenseeComm} className='right-align'>{licenseeComm}</Cell>,
            <Cell key='licenseeWLTotal' title={licenseeWLTotal} className={`right-align ${item.LicenseeWLTotal < 0 ? 'negative' : ''}`}>{licenseeWLTotal}</Cell>];

        return cells;
    }

    _renderBreadcrumb() {
        const breadcrumbText = service.getBreadcrumbText();
        const items = [{
                    label: breadcrumbText.main,
                    value: ReportLevel.MAIN,        
                }];

        if (Number(this.licenseeId) === Licensee.ONE_TWO_BET) {
            items.push({
                label: breadcrumbText.licensee,
                value: ReportLevel.LICENSEE,          
            });
        }

        items.push({
            label: breadcrumbText.customer,
            value: ReportLevel.CUSTOMER,           
        });

        return <Breadcrumb items={items} value={this.props.gridData.level} onChange={(item) => this._drillDownData(item.value)} />
    }

    _renderNodata() {
        return (
            <div className="result-content">
                {this._renderBreadcrumb()}
                <ReportTitle content={this.reportTitle} />
                <NoDataTitle />
            </div>
        )
    }

    render() {
        const gridData = this.props.gridData;
        if (!gridData || !gridData.data) {
            return null;
        }

        if (!gridData.data.DataList.length) {
            return this._renderNodata();
        }

        const reportLevel = gridData.level;
        const reportBy = this.props.params[ReportLevel.MAIN].ReportBy;

        return (
            <div className='result-content'>
                {this._renderBreadcrumb()}

                <ReportTitle content={this.reportTitle} />

                <Table
                    minWidth={1000}
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
    const { gridData, dateRange, selectedCurrency, selectedReportBy, excludeTestDataOption, isLoading, params, xsrfToken, levelGridData, licenseeId, isAllowedCustomerComm } = state;
    return Object.assign({}, { gridData, dateRange, selectedCurrency, selectedReportBy, excludeTestDataOption, isLoading, params, xsrfToken, levelGridData, licenseeId, isAllowedCustomerComm });
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);