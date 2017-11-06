import React, { Component } from 'react'

import DataGrid from './DataGrid'
import FormFilter from './FormFilter'

export default class RefNo extends Component {
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