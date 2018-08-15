import moment from 'moment'
import React, { Component } from 'react'
import { connect } from 'react-redux'
import { ActionCreators } from '../actions/index.js'
import { bindActionCreators } from 'redux'
import { Table, Row, Cell } from '../../../components/fixed-table-header'
import NumberFormat from '../../../lib/numberFormat'
import Breadcrumb from '../../../components/breadcrumb/Breadcrumb'
import ReportTitle from '../../../components/ReportTitle'
import NoDataTitle from '../../../components/NoDataTitle'
import Report from '../../../lib/report'
import { ReportLevel, ReportBy } from '../../../constants/report'
import { Licensee } from '../../../constants/licensee'
import { Service, HeaderText, Url, BreadcrumbText } from '../lib/service'
import Site from '../../../lib/site'

export class DataGrid extends Component {
    constructor(props) {
        super(props);

        this.isAllowedCustomerComm = this.props.setting.isAllowedCustomerComm;
        this.licenseeId = this.props.setting.licenseeId;
        this.isDisableBetList = this.props.setting.isDisableBetList;
        this.breadcumbItems = [{
            label: BreadcrumbText.main,
            value: ReportLevel.MAIN,
        }];

        if (Number(this.props.setting.licenseeId) === Licensee.ONE_TWO_BET) {
            this.breadcumbItems.push({
                label: BreadcrumbText.licensee,
                value: ReportLevel.LICENSEE});
        }

        this.breadcumbItems.push({
            label: BreadcrumbText.customer,
            value: ReportLevel.CUSTOMER});

        this.levelGridData = [];
        this.drillDownParams = [];
    }

    componentDidUpdate(prevProps) {
        if (prevProps.gridData !== this.props.gridData) {
            this.props.toggleLoader(false);
        }
    }

    onPaging = (index, size) => {
        let params = this.props.params;

        params.PageIndex = index;
        params.PageSize = size;

        this._updateGridData(params);
        this.props.setParams(params);
    }

    _updateGridData(params) {
        this.props.toggleLoader(true);
        Report.fetchGridData(Url, { xsrfToken: this.props.xsrfToken, data: params })
            .then(response => {
                this.props.setGridData(response.data);
            })
            .catch(error => {
                Service.showErrorMsg();
                this.props.toggleLoader(false);
            });
    }

    _buildHeader() {
        const level = this.props.params.DrillDownLevel;
        const rowSpan = this.isAllowedCustomerComm ? 2 : 1;

        let rows = [
            (<Row key='header1' style={{ height: 32 }}>
                <Cell colWidth={50} header={true} rowSpan={rowSpan}>{HeaderText.index}</Cell>
                {
                    level === ReportLevel.CUSTOMER
                    && <Cell colWidth={120} header={true} rowSpan={rowSpan}>{HeaderText.externalId}</Cell>
                }
                {
                    level === ReportLevel.MAIN
                    ? <Cell colWidth={150} header={true} rowSpan={rowSpan}>{HeaderText.date}</Cell>
                    : <Cell colWidth={150} header={true} rowSpan={rowSpan}>{HeaderText.systemId}</Cell>
                }      
                <Cell colWidth={100} header={true} rowSpan={rowSpan}>{HeaderText.currency}</Cell>
                <Cell colWidth={200} header={true} rowSpan={rowSpan}>{HeaderText.turnover}</Cell>
                <Cell colWidth={130} header={true} rowSpan={rowSpan}>{HeaderText.totalTransaction}</Cell>
                {
                    this.isAllowedCustomerComm
                    ? <Cell colWidth={520} header={true} colSpan={3}>{HeaderText.customer}</Cell>
                    : <Cell colWidth={200} header={true} rowSpan={rowSpan}>{HeaderText.customer}</Cell>
                }
                <Cell colWidth={200} header={true} rowSpan={rowSpan}>{HeaderText.licenseeWL}</Cell>
            </Row>)
        ];

        if (this.isAllowedCustomerComm) {
            rows.push(
                (<Row key='header2' style={{ height: 32 }}>
                    <Cell colWidth={200} header={true}>{HeaderText.customerWL}</Cell>
                    <Cell colWidth={120} header={true}>{HeaderText.customerComm}</Cell>
                    <Cell colWidth={200} header={true}>{HeaderText.customerTotal}</Cell>
                </Row>)
            );
        }
        
        return rows;
    }

