import React, { Component } from 'react'
import { connect } from 'react-redux'
import { ActionCreators } from '../actions/index.js'
import { bindActionCreators } from 'redux'
import { Table, Row, Cell } from '../../../components/fixed-table-header'
import NumberFormat from '../../../lib/numberFormat'
import ReportTitle from '../../../components/ReportTitle'
import { Service, TotalText, HeaderText, BreadcrumbText } from '../lib/service'
import { ReportLevel, ReportBy } from '../../../constants/report'
import { Licensee } from '../../../constants/licensee'
import NoDataTitle from '../../../components/NoDataTitle'
import moment from 'moment'
import Breadcrumb from '../../../components/breadcrumb/Breadcrumb'
import Site from '../../../lib/site'
import BetList from './BetList'
import Popup from '../../../components/popup'
import Report from '../../../lib/report'

export class DataGrid extends Component {
    constructor(props) {
        super(props);

        this.reportTitle = '';
        this.isAllowedCustomerComm = this.props.isAllowedCustomerComm;
        this.licenseeId = this.props.licenseeId;

        this.levelGridData = [];
        this.drillDownParams = [];

        this.state = {
            showBetList: false,
            betListData: {
                mainData: null,
                grandTotal: 0
            }
        }
    }

    componentWillReceiveProps(nextProps) {
        if (this.props.params !== nextProps.params) {
            this.drillDownParams[ReportLevel.MAIN] = nextProps.params;
        }

        if (this.props.gridData !== nextProps.gridData) {
            const params = this.drillDownParams[nextProps.gridData.level];
            this.reportTitle = Service.getReportTitle({ fromDate: params.FromDate, toDate: params.ToDate }, params.CurrencyName, params.CurrencyId === 0);
            this.props.toggleLoader(false);
        }
    }

    _getColSpan(level) {
        switch (level) {
            case ReportLevel.MAIN:
                return 2;
            case ReportLevel.LICENSEE:
                return 2;
            case ReportLevel.CUSTOMER:
                return 3;
            default: return 2;
        }
    }

    _getDrillDownParams(level, date, currencyId, currencyName, custId) {
        if (!this.props.xsrfToken) {
            return null;
        }

        const mainParam = this.drillDownParams[ReportLevel.MAIN];
        if (!mainParam) {
            return null;
        }

        const drillDownDate = Service.getDrilldownDateRange(mainParam.ReportBy, mainParam.FromDate, mainParam.ToDate, date);
        const drillDownData = {
            ...mainParam,
            CurrencyId: currencyId,
            CurrencyName: currencyName,
            FromDate: Site.convertDateToString(moment(drillDownDate.fromDate)),
            ToDate: Site.convertDateToString(moment(drillDownDate.toDate)),
            Level: level,
            CustId: custId
        };

        this.drillDownParams[level] = drillDownData;

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

            this.levelGridData[currentLevel] = this.props.gridData.data;

            this.props.getGridData(params, () => {
                Service.showErrorMsg();
                this.props.toggleLoader(false);
            });
        }
        else {
            const data = this.levelGridData[level];
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
        return <a onClick={() => this._drillDownData(level, date, currencyId, currencyName, custId)} href='javascript:;'>{text}</a>
    }

    _renderBetListLink(text) {
        return (<a href="javascript:;" onClick={this.showBetListPopup}>{text}</a>);
    }

