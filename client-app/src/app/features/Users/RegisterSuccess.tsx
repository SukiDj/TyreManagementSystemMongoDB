
import { Button, Header, Icon, Segment } from 'semantic-ui-react';
import { router } from '../../router/Routes';
import { observer } from 'mobx-react-lite';

export default observer( function RegisterSuccess() {    

    return (
        <Segment placeholder textAlign = 'center'>
            <Header icon color = 'green'>
                <Icon name = 'check'/>
                Uspesno ste se registrovali na sistem!
            </Header>
            <Button primary onClick={()=>router.navigate('/')} content = 'Login' size = 'huge' />
        </Segment>
    )
})