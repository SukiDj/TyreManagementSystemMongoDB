import { observer } from 'mobx-react-lite';
import { Container, Dropdown, Icon, Menu } from 'semantic-ui-react';
import { useStore } from '../stores/store';

export default observer(function NavBar() {
  const { userStore } = useStore();
  const user = userStore.user;

  return (
    <Menu fixed='top' inverted className="app-navbar">
      <Container>
        <Menu.Item>
          <Icon name='tint' style={{ marginRight: 6 }} />
          Tyre Management
        </Menu.Item>

        {user ? (
        <Menu.Item position='right'>
            <Menu.Menu position='right'>
                <Dropdown pointing='top right' text={user.userName ?? 'Account'}>
                <Dropdown.Menu>
                    <Dropdown.Item onClick={userStore.loguot}>
                    <Icon name='sign out' /> Logout
                    </Dropdown.Item>
                </Dropdown.Menu>
                </Dropdown>

            </Menu.Menu>
        </Menu.Item>
        ) : null}

      </Container>
    </Menu>
  );
});