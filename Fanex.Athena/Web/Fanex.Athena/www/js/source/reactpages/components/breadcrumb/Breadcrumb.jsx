import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Item from './BreadcrumbItem';

class BreadcrumbItem extends Component {
    constructor(props) {
        super(props);
        
        this.state = {
            value: this.props.value
        };
    }

    static propTypes = {
        items: PropTypes.arrayOf(PropTypes.object).isRequired,
        value: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
        onChange: PropTypes.func
    }

    onChange(item) {
        if (this.state.value === item.value) {
            return;
        }

        this.setState({
            value: item.value
        });

        this.props.onChange && this.props.onChange(item);
    }

    _renderItem() {
        const items = this.props.items;
        const value = this.state.value;
        let breadcrumbItems = [];

        for (let i = 0; i< items.length; i++) {
            const item = items[i];
            const isActive = item.value === value;

            breadcrumbItems.push(
                <Item key={`breadcrumb-item${i}`} isActive={isActive} label={item.label} onClick={this.onChange.bind(this, item)} />
            );

            if (isActive) {
                break;
            }
        }

        return breadcrumbItems;
    }

    render() {
        const items = this._renderItem();
        if (items.length === 1) {
            return null;
        }

        return (
            <div className='breadcrumb'>
                {items}
            </div>    
        );
    }
}

export default BreadcrumbItem;