import { Link } from 'react-router-dom'
import { Button, Header, Icon, Segment } from 'semantic-ui-react'

export default function NotFound() {
  return (
    <Segment placeholder>
        <Header icon>
            <Icon name='search'/>
            Nazalost nismo nasli ono sto trazite!
        </Header>
        <Segment.Inline>
            <Button as={Link} to='/'>
                Nazad na pocetnu stranicu
            </Button>
        </Segment.Inline>
    </Segment>
  )
}
