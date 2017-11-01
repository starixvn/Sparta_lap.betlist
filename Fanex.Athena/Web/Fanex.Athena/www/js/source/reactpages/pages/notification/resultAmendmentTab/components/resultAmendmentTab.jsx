import React, { Component } from 'react';
import DataGrid from './datagrid'
import FormFilter from './formFilter'

export default class ResultAmendmentTab extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    return (
      <div>
        <FormFilter />
        <DataGrid />
      </div>
    );
  }
}