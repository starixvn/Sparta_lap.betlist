import React, { Component, PropTypes } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux'
import { ActionCreators } from '../actions/index.js'
import DataGrid from './datagrid.jsx'
import FormFilter from './formFilter.jsx'
import AppLayout from '../../../layouts/AppLayout.jsx'
class AppContainer extends Component {

  constructor(props) {
    super(props);

  }
  componentDidMount() {
    console.log(this.props);
  }

  render() {
    return (
      <AppLayout>
        <div className="page-filter">
          <FormFilter />
        </div>

        <div className="page-content">
          <DataGrid />
        </div>
      </AppLayout>
    );
  }
}
function mapDispatchToProps(dispatch) {
  return bindActionCreators(ActionCreators, dispatch);
}

function mapStateToProps(state) {
  return Object.assign({}, state);
}


export default connect(mapStateToProps, mapDispatchToProps)(AppContainer);