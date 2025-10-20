import { useEffect, useState } from 'react';
import { Delivery } from '../../models/Delivery';
import { Button, Icon, List, Segment, Header, Label } from 'semantic-ui-react';
import { toast } from 'react-toastify';
import agent from '../../../api/agent';

export default function DeliveryList() {
  const [loading, setLoading] = useState(true);
  const [items, setItems] = useState<Delivery[]>([]);
  const [busyId, setBusyId] = useState<string | null>(null);

  const load = async () => {
    setLoading(true);
    try {
      const data = await agent.Deliveries.list();
      setItems(data);
      console.log(data);
      data.forEach(d => console.log(d.distanceMeters));
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { load(); }, []);

  const markDelivered = async (id: string) => {
    setBusyId(id);
    try {
      await agent.Deliveries.markDelivered(id);
      toast.success('Delivery marked as Delivered');
      setItems(prev => prev.map(d => d.id === id ? { ...d, status: 'Delivered' } : d));
    } finally {
      setBusyId(null);
    }
  };

  return (
    <Segment loading={loading}>
      <Header as='h4'>Deliveries</Header>
      <List divided relaxed>
        {items.map(d => (
          <List.Item key={d.id}>
            <List.Content floated='right'>
              <Button
                size='small'
                color='green'
                onClick={() => markDelivered(d.id)}
                disabled={d.status === 'Delivered'}
                loading={busyId === d.id}
                content={d.status === 'Delivered' ? 'Delivered' : 'Mark Delivered'}
              />
            </List.Content>
            <Icon name='truck' color='blue' />
            <List.Content>
              <List.Header>{d.client}</List.Header>
              <List.Description>
                {Math.round(d.distanceMeters)} m | Status: <Label color={d.status === 'Delivered' ? 'green' : 'orange'}>{d.status}</Label>
              </List.Description>
            </List.Content>
          </List.Item>
        ))}
      </List>
    </Segment>
  );
}
