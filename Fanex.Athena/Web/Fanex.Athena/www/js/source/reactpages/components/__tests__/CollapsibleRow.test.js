import React from 'react'
import renderer from 'react-test-renderer'

import CollapsibleRow from '../CollapsibleRow'

describe('CollapsibleRow ', () => {
    describe('render', () => {
        it('isExpand = true - component is expanded', () => { 
            const props = {
                isExpand: false
            };

            const component = renderer.create(<CollapsibleRow {...props} />);

            const result = component.toJSON(); 
        
            expect(result).toMatchSnapshot(); 
        });

        it('isExpand = false - component is collapsed', () => { 
            const props = {
                isExpand: false
            };

            const component = renderer.create(<CollapsibleRow {...props} />);

            const result = component.toJSON(); 
        
            expect(result).toMatchSnapshot(); 
        });

        it('isHtml = false - component render correctly - show raw content', () => { 
            const props = {
                isHtml: false,
                subject: 'Subject',
                content: '<p>Content</p>'
            };

            const component = renderer.create(<CollapsibleRow {...props} />);

            const result = component.toJSON(); 
        
            expect(result).toMatchSnapshot(); 
        });

        it('isHtml = true - component render correctly - show content with Html format', () => { 
            const props = {
                isHtml: true,
                subject: 'Subject',
                content: '<p>Content</p>'
            };

            const component = renderer.create(<CollapsibleRow {...props} />);

            const result = component.toJSON(); 
        
            expect(result).toMatchSnapshot();
        });
    });

    describe('componentWillReceiveProps', () => {
        it('props is not changed - state is not changed', () => { 
            const props = {
                isExpand: false
            };
            let component = new CollapsibleRow(props);
            component.setState = jest.fn();

            const newProps = {
                isExpand: false
            };
            component.componentWillReceiveProps(newProps);

            expect(component.setState).not.toHaveBeenCalled();
        });

        it('props is changed - state is changed', () => { 
            const props = {
                isExpand: false
            };
            let component = new CollapsibleRow(props);
            component.setState = jest.fn();

            const newProps = {
                isExpand: true
            };
            component.componentWillReceiveProps(newProps);

            expect(component.setState).toHaveBeenCalled();
        });
    });

    describe('onClick', () => {
        it('Class name is changed to show/hide content', () => {
            const props = {
                isHtml: false,
                subject: 'Subject',
                content: '<p>Content</p>'
            };

            const component = renderer.create(<CollapsibleRow {...props} />);
            
            let tree = component.toJSON();      
            expect(tree).toMatchSnapshot();

            // expand
            tree.children[0].children[1].props.onClick();           
            tree = component.toJSON();      
            expect(tree).toMatchSnapshot();

            // collapse
            tree.children[0].children[1].props.onClick();
            tree = component.toJSON();      
            expect(tree).toMatchSnapshot();
        });

        it('onExpand function is called when expanding', () => {
            const props = {
                isHtml: false,
                subject: 'Subject',
                content: '<p>Content</p>',
                onExpand: jest.fn(),
                isExpand: true
            };

            const component = renderer.create(<CollapsibleRow {...props} />);
            
            let tree = component.toJSON();      
            expect(tree).toMatchSnapshot();

            // collapse
            tree.children[0].children[1].props.onClick();
            expect(props.onExpand).not.toHaveBeenCalled();

            // expand
            tree.children[0].children[1].props.onClick();
            expect(props.onExpand).toHaveBeenCalled();
        });
    });
});