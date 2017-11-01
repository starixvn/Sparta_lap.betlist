import React, { Component } from 'react'
import DataGrid from './datagrid'
import FormFilter from './formFilter'

export default class Notification extends Component {
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