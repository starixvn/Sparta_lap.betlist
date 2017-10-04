import React from 'react';
import ReactDOM from 'react-dom';

export default class Loader extends React.Component {
    constructor(props) {
        super(props);
    };
    render() {
        return <div className="loading-process default" style={{ display: `${this.props.isLoading ? 'block' : 'none'}` }}>
            <div className="icon-loading">
                <div className="loader-inner ball-beat">
                    <div></div>
                    <div></div>
                    <div></div>
                </div>
            </div>
        </div>
    }
};
