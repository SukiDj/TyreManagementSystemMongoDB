import { observer } from 'mobx-react-lite';
import { useStore } from '../../stores/store';
import { Header, Segment, Table, Form, Icon } from 'semantic-ui-react';
import { useState } from 'react';

export default observer(function StockBalanceReport() {
  const { stockRecordStore } = useStore();
  const [selectedDate, setSelectedDate] = useState<string>('');

  const handleDateChange = (e: any) => {
    const date = new Date(e.target.value);
    setSelectedDate(e.target.value);
    stockRecordStore.loadStockBalance(date);
  };

  return (
    <>
      <Segment>
        <Header as="h4">
          <Icon name="calendar" />
          <Header.Content>Pick date</Header.Content>
        </Header>

        <Form>
          <Form.Input
            label="Date"
            type="date"
            value={selectedDate}
            onChange={handleDateChange}
          />
        </Form>
      </Segment>

      <Segment>
        <Table celled>
          <Table.Header>
            <Table.Row>
              <Table.HeaderCell>Tyre Code</Table.HeaderCell>
              <Table.HeaderCell>Stock Balance</Table.HeaderCell>
            </Table.Row>
          </Table.Header>
          <Table.Body>
            {stockRecordStore.stockBalance.map((r) => (
              <Table.Row key={r.tyreCode}>
                <Table.Cell>{r.tyreCode}</Table.Cell>
                <Table.Cell>{r.stockBalance}</Table.Cell>
              </Table.Row>
            ))}
          </Table.Body>
        </Table>
      </Segment>
    </>
  );
});
