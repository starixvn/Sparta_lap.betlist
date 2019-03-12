import React from 'react';
import PropTypes from 'prop-types';

const Cell = (props) => {
    const { header, colWidth, sortable, sorting, asc, onClick, ...rest } = props;
    
    let headerSection = null;
    if(header) {
        let sortableHeader = null;
        if(sortable) {
            const upArrow = <div style={{display:'block', color: `${sorting && asc ? '#111':'#777'}`}} className={'asc'}></div>
            const downArrow = <div style={{display:'block', marginTop:'2px', color: `${sorting && !asc ? '#111':'#777'}`}} className={'desc'}></div>
            sortableHeader = (<th {...rest} className={'sortable sorting'} onClick={onClick}>                   
        <span>{props.children}</span>
                                <span style={{float: 'right', marginTop:'5px'}}>
                                {upArrow}
                                {downArrow}
                                </span>
                            </th>)
}

        headerSection = (sortable 
                    ? sortableHeader
                : <th {...rest}>{props.children}</th>)
}
     
    return header 
            ? headerSection
            : <td {...rest}>{props.children}</td>;
}

    
Cell.propTypes = {
    header: PropTypes.bool,
    sortable: PropTypes.bool,
    sorting: PropTypes.bool,
    asc: PropTypes.bool,
    onClick: PropTypes.func
}
 
Cell.defaultProps = {
    header: false,
    sortable: false,
    sorting: false,
    asc: true,
    onClick: function() {}
}

export default Cell;