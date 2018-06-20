import React, { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import { ActionCreators } from '../actions'
import LangHelper from '../../../../lib/langHelper'
import BaseDataGrid from '../../common/components/BaseDataGrid'

export class DataGrid extends BaseDataGrid {
    constructor(props) {
        super(props);
    }

    componentWillReceiveProps(nextProps) {
        if (this.props.gridData !== nextProps.gridData) {
             const { selectedBaseCurrency } = nextProps;
              this.selectedBaseCurrency = selectedBaseCurrency;
              this.reportTitle = this.getReportTitle(selectedBaseCurrency);
            this.props.toggleLoader(false);
        }
    }

    getReportTitle(baseCurrency) {
        const baseCurrencyText = LangHelper.getSingleResource("BaseCurrency") + ": " + baseCurrency;
        return `${LangHelper.getSingleResource("BettingTicketCenter")}, ${baseCurrencyText}`;
    }
  }

function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
  }
function mapStateToProps(state) {
    const { gridData, selectedBaseCurrency, isLoading, isConvertAll } = state;
    return Object.assign({}, { gridData, selectedBaseCurrency, isLoading, isConvertAll });
  };

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);