    _buildHeader(level, reportBy) {
        let headerRows = [];

        headerRows.push(
            <Row style={{ height: 32 }} key='tr-header1'>
                <Cell colWidth={30} header={true} title={HeaderText.index} rowSpan='2'>{HeaderText.index}</Cell>
                {level === ReportLevel.MAIN && <Cell colWidth={100} header={true} title={reportBy === ReportBy.DAILY ? HeaderText.date : HeaderText.month} rowSpan='2'>{reportBy === ReportBy.DAILY ? HeaderText.date : HeaderText.month}</Cell>}
                {level === ReportLevel.CUSTOMER && <Cell colWidth={100} header={true} title={HeaderText.externalId} rowSpan='2'>{HeaderText.externalId}</Cell>}
                {level !== ReportLevel.MAIN && <Cell colWidth={100} header={true} title={HeaderText.systemId} rowSpan='2'>{HeaderText.systemId}</Cell>}
                <Cell colWidth={70} header={true} title={HeaderText.currency} rowSpan='2'>{HeaderText.currency}</Cell>
                <Cell colWidth={100} header={true} title={HeaderText.turnover} rowSpan='2'>{HeaderText.turnover}</Cell>
                <Cell colWidth={100} header={true} title={HeaderText.netTurnover} rowSpan='2'>{HeaderText.netTurnover}</Cell>
                <Cell colWidth={80} header={true} title={HeaderText.totalTransaction} rowSpan='2'>{HeaderText.totalTransaction}</Cell>
                {
                    this.isAllowedCustomerComm
                        ? <Cell colWidth={100} header={true} title={HeaderText.customer} colSpan='3'>{HeaderText.customer}</Cell>
                        : <Cell colWidth={100} header={true} title={HeaderText.customerWL} rowSpan='2'>{HeaderText.customerWL}</Cell>
                }
                <Cell colWidth={100} header={true} title={HeaderText.licensee} colSpan='4'>{HeaderText.licensee}</Cell>
            </Row>
        );

        if (this.isAllowedCustomerComm) {
            headerRows.push(
                <Row style={{ height: 32 }} key='tr-header2'>
                    <Cell colWidth={100} header={true} title={HeaderText.customerWL}>{HeaderText.customerWL}</Cell>
                    <Cell colWidth={100} header={true} title={HeaderText.customerComm}>{HeaderText.customerComm}</Cell>
                    <Cell colWidth={100} header={true} title={HeaderText.total}>{HeaderText.total}</Cell>
                    <Cell colWidth={100} header={true} title={HeaderText.licenseeWL}>{HeaderText.licenseeWL}</Cell>
                    <Cell colWidth={100} header={true} title={HeaderText.amountDueTo}>{HeaderText.amountDueTo}</Cell>
                    <Cell colWidth={100} header={true} title={HeaderText.licenseeComm}>{HeaderText.licenseeComm}</Cell>
                    <Cell colWidth={100} header={true} title={HeaderText.total}>{HeaderText.total}</Cell>
                </Row>
            );
        }
        else {
            headerRows.push(
                <Row style={{ height: 32 }} key='tr-header2'>
                    <Cell colWidth={100} header={true} title={HeaderText.licenseeWL}>{HeaderText.licenseeWL}</Cell>
                    <Cell colWidth={100} header={true} title={HeaderText.amountDueTo}>{HeaderText.amountDueTo}</Cell>
                    <Cell colWidth={100} header={true} title={HeaderText.licenseeComm}>{HeaderText.licenseeComm}</Cell>
                    <Cell colWidth={100} header={true} title={HeaderText.total}>{HeaderText.total}</Cell>
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
                    <Cell title={i + 1} className='center-align'>{i + 1}</Cell>
                    {level === ReportLevel.MAIN && <Cell title={date} className='center-align'>{date}</Cell>}
                    {
                        level === ReportLevel.CUSTOMER &&
                        <Cell title={item.ExternalId} className='left-align'>
                            {this._renderBetListLink(item.ExternalId)}
                        </Cell>
                    }
                    {
                        level !== ReportLevel.MAIN &&
                        <Cell title={item.SystemId} className='left-align'>
                            {
                                level === ReportLevel.LICENSEE ?
                                    this._renderDrillDownLink(item.SystemId, ReportLevel.CUSTOMER, item.Date, item.CurrencyId, item.CurrencyName, item.CustId) :
                                    this._renderBetListLink(item.SystemId)
                            }
                        </Cell>
                    }
                    <Cell title={item.CurrencyName} className='center-align'>
                        {
                            level === ReportLevel.MAIN
                                ? this._renderDrillDownLink(item.CurrencyName, this.licenseeId === Licensee.ONE_TWO_BET ? ReportLevel.LICENSEE : ReportLevel.CUSTOMER, item.Date, item.CurrencyId, item.CurrencyName)
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
            colSpan = this._getColSpan(level),
            rowSpan = data.length;

        for (let i = 0; i < rowSpan; i++) {
            const item = data[i];

            totalRows.push(
                <Row key={`tr-total${i}`} className="fixed-table-total">
                    {i === 0 && <Cell title={TotalText.total} className='center-align' rowSpan={rowSpan} colSpan={colSpan}>{TotalText.total}</Cell>}
                    <Cell title={item.CurrencyName} className='center-align'>{item.CurrencyName}</Cell>
                    {this._buildCell(item)}
                </Row>
            );
        }

        const dataSummary = this.props.gridData.data.SummaryTotal,
            baseCurrencyList = Object.keys(dataSummary),
            colSummarySpan = this._getColSpan(level) + 1;

        for (let i = baseCurrencyList.length - 1; i >= 0; i--) {
            const currency = baseCurrencyList[i],
                item = dataSummary[currency];

            totalRows.push(
                <Row key={`tr-grandtotal${i}`} className="fixed-table-grant-total">
                    <Cell title={`${TotalText.totalIn} ${currency}`} className='center-align' colSpan={colSummarySpan}>{`${TotalText.totalIn} ${currency}`}</Cell>
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
            this.isAllowedCustomerComm && <Cell key='customerComm' title={customerComm} className='right-align'>{customerComm}</Cell>,
            this.isAllowedCustomerComm && <Cell key='customerWLTotal' title={customerWLTotal} className='right-align'>{customerWLTotal}</Cell>,
            <Cell key='licenseeWL' title={licenseeWL} className={`right-align ${item.LicenseeWL < 0 ? 'negative' : ''}`}>{licenseeWL}</Cell>,
            <Cell key='amountDueTo' title={amountDueTo} className='right-align'>{amountDueTo}</Cell>,
            <Cell key='licenseeComm' title={licenseeComm} className='right-align'>{licenseeComm}</Cell>,
            <Cell key='licenseeWLTotal' title={licenseeWLTotal} className={`right-align ${item.LicenseeWLTotal < 0 ? 'negative' : ''}`}>{licenseeWLTotal}</Cell>];

        return cells;
    }

    _renderBreadcrumb() {
        const items = [{
            label: BreadcrumbText.main,
            value: ReportLevel.MAIN,
        }];

        if (Number(this.licenseeId) === Licensee.ONE_TWO_BET) {
            items.push({
                label: BreadcrumbText.licensee,
                value: ReportLevel.LICENSEE,
            });
        }

        items.push({
            label: BreadcrumbText.customer,
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

    showBetListPopup = () => {
        const url = "api/ExchangeWinLossBetList/Index";
        const params = {
            xsrfToken: this.props.xsrfToken,
            data: {}
        }

        this.props.toggleLoader(true);
        Report.fetchGridData(url, params)
            .then(response => {
                const { MainData, GrandTotal } = response.data;
                this.setState({
                    showBetList: true,
                    betListData: { mainData: MainData || [], grandTotal: GrandTotal }
                });
                this.props.toggleLoader(false);
            })
            .catch(error => {
                Service.showErrorMsg();
                this.props.toggleLoader(false);
            })
    }

    closeBetListPopup = () => {
        this.setState({ showBetList: false, betListData: null });
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
        const reportBy = this.props.params.ReportBy;

        const { showBetList, betListData } = this.state;

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

                {
                    reportLevel === ReportLevel.CUSTOMER && showBetList && betListData &&
                    <Popup isShow={showBetList} title={"CUSTOMER BET LIST"} width={900} height={600} onClose={this.closeBetListPopup}>
                        <BetList data={betListData} />
                    </Popup>
                }
            </div>
        );
    }
}

function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
};

function mapStateToProps(state) {
    const { gridData, params, xsrfToken, licenseeId, isAllowedCustomerComm } = state;
    return Object.assign({}, { gridData, params, xsrfToken, licenseeId, isAllowedCustomerComm });
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);