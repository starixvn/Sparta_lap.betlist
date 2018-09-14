import moment from 'moment'
import React, { Component } from 'react'
import { connect } from 'react-redux'
import { ActionCreators } from '../actions/index.js'
import { bindActionCreators } from 'redux'
import { Table, Row, Cell } from '../../../components/fixed-table-header'
import NumberFormat from '../../../lib/numberFormat'
import NoDataTitle from '../../../components/NoDataTitle'
import Report from '../../../lib/report'
import { ReportBy } from '../../../constants/report'
import { Service, HeaderText, Url, TotalText } from '../lib/service'

export class DataGrid extends Component {
    constructor(props) {
        super(props);

        this.state = {
            expandTurnover: false,
            expandLicensee: false,
            gridData: {}
        }
    }

    componentDidUpdate(prevProps) {
        if (prevProps.params.Timestamp !== this.props.params.Timestamp) {
            this._getGridData();
        }
    }
    
    _getGridData() {
        this.props.toggleLoader(true);
        Report.fetchGridData(Url, { xsrfToken: this.props.xsrfToken, data: this.props.params })
            .then(response => {
                var result = response.data;

                var mainData = result.Data;
                if (mainData && mainData.length > 1) {
                    Service.sort(mainData, this.props.params.SortBy, this.props.params.SortOrder);

                    var sortedResult = Object.assign({}, result, { Data: mainData })

                    this.setState({ gridData: sortedResult })

                }

                this.props.setReportTitle(Service.getReportTitle(this.props.params.FromDate,
                    this.props.params.ToDate, this.props.params.CurrencyId,
                    this.props.params.CurrencyName, this.props.params.BaseCurrency));

                this.props.toggleLoader(false);

            })
            .catch(error => {
                Service.showErrorMsg();
                this.props.toggleLoader(false);
            });
    }
    _onExpandTurnOverColumn(e) {
       e.stopPropagation();
        this.setState((prevState) => ({ expandTurnover: !prevState.expandTurnover }));
    }
    _onExpandLicenseeColumn(e) {
        e.stopPropagation();
        this.setState((prevState) => ({ expandLicensee: !prevState.expandLicensee }));

    }
    _sort(sortBy) {
        this.props.toggleLoader(true);

        let param = Object.assign({}, this.props.params);
        var mainData = this.state.gridData.Data;
        if (!param.SortBy || param.SortBy != sortBy) {
            param.SortBy = sortBy;
            param.SortOrder = 'asc';
        }
        else {
            param.SortOrder = param.SortOrder == 'asc' ? 'desc' : 'asc';
        }

        if (mainData && mainData.length > 1) {

            Service.sort(mainData, param.SortBy, param.SortOrder);

            var sortedResult = Object.assign({}, this.state.gridData, { Data: mainData })

            this.setState({ gridData: sortedResult })
           
        }

        this.props.setParams(param);
        this.props.toggleLoader(false);

      
    }