    _buildBody() {
        const data = this.props.gridData.Data,
            level = this.props.params.DrillDownLevel,
            formatDate = this.props.params.ReportBy === ReportBy.DAILY.toString() ? "MM/DD/YYYY" : "MM/YYYY";
        
        let rows = [];
        for (let i = 0; i < data.length; i++) {
            const item = data[i];
            const reportBy = level === ReportLevel.MAIN 
                        ? moment(item.Date).format(formatDate)
                        : '',
                    turnover = NumberFormat.formatDecimal(item.TotalTurnOver),
                    customerWl= NumberFormat.formatDecimal(item.CustomerWl),
                    customerComm= NumberFormat.formatDecimal(item.CustomerComm),
                    customerTotal= NumberFormat.formatDecimal(item.CustomerTotal),
                    licenseeWL = NumberFormat.formatDecimal(item.TotalLicenseeWL);
            
            rows.push(
                <Row key={`tr${i}`}>
                    <Cell title={i + 1} className='center-align'>{i + 1}</Cell>
                    {
                        level === ReportLevel.CUSTOMER
                        && <Cell title={item.ExternalId} className="center-align">{item.ExternalId}</Cell>
                    }
                    {
                        level === ReportLevel.MAIN
                        ? <Cell title={reportBy} className="center-align">{reportBy}</Cell>
                        : !this.isDisableBetList && <Cell title={item.SystemId} className="center-align">
                            {
                                level === ReportLevel.LICENSEE
                                ? this._renderDrillDownLink(item.SystemId, ReportLevel.CUSTOMER, item.Date, item.CurrencyId, item.CurrencyName, item.CustId)
                                : item.SystemId
                            }
                        </Cell>
                    }
                    <Cell title={item.CurrencyName} className="center-align">
                        {
                            level === ReportLevel.MAIN && !this.isDisableBetList
                            ? this._renderDrillDownLink(item.CurrencyName, this.licenseeId === Licensee.ONE_TWO_BET ? ReportLevel.LICENSEE : ReportLevel.CUSTOMER, item.Date, item.CurrencyId, item.CurrencyName)
                            : item.CurrencyName
                        }                  
                    </Cell>                                     
                    <Cell title={turnover} className="right-align">{turnover}</Cell>
                    <Cell title={item.TotalTransaction} className={`right-align`}>{item.TotalTransaction}</Cell>
                    { this.isAllowedCustomerComm && <Cell title={customerWl} className="right-align">{customerWl}</Cell> }
                    { this.isAllowedCustomerComm && <Cell title={customerComm} className="right-align">{customerComm}</Cell> }
                    <Cell title={customerTotal} className="right-align">{customerTotal}</Cell>
                    <Cell title={licenseeWL} className={`right-align ${licenseeWL < 0 ? "negative" : ""}`}>{licenseeWL}</Cell>
                </Row>
            );
    }

        return rows;
    }

    _buildFooter() {
    }

    _getDrillDownParams(level, date, currencyId, currencyName, custId) {
        if (!this.props.xsrfToken) {
            return null;
        }

        let params = this.props.params;
        if (!params) {
            return null;
        }

        const drillDownDate = Report.getDrilldownDateRange(params.ReportBy, params.FromDate, params.ToDate, date);
        params.DrillDownLevel = level;
        params.DrillDownFromDate = Site.convertDateToString(moment(drillDownDate.fromDate));
        params.DrillDownToDate = Site.convertDateToString(moment(drillDownDate.toDate));
        params.DrillDownCurrencyId = currencyId;
        params.DrillDownCurrencyName = currencyName;
        params.DrillDownCustomerId = custId;

        return params;
    }

    _drillDownData(level, date, currencyId, currencyName, custId) {
        const currentLevel = this.props.params.DrillDownLevel;

        if (currentLevel === level || level < ReportLevel.MAIN || level > ReportLevel.CUSTOMER) {
            return;
        }

        if (level > currentLevel) {
            this.drillDownParams[currentLevel] = { ...this.props.params };
        
            const drillDownParams = this._getDrillDownParams(level, date, currencyId, currencyName, custId);
            if (!drillDownParams) {
                return;
            }

            this.levelGridData[currentLevel] = this.props.gridData;
            this._updateGridData(drillDownParams);
            this.props.setParams(drillDownParams);
            this.props.setReportTitle(Service.getReportTitle(
                drillDownParams.DrillDownFromDate,
                drillDownParams.DrillDownToDate,
                drillDownParams.DrillDownCurrencyId,
                drillDownParams.DrillDownCurrencyName,
                drillDownParams.BaseCurrency));
        }
        else {
            const data = this.levelGridData[level];
            if (!data) {
                return;
            }
            
            const params = this.drillDownParams[level];

            this.props.toggleLoader(true);
            this.props.setGridData(data);
            this.props.setParams(this.drillDownParams[level]);
            this.props.setReportTitle(Service.getReportTitle(
                level === ReportLevel.MAIN ? params.FromDate : params.DrillDownFromDate,
                level === ReportLevel.MAIN ? params.ToDate : params.DrillDownToDate,
                level === ReportLevel.MAIN ? params.CurrencyId : params.DrillDownCurrencyId,
                level === ReportLevel.MAIN ? params.CurrencyName : params.DrillDownCurrencyName,
                params.BaseCurrency));
        }     
    }

    _renderDrillDownLink(text, level, date, currencyId, currencyName, custId) {
        return <a onClick={() => this._drillDownData(level, date, currencyId, currencyName, custId)} href='javascript:;'>{text}</a>
    }

    _renderBreadcrumb() {
        return <Breadcrumb items={this.breadcumbItems} value={this.props.params.DrillDownLevel} onChange={(item) => this._drillDownData(item.value)} />
    }

    _renderNodata() {
        return (
            <div className="result-content">
                {this._renderBreadcrumb()}
                <ReportTitle content={this.props.reportTitle} />
                <NoDataTitle />
            </div>
        )
    }

    render() {
        const data = this.props.gridData.Data;
        if (!data) {
            return null;
        }

        if (!data.length) {
            return this._renderNodata();
        }
        
        const param = this.props.params;
            const pageOption = {
            PageIndex: param.PageIndex,
            PageSize: param.PageSize,
            TotalItem: data.Total,
            PageList: [50, 100, 200, 300, 400, 500, 1000, 2000]
        };

        return (
            <div className='result-content'>
                {this._renderBreadcrumb()}

                <ReportTitle content={this.props.reportTitle} />
            
                <Table
                    minWidth={800}
                    autoWidth={true}
                    bodyHeight={300}
                    header={this._buildHeader()}
                    body={this._buildBody()}
                    footer={this._buildFooter()}
                    isPaging={true}
                    pageOption={pageOption}
                    onPaging={this.onPaging}
                />
            </div>
        );
    }
}
function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
}
function mapStateToProps(state) {
    const { params, setting, xsrfToken, gridData, reportTitle } = state;
    return Object.assign({}, { params, setting, xsrfToken, gridData, reportTitle });
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);