import React, { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import { ActionCreators } from '../actions'
import { Row, Cell } from '@jsroot/components/fixed-table-header'
import LangHelper from '@jsroot/lib/langHelper'
import NumberFormat from '@jsroot/lib/numberFormat'
import BaseDataGrid from '../../common/components/BaseDataGrid'
import Service from '../../common/lib/Service'
import ApiUrl from '../../common/constants/apiUrl'

export class DataGrid extends BaseDataGrid {
    constructor(props) {
        super(props);
    }

    componentDidUpdate(prevProps, prevState) {
        if (this.props.gridData !== prevProps.gridData) {
            const { dateRange, selectedCurrency, selectedBaseCurrency } = this.props;
              this.selectedBaseCurrency = selectedBaseCurrency;
              this.reportTitle = Service.getTitle(dateRange, selectedCurrency.label, selectedBaseCurrency);;
            this.props.toggleLoader(false);
        }
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

  }

function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
  }
function mapStateToProps(state) {
    const { requestParam, gridData, selectedBaseCurrency, isLoading, selectedCurrency, dateRange } = state;
    return Object.assign({}, { requestParam, gridData, selectedBaseCurrency, isLoading, selectedCurrency, dateRange });
  };

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);