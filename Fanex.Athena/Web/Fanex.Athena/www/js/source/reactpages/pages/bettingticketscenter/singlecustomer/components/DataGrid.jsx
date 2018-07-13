import React, { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import { ActionCreators } from '../actions'
import { Row, Cell } from '@jsroot/components/fixed-table-header'
import LangHelper from '../../../../lib/langHelper'
import NumberFormat from '@jsroot/lib/numberFormat'
import BaseDataGrid from '../../common/components/BaseDataGrid'
import Service from '../../common/lib/Service'

export class DataGrid extends BaseDataGrid {
    constructor(props) {
        super(props);
    }

    get dataUrl(){
        return ApiUrl.SINGLECUSTOMER_DATA_URL;
    }

    componentDidUpdate(prevProps, prevState) {
        if (this.props.gridData !== prevProps.gridData) {
            const { dateRange, selectedCurrency, selectedBaseCurrency, isConvertAll } = this.props;
            this.selectedBaseCurrency = selectedBaseCurrency;
            this.reportTitle = Service.getTitle(dateRange, null, isConvertAll ? selectedBaseCurrency : null);
            this.props.toggleLoader(false);
        }
    }

    _buildFooter() {
        const item = this.props.gridData.Total;
        const sumStake = NumberFormat.format(item.SumStake);
        const sumWinLoss = NumberFormat.format(item.SumWinLoss);
        const sumLicenseeWL= NumberFormat.format(item.SumLicenseeWL);
        
        return [(
            <Row className="fixed-table-grant-total" key={1}>
               <Cell className="center-align" colSpan={6}>{LangHelper.getSingleResource("Total")}</Cell>
               <Cell title={sumStake} className="right-align">{sumStake}</Cell>
               <Cell title={sumWinLoss} className="right-align">{sumWinLoss}</Cell>
               <Cell title={sumLicenseeWL} className="right-align">{sumLicenseeWL}</Cell>
               <Cell className="right-align" colSpan={3}></Cell>
            </Row>
            )]
    }
  }

function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
  }
function mapStateToProps(state) {
    const { requestParam, gridData, selectedBaseCurrency, isLoading, selectedCurrency, dateRange,isConvertAll } = state;
    return Object.assign({}, { requestParam, gridData, selectedBaseCurrency, isLoading, selectedCurrency, dateRange, isConvertAll });
  };

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);