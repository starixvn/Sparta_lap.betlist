import React, { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import { ActionCreators } from '../actions'
import LangHelper from '../../../../lib/langHelper'
import BaseDataGrid from '../../common/components/BaseDataGrid'
import Service from '../../common/lib/Service'

export class DataGrid extends BaseDataGrid {
    constructor(props) {
        super(props);
    }

    componentWillReceiveProps(nextProps) {
        if (this.props.gridData !== nextProps.gridData) {
            const { dateRange, selectedCurrency, selectedBaseCurrency } = nextProps;
              this.selectedBaseCurrency = selectedBaseCurrency;
              this.reportTitle = Service.getTitle(dateRange, selectedCurrency.label, selectedBaseCurrency);;
            this.props.toggleLoader(false);
        }
    }
  }

function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
  }
function mapStateToProps(state) {
    const { gridData, selectedBaseCurrency, isLoading, selectedCurrency, dateRange } = state;
    return Object.assign({}, { gridData, selectedBaseCurrency, isLoading, selectedCurrency, dateRange });
  };

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);