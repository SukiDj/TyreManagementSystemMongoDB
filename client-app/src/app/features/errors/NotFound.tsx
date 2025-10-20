import { Segment, Header, Icon, Button } from 'semantic-ui-react';
import { router } from '../../router/Routes';

export default function NotFound() {
  return (
    <Segment placeholder textAlign='center' style={{ marginTop: '2rem' }}>
      <Header icon color='grey'>
        <Icon name='question circle outline' />
        Not found
        <Header.Subheader>The resource you requested does not exist.</Header.Subheader>
      </Header>
      <Button onClick={() => router.navigate('/')} content='Back to Home' />
    </Segment>
  );
}
