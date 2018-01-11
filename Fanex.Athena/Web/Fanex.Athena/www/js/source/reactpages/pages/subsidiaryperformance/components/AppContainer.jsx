import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux'
import { ActionCreators } from '../actions/index'
import DataGrid from './DataGrid'
import FormFilter from './FormFilter'
import Loader from '../../../components/Loader'
import AppLayout from '../../../layouts/AppLayout'
import ReportTitle from '../../../components/ReportTitle'
import service from '../lib/service'

export class AppContainer extends Component {
  constructor(props) {
    super(props);
    this.reportTitle = null;
  }

  componentWillReceiveProps(nextProps) {
    if (this.props.isLoading && !nextProps.isLoading) {
      const { dateRange, selectedBaseCurrency } = nextProps;
      this.reportTitle = service.getReportTitle(dateRange, selectedBaseCurrency);
    }
  }

  render() {
    return (
      <AppLayout>
        <Loader isLoading={this.props.isLoading} />
        <FormFilter />
        <div className="result-content">
          {this.reportTitle && <ReportTitle content={this.reportTitle} />}
          <DataGrid />
        </div>
      </AppLayout>
    );
  }
}

function mapStateToProps(state) {
  const { isLoading, dateRange, selectedBaseCurrency } = state;
  return { isLoading, dateRange, selectedBaseCurrency };
}

export default connect(mapStateToProps)(AppContainer);