import {Button, Container, Menu} from "semantic-ui-react";

export default function NavBar() {
    return (
        <Menu inverted fixed='top'>
            <Container>
                <Menu.Item header>
                    <img src='/assets/logo.png' alt='logo' style={{marginRight: 10}}/>
                    Tyre Management System
                </Menu.Item>
                <Menu.Item name='Tyres' />
                <Menu.Item name='Errors' />
                <Menu.Item>
                    <Button positive content='Create Tyre' />
                </Menu.Item>
            </Container>
        </Menu>
    )
}