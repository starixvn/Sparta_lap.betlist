import React, { Component } from 'react'
import { connect } from 'react-redux'
import { bindActionCreators } from 'redux'

import { ActionCreators } from '../actions'
import LangHelper from '../../../../lib/langHelper'
import BaseDataGrid from '../../common/components/BaseDataGrid'
import Service from '../../common/lib/Service'
import ApiUrl from '../../common/constants/apiUrl'

export class DataGrid extends BaseDataGrid {
    constructor(props) {
        super(props);
    }

    componentDidUpdate(prevProps, prevState) {
        if (this.props.gridData !== prevProps.gridData) {
            const { isConvertAll, selectedBaseCurrency } = this.props;
            this.selectedBaseCurrency = selectedBaseCurrency;
            this.reportTitle = Service.getTitle(null, null, isConvertAll ? selectedBaseCurrency : null);
            this.props.toggleLoader(false);
        }
    }
     
    get dataUrl(){
        return ApiUrl.REFNO_DATA_URL;
    }
  }

function mapDispatchToProps(dispatch) {
    return bindActionCreators(ActionCreators, dispatch);
  }
function mapStateToProps(state) {
    const { requestParam, gridData, selectedBaseCurrency, isLoading, isConvertAll } = state;
    return Object.assign({}, { requestParam, gridData, selectedBaseCurrency, isLoading, isConvertAll });
  };

export default connect(mapStateToProps, mapDispatchToProps)(DataGrid);