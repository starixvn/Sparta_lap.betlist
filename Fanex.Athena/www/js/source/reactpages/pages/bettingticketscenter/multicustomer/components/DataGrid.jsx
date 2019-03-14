import React, { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import { ActionCreators } from '../actions'
import { HeaderRow, Row, HeaderCell, Cell } from '@9softstudio/react-grid'
import LangHelper from '../../../../lib/langHelper'
import BaseDataGrid from '../../common/components/BaseDataGrid'
import Service from '../../common/lib/Service'
import ApiUrl from '../../common/constants/apiUrl'
import AppData from '@jsroot/lib/AppData'

export class DataGrid extends BaseDataGrid {
    constructor(props) {
        super(props);
    }

    get dataUrl(){
        return ApiUrl.MULTICUSTOMER_DATA_URL;
    }

    _buildFooter() {
        const item = this.props.gridData.Total;
        const isNegativeWL = item.SumLicenseeWL.substring(0, 1) === '-';
        
        return [(
            <Row className="fixed-table-grant-total" key={1}>
               <Cell className="right-align" colSpan={6}>{LangHelper.getSingleResource("Total")}</Cell>
               <Cell className="right-align">{item.SumStake}</Cell>
               <Cell className="right-align" dangerouslySetInnerHTML={{ __html: item.SumWinLoss}}></Cell>
               <Cell className={`right-align ${isNegativeWL ? "negative" : ""}`}>{item.SumLicenseeWL}</Cell>
               <Cell className="right-align" colSpan={3}></Cell>
            </Row>
        )]
    }

    _buildHeader() {
        var headerText = Service.getHeaderText();
        const licenseeWLText = `${headerText.LicenseeWL}/${headerText.Comm}`;
        const winlossText = AppData.isAllowedCustomerComm ? `${headerText.WinLoss}/${headerText.Comm}` : headerText.WinLoss;

        const { SortBy, SortOrder } = this.props.requestParam;

        return (
            <HeaderRow style={{ height: 32 }}>
                <HeaderCell colWidth={40} header={true} title={headerText.Index}>{headerText.Index}</HeaderCell>               
                <HeaderCell colWidth={120} header={true} title={headerText.ExternalId}>{headerText.ExternalId}</HeaderCell>
                <HeaderCell colWidth={120} header={true} title={headerText.SystemId}>{headerText.SystemId}</HeaderCell>
                <HeaderCell colWidth={150} header={true} title={headerText.TransTime}>{headerText.TransTime}</HeaderCell>
                <HeaderCell colWidth={400} header={true} title={headerText.Choice}>{headerText.Choice}</HeaderCell>
                <HeaderCell colWidth={70} header={true} title={headerText.Odds}>{headerText.Odds}</HeaderCell>
                <HeaderCell colWidth={100} header={true} title={headerText.Stake} sortBy={'Stake'}>
                    {headerText.Stake}
                </HeaderCell>
                <HeaderCell colWidth={100} header={true} title={winlossText} sortBy={'WinLost'}>
                    {winlossText}
                </HeaderCell>
                <HeaderCell colWidth={100} header={true} title={licenseeWLText} sortBy={'LicenseeWL'}>
                    {licenseeWLText}
                </HeaderCell>
                <HeaderCell colWidth={80} header={true} title={headerText.Status}>{headerText.Status}</HeaderCell>
                <HeaderCell colWidth={80} header={true} title={headerText.Platform}>{headerText.Platform}</HeaderCell>
                <HeaderCell colWidth={80} header={true} title={headerText.Live}>{headerText.Live}</HeaderCell>
            </HeaderRow>
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