import React, { Component } from 'react';

class RowLayout extends Component {
    constructor(props){
        super(props)
    }

    render() {
        console.log("Layout config", this.props.layoutConfig);
        const width = `style="width: ${this.props.layoutConfig[0]}px"`;
        console.log(width);

        const cells =  this.props.layoutConfig.map((cellWidth, index) => {
            return (<td key={`cellLayout${index}`} style={{width: cellWidth + 'px'}}></td>);
        });

        return(           
            <tr>
               {cells}
            </tr>
        );
    }
}

export default RowLayout;