    _buildHeader(selectedProductList, sortBy, sortOrder) {
        const productsCount = selectedProductList.length;
        const { expandLicensee, expandTurnover } = this.state;
        const rowSpan = productsCount > 1 && (expandLicensee || expandTurnover) ? 2 : 1;

        let rows = [
            <Row key='header1' style={{ height: 32 }}>
                <Cell colWidth={50} header={true} rowSpan={rowSpan}>{HeaderText.index}</Cell>
                <Cell colWidth={150} header={true} rowSpan={rowSpan} sortable={true} sorting={sortBy === 'Date'} asc={sortOrder === 'asc'} onClick={() => this._sort('Date')}>
                    {HeaderText.date}
                </Cell>
                <Cell colWidth={100} header={true} rowSpan={rowSpan} sortable={true} sorting={sortBy === 'CurrencyName'} asc={sortOrder === 'asc'} onClick={() => this._sort('CurrencyName')}>
                    {HeaderText.currency}
                </Cell>
                {
                    productsCount > 1
                        ? <Cell colWidth={200 * (productsCount * expandTurnover + 1)} header={true} rowSpan={expandTurnover ? 1 : rowSpan} colSpan={productsCount * expandTurnover + 1}
                            sortable={!expandTurnover} sorting={sortBy === 'TotalTurnOver'} asc={sortOrder === 'asc'} onClick={() => this._sort('TotalTurnOver')}>
                            {HeaderText.turnover}
                            <button className={`header-button btn ${expandTurnover ? 'btnCollapse' : ''}`} onClick={(e) => this._onExpandTurnOverColumn(e)}>
                                <i className='btn-icon icon-arrow'></i>
                            </button>
                        </Cell>
                        : <Cell colWidth={200} header={true} rowSpan={rowSpan} sortable={true} sorting={sortBy === 'TotalTurnOver'} asc={sortOrder === 'asc'} onClick={() => this._sort('TotalTurnOver')}>
                            {HeaderText.turnover}
                        </Cell>
                }
                <Cell colWidth={130} header={true} rowSpan={rowSpan} sortable={true} sorting={sortBy === 'TotalTransaction'} asc={sortOrder === 'asc'} onClick={() => this._sort('TotalTransaction')}>
                    {HeaderText.totalTransaction}
                </Cell>
                <Cell colWidth={200} header={true} rowSpan={rowSpan} sortable={true} sorting={sortBy === 'CustomerWinLoss'} asc={sortOrder === 'asc'} onClick={() => this._sort('CustomerWinLoss')}>
                    {HeaderText.customer}
                </Cell>
                {
                    productsCount > 1
                        ? <Cell colWidth={200 * (productsCount * expandLicensee + 1)} header={true} rowSpan={expandLicensee ? 1 : rowSpan} colSpan={productsCount * expandLicensee + 1}
                            sortable={!expandLicensee} sorting={sortBy === 'TotalLicenseeWL'} asc={sortOrder === 'asc'} onClick={() => this._sort('TotalLicenseeWL')}>
                            {HeaderText.licenseeWL}
                            <button className={`header-button btn ${expandLicensee ? 'btnCollapse' : ''}`} onClick={(e) => this._onExpandLicenseeColumn(e)}>
                                <i className='btn-icon icon-arrow'></i>
                            </button>
                        </Cell>
                        : <Cell colWidth={200} header={true} rowSpan={rowSpan} sortable={true} sorting={sortBy === 'TotalLicenseeWL'} asc={sortOrder === 'asc'} onClick={() => this._sort('TotalLicenseeWL')}>
                            {HeaderText.licenseeWL}
                        </Cell>
                }
            </Row>
        ];

        if (rowSpan > 1) {
            let cols = [];

            if (this.state.expandTurnover) {
                for (let i = 0; i < productsCount; i++) {
                    const code = selectedProductList[i];
                    const colName = `TurnOver${code}`;
                    cols.push(
                        <Cell key={colName} colWidth={200} header={true} sortable={true} sorting={sortBy === colName} asc={sortOrder === 'asc'} onClick={() => this._sort(colName)}>
                            {HeaderText[code]}
                        </Cell>
                    );
                }
                cols.push(
                    <Cell key='turnover-total' colWidth={200} header={true} sortable={true} sorting={sortBy === 'TotalTurnOver'} asc={sortOrder === 'asc'} onClick={() => this._sort('TotalTurnOver')}>
                        {HeaderText.total}
                    </Cell>
                );
            }

            if (this.state.expandLicensee) {
                for (let i = 0; i < productsCount; i++) {
                    const code = selectedProductList[i];
                    const colName = `LicenseeWL${code}`;
                    cols.push(
                        <Cell key={colName} colWidth={200} header={true} sortable={true} sorting={sortBy === colName} asc={sortOrder === 'asc'} onClick={() => this._sort(colName)}>
                            {HeaderText[code]}
                        </Cell>);
                }
                cols.push(
                    <Cell key='licenseeWL-total' colWidth={200} header={true} sortable={true} sorting={sortBy === 'TotalLicenseeWL'} asc={sortOrder === 'asc'} onClick={() => this._sort('TotalLicenseeWL')}>
                        {HeaderText.total}
                    </Cell>
                );
            }

            rows.push(
                <Row key='header2' style={{ height: 32 }}>
                    {cols}
                </Row>
            );
        }

        return rows;
    }

    _buildBody(reportBy, selectedProductList) {
        const data = this.state.gridData.Data,
            formatDate = reportBy === ReportBy.DAILY.toString() ? 'MM/DD/YYYY' : 'MM/YYYY';

        let rows = [];
        for (let i = 0; i < data.length; i++) {
            const item = data[i];
            const reportBy = moment(item.Date).format(formatDate);

            rows.push(
                <Row key={item.Ticks + item.CurrencyId}>
                    <Cell title={i + 1} className='center-align'>{i + 1}</Cell>
                    <Cell title={reportBy} className='center-align'>{reportBy}</Cell>
                    <Cell title={item.CurrencyName} className='center-align'>{item.CurrencyName}</Cell>
                    {this._buildCell(item, selectedProductList)}
                </Row>
            );
        }

        return rows;
    }

