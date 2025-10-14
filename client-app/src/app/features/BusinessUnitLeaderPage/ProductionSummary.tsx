import { observer } from 'mobx-react-lite';
import { useStore } from '../../stores/store';
import { Header, Segment, Table } from 'semantic-ui-react';
import { useEffect } from 'react';

export default observer(function ProductionSummary() {
  const { productionRecordStore } = useStore();

  useEffect(() => {
    productionRecordStore.loadAllProductionRecords(); // Poziva se funkcija za učitavanje svih zapisa o proizvodnji
  }, [productionRecordStore]);

  if (productionRecordStore.loadingInitial) return <div>Loading...</div>;

  return (
    <Segment>
      <Header as="h3" content="Production Summary" />
      <Table celled>
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell>Date</Table.HeaderCell>
            <Table.HeaderCell>Shift</Table.HeaderCell>
            <Table.HeaderCell>Machine</Table.HeaderCell>
            <Table.HeaderCell>Operator</Table.HeaderCell>
            <Table.HeaderCell>Total Produced</Table.HeaderCell>
          </Table.Row>
        </Table.Header>
        <Table.Body>
          {productionRecordStore.groupedRecords.map(([date, records]) => (
            <Table.Row key={date}>
              <Table.Cell>{date}</Table.Cell>
              <Table.Cell>{records.map(record => (
                <div key={record.id}>{record.shift}</div>
              ))}</Table.Cell>
              <Table.Cell>{records.map(record => (
                <div key={record.id}>{record.machineNumber}</div>
              ))}</Table.Cell>
              <Table.Cell>{records.map(record => (
                <div key={record.id}>{record.operatorId}</div>
              ))}</Table.Cell>
              <Table.Cell>{records.reduce((sum, record) => sum + record.quantityProduced, 0)}</Table.Cell>
            </Table.Row>
          ))}
        </Table.Body>
      </Table>
    </Segment>
  );
});
