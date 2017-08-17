import React, { Component } from 'react';
import TableHeader from './components/TableHeader';
import TableContent from './components/TableContent';

class FixedTableHeader extends Component {
    constructor(props){
        super(props);
    }

    render() {
        const { headers, data, layoutConfig } = this.props.dataConfig;

        return (
            <div className="table-container">
                <TableHeader headers={headers} layoutConfig={layoutConfig} />
                <TableContent data={data} layoutConfig={layoutConfig} />
            </div>
        );
    }
}

export default FixedTableHeader;