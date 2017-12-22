import React from 'react';
import ReactDOM from 'react-dom';
import { DEFAULT_WIDTH, DEFAULT_HEIGHT} from './constants';
import PropTypes from 'prop-types';

export default class Popup extends React.Component {
    constructor(props) {
        super(props);
    };

    static propTypes = {
        width: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
        height: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
        content: PropTypes.string,
        title: PropTypes.string,
        isShow: PropTypes.bool,
        onClose: PropTypes.func,
        isHtml: PropTypes.bool
    }

    static defaultProps = {
        width: DEFAULT_WIDTH,
        height: DEFAULT_HEIGHT,
        isShow: false,
        isHtml: false
    }

    onCloseHandler = () => {
        this.props.onClose && this.props.onClose();
    }

    render() {
        const { content, title, isShow, width, height, isHtml } = this.props;

        return (<div className={`popup-container ${isShow ? 'show' : 'hide'}`}>
            <div className='popup-block-body'></div>
            <div className='flat-popup' style={{ height: height, width: width }}>
                <div className='popup-title'>
                    <div className="txt-title">{title}</div>
                    <a className="popup-close" href="javascript:;" onClick={this.onCloseHandler}><i className="icon-close"></i></a>
                </div>
                { isHtml
                    ? (<div className='popup-content' dangerouslySetInnerHTML={{ __html: content }} />)
                    : (<div className='popup-content'>{content}</div>)
                }
            </div>
        </div>);
    }
};