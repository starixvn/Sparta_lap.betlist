import React, { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import { ActionCreators } from '../actions'
import { Row, Header, Cell } from '@jsroot/components/fixed-table-header'
import BaseDataGrid from '../../common/components/BaseDataGrid'
import Service from '../../common/lib/Service'
import ApiUrl from '../../common/constants/apiUrl'
import AppData from '@jsroot/lib/AppData'

export class DataGrid extends BaseDataGrid {
    constructor(props) {
        super(props);
    }
     
    get dataUrl(){
        return ApiUrl.REFNO_DATA_URL;
    }

    _buildHeader() {
        var headerText = Service.getHeaderText();
        const licenseeWLText = `${headerText.LicenseeWL}/${headerText.Comm}`;
        const winlossText = AppData.isAllowedCustomerComm ? `${headerText.WinLoss}/${headerText.Comm}` : headerText.WinLoss;

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
                <Header colWidth={100} title={headerText.Stake}>{headerText.Stake}</Header>
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