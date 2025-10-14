import { observer } from 'mobx-react-lite';
import { useStore } from '../../stores/store';
import { Header, Segment, Table, Form, Dropdown } from 'semantic-ui-react';
import { useEffect, useState } from 'react';

export default observer(function StockBalanceReport() {
  const { tyreStore, stockRecordStore } = useStore();
  const [selectedDate, setSelectedDate] = useState<Date | null>(null);

  useEffect(() => {
    tyreStore.loadTyres();
  }, [tyreStore]);

  const handleDateChange = (e: any) => {
    const date = new Date(e.target.value);
    setSelectedDate(date);
    stockRecordStore.loadStockBalance(date);
  };

  return (
    <Segment>
      <Header as="h3" content="Stock Balance Report" />
      <Form>
        <Form.Field>
          <label>Select Date</label>
          <input type="date" onChange={handleDateChange} />
        </Form.Field>
      </Form>
      <Table celled>
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell>Tyre Code</Table.HeaderCell>
            <Table.HeaderCell>Stock Balance</Table.HeaderCell>
          </Table.Row>
        </Table.Header>
        <Table.Body>
          {stockRecordStore.stockBalance.map((record) => (
            <Table.Row key={record.tyreCode}>
              <Table.Cell>{record.tyreCode}</Table.Cell>
              <Table.Cell>{record.stockBalance}</Table.Cell>
            </Table.Row>
          ))}
        </Table.Body>
      </Table>
    </Segment>
  );
});
