import React, { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import { ActionCreators } from '../actions'
import { Row, Cell } from '@jsroot/components/fixed-table-header'
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
        const colSpan = 6;
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
                <Cell colWidth={40} header={true} title={headerText.Index}>{headerText.Index}</Cell>               
                <Cell colWidth={120} header={true} title={headerText.ExternalId}>{headerText.ExternalId}</Cell>
                <Cell colWidth={120} header={true} title={headerText.SystemId}>{headerText.SystemId}</Cell>
                <Cell colWidth={150} header={true} title={headerText.TransTime}>{headerText.TransTime}</Cell>
                <Cell colWidth={400} header={true} title={headerText.Choice}>{headerText.Choice}</Cell>
                <Cell colWidth={70} header={true} title={headerText.Odds}>{headerText.Odds}</Cell>
                <Cell colWidth={100} header={true} title={headerText.Stake} sortable={true} sorting={SortBy=='Stake'} asc={SortOrder=='asc'} onClick={()=>this._sort('Stake')}>
                    {headerText.Stake}
                </Cell>
                <Cell colWidth={100} header={true} title={winlossText}>{winlossText}</Cell>
                <Cell colWidth={100} header={true} title={licenseeWLText}>{licenseeWLText}</Cell>
                <Cell colWidth={80} header={true} title={headerText.Status}>{headerText.Status}</Cell>
                <Cell colWidth={80} header={true} title={headerText.Platform}>{headerText.Platform}</Cell>
                <Cell colWidth={80} header={true} title={headerText.Live}>{headerText.Live}</Cell>
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