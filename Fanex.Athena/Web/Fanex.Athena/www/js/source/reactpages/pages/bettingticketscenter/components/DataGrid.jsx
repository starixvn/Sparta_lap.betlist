import React, { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import { ActionCreators } from '../actions'
import { Table, Row, Cell } from '../../../components/fixed-table-header'
import service from '../lib/service'
import ReportTitle from '../../../components/ReportTitle'

export class DataGrid extends Component {
    constructor(props) {
        super(props);
    }

    componentWillReceiveProps(nextProps) {
        if (this.props.gridData !== nextProps.gridData) {
            const { groupBy, selectedReportBy, selectedTimeZone, dateRange, selectedCurrency, selectedBaseCurrency, isConvertAll } = nextProps;
            this.groupBy = groupBy;
            this.selectedReportBy = selectedReportBy;
            this.selectedBaseCurrency = selectedBaseCurrency;
            this.reportTitle = "Betting Ticket Center";
            this.props.toggleLoader(false);
        }
    }

    _buildHeader() {
        var headerText = service.getHeaderText();
        return (
            <Row style={{ height: 32 }}>
                <Cell colWidth={40} header={true} title={headerText.Index}>{headerText.Index}</Cell>
                <Cell colWidth={100} header={true} title={headerText.Index}>{headerText.Platform}</Cell>
                <Cell colWidth={120} header={true} title={headerText.Index}>{headerText.ExternalId}</Cell>
                <Cell colWidth={120} header={true} title={headerText.Index}>{headerText.SystemId}</Cell>
                <Cell colWidth={180} header={true} title={headerText.Index}>{headerText.TransTime}</Cell>
                <Cell colWidth={250} header={true} title={headerText.Index}>{headerText.Choice}</Cell>
                <Cell colWidth={100} header={true} title={headerText.Index}>{headerText.Odds}</Cell>
                <Cell colWidth={100} header={true} title={headerText.Index}>{headerText.Stake}</Cell>
                <Cell colWidth={120} header={true} title={headerText.Index}>{headerText.WinLoss}</Cell>
                <Cell colWidth={120} header={true} title={headerText.Index}>{headerText.LicenseeWL}</Cell>
                <Cell colWidth={100} header={true} title={headerText.Index}>{headerText.Status}</Cell>
                <Cell colWidth={100} header={true} title={headerText.Index}>{headerText.Live}</Cell>
            </Row>
        )
    }

    _buildBody() {
                    const data = this.props.gridData.rows;
                    let rows = [];

                    for (let i = 0; i < data.length; i++) {
                        const item = data[i];
                        rows.push(
                            <Row key={`product-${i}`}>
                    <Cell className="center-align">{item.Index}</Cell>
                    <Cell>Platform 1</Cell>
                    <Cell>{item.ExternalId}</Cell>
                    <Cell>{item.SystemId}</Cell>
                    <Cell className="center-align" dangerouslySetInnerHTML={{ __html: item.TransTime }}></Cell>
                    <Cell className="right-align" dangerouslySetInnerHTML={{ __html: item.Choice}}></Cell>
                    <Cell className="right-align" dangerouslySetInnerHTML={{ __html: item.Odds}}></Cell>
                    <Cell className="right-align" dangerouslySetInnerHTML={{ __html: item.Stake}}></Cell>
                    <Cell className="right-align" dangerouslySetInnerHTML={{ __html: item.WinLoss}}></Cell>
                    <Cell className="right-align" dangerouslySetInnerHTML={{ __html: item.LicenseeWL}}></Cell>
                    <Cell className="center-align" dangerouslySetInnerHTML={{ __html: item.Status}}></Cell>
                    <Cell className="center-align">Live</Cell>
                </Row>
            );
                }

        return rows;
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
        
        const { rows } = this.props.gridData;
        if (!rows) {
            return null;
                }
        
        if (!rows.length) {
            return this._renderNodata();
                }

        return (
            <div className="betlist-container result-content">
                <ReportTitle content={this.reportTitle} />
                <Table
                    minWidth={600}
                    autoWidth={true}
                    bodyHeight={300}
                    header={this._buildHeader()}
                    body={this._buildBody()}
                />
            </div>
        );
  }
  }

function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
  }
function mapStateToProps(state) {
    const { gridData, groupBy, selectedReportBy, selectedTimeZone, dateRange, selectedCurrency, selectedBaseCurrency, isLoading, isConvertAll } = state;
    return Object.assign({}, { gridData, groupBy, selectedReportBy, selectedTimeZone, dateRange, selectedCurrency, selectedBaseCurrency, isLoading, isConvertAll });
  };

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);