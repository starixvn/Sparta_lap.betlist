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
        title: PropTypes.string,
        isShow: PropTypes.bool,
        onClose: PropTypes.func,
        isHtml: PropTypes.bool,
        iframeUrl: PropTypes.string,
    }

    static defaultProps = {
        width: DEFAULT_WIDTH,
        height: DEFAULT_HEIGHT,
        isShow: false,
        isHtml: false,
        iframeUrl: null
    }

    onCloseHandler = () => {
        this.props.onClose && this.props.onClose();
    }

    render() {
        const { title, isShow, width, height, isHtml, children, iframeUrl } = this.props;
        let content;
        if(iframeUrl)
        {
            content = <iframe src={iframeUrl} width="100%" height="100%" />;
        }
        else
        {                    
            content = isHtml ? 
            (<div className='popup-content' dangerouslySetInnerHTML={{ __html: children }} />) :
                (<div className='popup-content'>{children}</div>)
        }

        return (<div className={`popup-container ${isShow ? 'show' : 'hide'}`}>
            <div className='popup-block-body'></div>
            <div className='flat-popup' style={{ height: height, width: width }}>
                <div className='popup-title'>
                    <div className="txt-title">{title}</div>
                    <a className="popup-close" href="javascript:;" onClick={this.onCloseHandler}><i className="icon-close"></i></a>
                </div>
                { content }
            </div>
        </div>);
}
};