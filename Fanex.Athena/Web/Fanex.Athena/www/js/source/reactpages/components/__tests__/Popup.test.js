import React from 'react'
import renderer from 'react-test-renderer'

import Popup from '../popup/Popup'

describe('Popup ', () => {
    describe('render', () => {
        it('isShow is false - popup component is hidden', () => {
            const props = {
                content: '<div></div>'
            };

            const component = renderer.create(<Popup {...props} />);

            const result = component.toJSON();
        
            expect(result).toMatchSnapshot();
            expect(result.props.className).toContain('hide');
        });

        it('isShow is true - popup component is shown', () => {
            const props = {
                content: '<div></div>',
                isShow: true
            };

            const component = renderer.create(<Popup {...props} />);

            const result = component.toJSON();
        
            expect(result).toMatchSnapshot();
            expect(result.props.className).toContain('show');
        });

        it('isHtml is false - popup component shows content as text', () => {
            const props = {
                content: '<div></div>',
                isShow: true
            };

            const component = renderer.create(<Popup {...props} />);

            const result = component.toJSON();
        
            expect(result).toMatchSnapshot();
        });

        it('isHtml is true - popup component shows content as html', () => {
            const props = {
                content: '<div></div>',
                isShow: true,
                isHtml: true
            };

            const component = renderer.create(<Popup {...props} />);

            const result = component.toJSON();
        
            expect(result).toMatchSnapshot();
        });
    });

    describe('onCloseHandler', () => {
        it('onClick prop is called', () => {
            const props = {
                onClose: jest.fn()
            };
            const component = new Popup(props);

            component.onCloseHandler();

            expect(props.onClose).toHaveBeenCalled();
        });
    });
});