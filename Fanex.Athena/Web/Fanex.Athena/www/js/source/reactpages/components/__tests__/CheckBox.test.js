import React from 'react'
import renderer from 'react-test-renderer'

import CheckBox from '../CheckBox'

describe('CheckBox ', () => {
    describe('render', () => {
        it('checked box is checked', () => {
            const props = { checked: true, id: "abc", label: "Hello" };

            const component = renderer.create(<CheckBox {...props} />);

            const result = component.toJSON();
            expect(result).toMatchSnapshot();
        });
    });

    describe('onChange', () => {
        it('set state correctly', () => {
            const newState = false;
            const component = new CheckBox ({onChange: jest.fn()});
            component.setState = jest.fn();

            component.onChange(newState);
            
            expect(component.setState).toHaveBeenCalledWith({ checked: newState });
        });
    });

    describe('componentWillReceiveProps', () => {
        it('call onChange() correctly', () => {
            const nextProps = { checked: true };
            const component = new CheckBox ({checked: false, onChange: jest.fn()});
            component.onChange = jest.fn();

            component.componentWillReceiveProps(nextProps);
            
            expect(component.onChange).toHaveBeenCalledWith(true);
        });
    });
});