import React, { Component } from 'react';
import PropTypes from 'prop-types';

export default class Page extends Component {
    constructor(props) {
        super(props);

        this.state = {
            index: this.props.pageIndex
        };
        
        this.totalPage = Math.ceil(this.props.totalItem/this.props.pageSize);
    }

    static propTypes = {
        pageIndex: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
        pageSize: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
        pageList: PropTypes.arrayOf(PropTypes.oneOfType([PropTypes.number, PropTypes.string])),
        totalItem: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
        onPaging: PropTypes.func
    };

    static defaultProps = {
        pageIndex: 1,
        pageSize: 50,
        pageList: [50, 100, 200],
        totalItem: 0
    };

    goToPage(index, size) {
        this.props.onPaging && this.props.onPaging(index, size);
    }

    onKeyUpHandle = (e) => {
        const code =  e.keyCode ? e.keyCode : e.which;
        if (code === 13) {
            var value = parseInt(this.state.index);
            if (value > 0 && value <= this.totalPage) {
                this.goToPage(value, this.props.pageSize);
            }           
        }
    } 

    _renderPageSizeSelection(pageIndex, pageSize, pageList) {
        return (
            <select className="pageSizeSelected" value={pageSize} onChange={(e) => this.goToPage(pageIndex, e.target.value)}>
                {this._renderPageSizeOption(pageList)}
	        </select>
        );
    }

    _renderPageSizeOption(pageList) {
        return pageList.map((item, index) => {
            return (<option key={`pagesize${index}`} value={item}>{item}</option>);
        });
    }

    _renderPageIcon(iconClass, isDisable, onClickFunc) {
        return (<a href="javascript:;" className={`icon ${iconClass} ${isDisable ? "pagingDisable" : ""}`} onClick={onClickFunc}></a>);
    }

    _renderPageSizeInput() {
        return (
            <div className="paging-info">Page <input type="text" className="paging-form" value={this.state.index} maxLength="4" size="2" 
                        onChange={(e) => { this.setState({ index: e.target.value }); }}
                        onKeyUp={this.onKeyUpHandle}/>
                {` of ${this.totalPage}`}
            </div>);
    }

    render() {
        const { pageIndex, pageSize, pageList } = this.props;

        return this.totalPage > 1 ? (
            <div className="pagingContainer center-align">
                {this._renderPageIcon("pagingFirst", pageIndex <= 1, () => this.goToPage(1, pageSize))}
                {this._renderPageIcon("pagingPrev", pageIndex <= 1, () => this.goToPage(pageIndex - 1, pageSize))}
                {this._renderPageSizeInput()}
                {this._renderPageIcon("pagingNext", pageIndex >= this.totalPage, () => this.goToPage(pageIndex + 1, pageSize))}
                {this._renderPageIcon("pagingLast", pageIndex >= this.totalPage, () => this.goToPage(this.totalPage, pageSize))}
                {this._renderPageSizeSelection(pageIndex, pageSize, pageList)}
            </div>
        ) : "";
    }
}