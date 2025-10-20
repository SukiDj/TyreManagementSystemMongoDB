import { observer } from 'mobx-react-lite';
import { useStore } from '../../stores/store';
import { Segment, Header, Table } from 'semantic-ui-react';
import { useEffect } from 'react';

export default observer(function SaleList() {
  const { saleRecordStore } = useStore(); 

  useEffect(() => {
    saleRecordStore.loadSaleRecords();
  }, [saleRecordStore]);

  if (saleRecordStore.loadingInitial) return <div>Loading...</div>; 

  return (
    <Segment>
      <Header as="h3" content="Sale Records" />
      <Table celled>
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell>Tyre Type</Table.HeaderCell>
            <Table.HeaderCell>Client</Table.HeaderCell>
            <Table.HeaderCell>Quantity Sold</Table.HeaderCell>
            <Table.HeaderCell>Price Per Unit</Table.HeaderCell>
            <Table.HeaderCell>Sale Date</Table.HeaderCell>
          </Table.Row>
        </Table.Header>
        <Table.Body>
          {saleRecordStore.groupedRecords.map(([date, records]) => (
            <Table.Row key={date}>
              <Table.Cell colSpan={5}>
                <Header as="h4" content={date} />
              </Table.Cell>
            </Table.Row>
          ))}
          {saleRecordStore.groupedRecords.map(([_, records]) =>
            records.map(record => (
              <Table.Row key={record.id}>
                <Table.Cell>{record.tyreType}</Table.Cell>
                <Table.Cell>{record.clientName}</Table.Cell>
                <Table.Cell>{record.quantitySold}</Table.Cell>
                <Table.Cell>{record.pricePerUnit}</Table.Cell>
                <Table.Cell>{record.saleDate?.toDateString()}</Table.Cell>
              </Table.Row>
            ))
          )}
        </Table.Body>
      </Table>
    </Segment>
  );
});
