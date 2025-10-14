import { observer } from 'mobx-react-lite';
import { Container, Header, Segment } from 'semantic-ui-react';

export default observer (function ServerError() {
    //const {commonStore} = useStore();
    //console.log(commonStore.error?.details)
  return (
    <Container style={{marginTop:'10px'}}>
        <Header as='h1' content='Server Error' />
        <Header sub as ='h5' color='red' />
        {
            
                <Segment>
                    <Header as='h4' content='Stack trace' color='teal' />
                    <code style={{marginTop:'10px'}}>{}</code>
                </Segment>
            
        }
    </Container>
  )
})
