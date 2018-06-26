import React from 'react';
import ReactDOM from 'react-dom';
import PropTypes from 'prop-types';

export default class CheckBox extends React.Component {
    constructor(props) {
        super(props);

        this.state = { checked: props.checked };
    };

    componentWillReceiveProps(nextProps) {
        if( this.props.checked !== nextProps.checked ) {
            this.onChange(nextProps.checked);
        }
    }

    static propTypes = {
        id: PropTypes.string.isRequired,
        label: PropTypes.string,
        checked: PropTypes.bool,
        disabled: PropTypes.bool,
        onChange: PropTypes.func
    }

    static defaultProps = {
        label: '',
        checked: true,
        disabled: false,
        onChange: function() {}
    }

    onChange = (newState) => {           
        this.setState({ checked: newState });

        this.props.onChange(newState);
    }

    render() {
        return <div className="checkbox-group">
                    <input type="checkbox" className="checkbox"                  
                        id={this.props.id}
                        checked={this.state.checked} 
                        disabled={this.props.disabled}
                        onChange={() => this.onChange(!this.state.checked)} />
                    <label htmlFor={this.props.id}>{this.props.label}</label>
                </div>
    }
};