    _buildFooter(selectedProductList) {
        let totalRows = [];

        const total = this.state.gridData.Total,
            colSpan = 2,
            rowSpan = total.length;

        for (let i = 0; i < rowSpan; i++) {
            const item = total[i];
            totalRows.push(
                <Row key={item.CurrencyName} className='fixed-table-total'>
                    {i === 0 && <Cell title={TotalText.total} className='center-align' rowSpan={rowSpan} colSpan={colSpan}>{TotalText.total}</Cell>}
                    <Cell title={item.CurrencyName} className='center-align'>{item.CurrencyName}</Cell>
                    {this._buildCell(item, selectedProductList)}
                </Row>
            );
        }

        const dataSummary = this.state.gridData.SummaryTotal,
            baseCurrencyList = Object.keys(dataSummary),
            colSummarySpan = colSpan + 1;

        for (let i = baseCurrencyList.length - 1; i >= 0; i--) {
            const currency = baseCurrencyList[i],
                item = dataSummary[currency];
            totalRows.push(
                <Row key={`tr-grandtotal${i}`} className='fixed-table-grant-total'>
                    <Cell title={`${TotalText.totalIn} ${currency}`} className='center-align' colSpan={colSummarySpan}>{`${TotalText.totalIn} ${currency}`}</Cell>
                    {this._buildCell(item, selectedProductList)}
                </Row>
            );
        }

        return totalRows;
    }

    _buildCell(item, selectedProductList) {
        const turnover = NumberFormat.formatDecimal(item.TotalTurnOver),
            customerWl = NumberFormat.formatDecimal(item.CustomerWinLoss),
            licenseeWL = NumberFormat.formatDecimal(item.TotalLicenseeWL),
            totalTransaction = NumberFormat.format(item.TotalTransaction, 0);

        const productsCount = selectedProductList.length;
        if (productsCount === 1) {
            const rows = [
                <Cell key='turnover' title={turnover} className='right-align'>{turnover}</Cell>,
                <Cell key='totalTransaction' title={totalTransaction} className='right-align'>{totalTransaction}</Cell>,
                <Cell key='customerWl' title={customerWl} className='right-align'>{customerWl}</Cell>,
                <Cell key='licenseeWL' title={licenseeWL} className={`right-align ${item.TotalLicenseeWL < 0 ? 'negative' : ''}`}>{licenseeWL}</Cell>
            ];

            return rows;
        }

        let rows = [];

        if (this.state.expandTurnover) {
            for (let i = 0; i < productsCount; i++) {
                const column = 'TurnOver' + selectedProductList[i];
                const turnoverByProduct = NumberFormat.formatDecimal(item[column]);
                rows.push(<Cell key={column} title={turnoverByProduct} className='right-align'>{turnoverByProduct}</Cell>);
            }
        }

        rows.push(
            <Cell key='turnover' title={turnover} className='right-align'>{turnover}</Cell>,
            <Cell key='totalTransaction' title={totalTransaction} className='right-align'>{totalTransaction}</Cell>,
            <Cell key='customerWl' title={customerWl} className='right-align'>{customerWl}</Cell>
        );

        if (this.state.expandLicensee) {
            for (let i = 0; i < productsCount; i++) {
                const column = 'LicenseeWL' + selectedProductList[i];
                const licenseeWLByProduct = NumberFormat.formatDecimal(item[column]);
                rows.push(<Cell key={column} title={licenseeWLByProduct} className={`right-align ${item[column] < 0 ? 'negative' : ''}`}>{licenseeWLByProduct}</Cell>);
            }
        }

        rows.push(<Cell key='licenseeWL' title={licenseeWL} className={`right-align ${item.TotalLicenseeWL < 0 ? 'negative' : ''}`}>{licenseeWL}</Cell>);

        return rows;
    }

    render() {
        const data = this.state.gridData.Data;
        if (!data) {
            return null;
        }

        const params = this.props.params;

        if (!data.length) {
            return <NoDataTitle />;
        }

        return (
            <Table
                minWidth={800}
                autoWidth={true}
                bodyHeight={300}
                header={this._buildHeader(params.ProductCodeList, params.SortBy, params.SortOrder)}
                body={this._buildBody(params.ReportBy, params.ProductCodeList)}
                footer={this._buildFooter(params.ProductCodeList)}
            />
        );
    }
}
function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
}
function mapStateToProps(state) {
    const { params, xsrfToken } = state;
    return Object.assign({}, { params, xsrfToken });
}

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);