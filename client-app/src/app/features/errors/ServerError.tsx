import { Segment, Header, Icon, Button } from 'semantic-ui-react';
import { router } from '../../router/Routes';

export default function ServerError() {
  return (
    <Segment placeholder textAlign='center' style={{ marginTop: '2rem' }}>
      <Header icon color='red'>
        <Icon name='server' />
        Server error
        <Header.Subheader>Something went wrong on our side.</Header.Subheader>
      </Header>
      <Button primary onClick={() => router.navigate('/')} content='Back to Home' />
    </Segment>
  );
}
