import React, { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import { ActionCreators } from '../actions'
import { Row, Header, Cell } from '@jsroot/components/fixed-table-header'
import LangHelper from '@jsroot/lib/langHelper'
import BaseDataGrid from '../../common/components/BaseDataGrid'
import Service from '../../common/lib/Service'
import ApiUrl from '../../common/constants/apiUrl'
import AppData from '@jsroot/lib/AppData'

export class DataGrid extends BaseDataGrid {
    constructor(props) {
        super(props);
    }

    get dataUrl(){
        return ApiUrl.RUNNING_DATA_URL;
    }

    _buildFooter() {
        const item = this.props.gridData.Total;
        const colSpan = 8;
        return [(
            <Row className="fixed-table-grant-total" key={1}>
               <Cell className="right-align" colSpan={colSpan}>{LangHelper.getSingleResource("Total")}</Cell>
               <Cell className="right-align">{item.SumStake}</Cell>
               <Cell className="right-align" colSpan={5}></Cell>
            </Row>
            )]
    }

    _buildHeader() {
        var headerText = Service.getHeaderText();
        const licenseeWLText = `${headerText.LicenseeWL}/${headerText.Comm}`;
        const winlossText = AppData.isAllowedCustomerComm ? `${headerText.WinLoss}/${headerText.Comm}` : headerText.WinLoss;

        const { SortBy, SortOrder } = this.props.requestParam;

        return (
            <Row style={{ height: 32 }}>
                <Header colWidth={40} title={headerText.Index}>{headerText.Index}</Header>               
                <Header colWidth={120} title={headerText.ExternalId}>{headerText.ExternalId}</Header>
                <Header colWidth={120} title={headerText.SystemId}>{headerText.SystemId}</Header>
                <Header colWidth={150} title={headerText.TransTime}>{headerText.RefNo}</Header>
                <Header colWidth={150} title={headerText.TransTime}>{headerText.TransTime}</Header>
                <Header colWidth={400} title={headerText.Choice}>{headerText.Choice}</Header>
                <Header colWidth={70} title={headerText.Odds}>{headerText.Odds}</Header>
                <Header colWidth={70} title={headerText.Odds}>{headerText.OddsType}</Header>
                <Header colWidth={100} title={headerText.Stake} sortable={true} sorting={SortBy=='Stake'} asc={SortOrder=='asc'} onClick={()=>this._sort('Stake')}>
                    {headerText.Stake}
                </Header>
                <Header colWidth={100} title={winlossText}>{winlossText}</Header>
                <Header colWidth={100} title={licenseeWLText}>{licenseeWLText}</Header>
                <Header colWidth={80} title={headerText.Status}>{headerText.Status}</Header>
                <Header colWidth={80} title={headerText.Platform}>{headerText.Platform}</Header>
                <Header colWidth={80} title={headerText.Live}>{headerText.Live}</Header>
            </Row>
            )
        }
  }

function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
}
function mapStateToProps(state) {
    const { requestParam, gridData, reportTitle } = state;
    return Object.assign({}, { requestParam, gridData, reportTitle });
};

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);