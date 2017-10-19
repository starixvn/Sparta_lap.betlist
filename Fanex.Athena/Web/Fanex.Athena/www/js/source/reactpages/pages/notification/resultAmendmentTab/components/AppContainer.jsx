import React, { Component } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux'
import { ActionCreators } from '../actions/index'
import DataGrid from './datagrid'
import FormFilter from './formFilter'
import Loader from '../../../../components/Loader'
import AppLayout from '../../../../layouts/AppLayout'
import Tab from '../../../../components/Tab'

export class AppContainer extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <AppLayout>
        <Loader isLoading={this.props.isLoading} />
        <Tab tabList={this.props.tabList} />
        <FormFilter />
        <DataGrid />
      </AppLayout>
    );
  }
}
function mapDispatchToProps(dispatch) {
  return bindActionCreators(ActionCreators, dispatch);
}

function mapStateToProps(state) {
    return Object.assign({}, { isLoading: state.isLoading, tabList: state.tabList });
}

export default connect(mapStateToProps, mapDispatchToProps)(AppContainer);