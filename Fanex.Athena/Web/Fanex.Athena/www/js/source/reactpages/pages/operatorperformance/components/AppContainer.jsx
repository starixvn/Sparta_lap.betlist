import React, { Component, PropTypes } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux'
import { ActionCreators }  from '../actions/index.js'
import * as DataGrid from './datagrid.jsx'
import * as FormFilter from './formFilter.jsx'
import AppLayout from '../../../layouts/AppLayout.jsx'
class AppContainer extends Component {

  constructor(props) {
    super(props);
    
  }

  render() {
   return (
    <AppLayout>
    <FormFilter/>
    <DataGrid/>
    </AppLayout>
    );
  }
}
function mapDispatchToProps(dispatch) {
  return bindActionCreators(ActionCreators, dispatch);
}

function mapStateToProps(state) {
  return {
   
  };
}


export default connect(mapStateToProps, mapDispatchToProps)(AppContainer);