import { observer } from 'mobx-react-lite';
import { useStore } from '../../stores/store';
import { Segment, Header, Table, Button, Dropdown, Form } from 'semantic-ui-react';
import { useEffect, useState } from 'react';

export default observer(function ProductionList() {
  const { recordStore } = useStore(); // Koristimo `recordStore`
  const [summaryType, setSummaryType] = useState<string | null>(null); // Drži trenutno odabranu opciju
  const [selectedDate, setSelectedDate] = useState<string>(''); // Za datum
  const [selectedShift, setSelectedShift] = useState<number | null>(null); // Za smenu
  const [selectedMachineId, setSelectedMachineId] = useState<string>(''); // Za ID mašine
  const [selectedOperatorId, setSelectedOperatorId] = useState<string>(''); // Za ID operatera

  useEffect(() => {
    recordStore.loadAllProductionRecords(); // Učitaj sve proizvodne zapise
  }, [recordStore]);

  const summaryOptions = [
    { key: 'day', text: 'Production by day', value: 'day' },
    { key: 'shift', text: 'Production by shift', value: 'shift' },
    { key: 'machine', text: 'Production by machine', value: 'machine' },
    { key: 'operator', text: 'Production by operator', value: 'operator' },
  ];

  const handleSummaryChange = (e: any, { value }: any) => {
    setSummaryType(value); // Postavlja odabranu opciju
  };

  const handleShowSummary = async () => {
    if (!summaryType) return; // Ako nije izabran tip, ne radi ništa

    switch (summaryType) {
      case 'day':
        if (selectedDate) {
          await recordStore.loadProductionByDay(new Date(selectedDate));
        }
        break;
      case 'shift':
        if (selectedShift !== null) {
          await recordStore.loadProductionByShift(selectedShift);
        }
        break;
      case 'machine':
        if (selectedMachineId) {
          await recordStore.loadProductionByMachine(selectedMachineId);
        }
        break;
      case 'operator':
        if (selectedOperatorId) {
          await recordStore.loadProductionByOperator(selectedOperatorId);
        }
        break;
      default:
        break;
    }
  };

  if (recordStore.loadingInitial) return <div>Loading...</div>;

  return (
    <Segment>
      <Header as="h3" content="Production Records" />
      <Dropdown
        placeholder="Select Production Summary"
        fluid
        selection
        options={summaryOptions}
        onChange={handleSummaryChange}
      />

      {summaryType === 'day' && (
        <Form.Field>
          <label>Production Date</label>
          <input
            type="date"
            value={selectedDate}
            onChange={(e) => setSelectedDate(e.target.value)}
          />
        </Form.Field>
      )}
      {summaryType === 'shift' && (
        <Form.Field>
          <label>Shift Number</label>
          <input
            type="number"
            value={selectedShift || ''}
            onChange={(e) => setSelectedShift(parseInt(e.target.value))}
          />
        </Form.Field>
      )}
      {summaryType === 'machine' && (
        <Form.Field>
          <label>Machine ID</label>
          <input
            type="text"
            value={selectedMachineId}
            onChange={(e) => setSelectedMachineId(e.target.value)}
          />
        </Form.Field>
      )}
      {summaryType === 'operator' && (
        <Form.Field>
          <label>Operator ID</label>
          <input
            type="text"
            value={selectedOperatorId}
            onChange={(e) => setSelectedOperatorId(e.target.value)}
          />
        </Form.Field>
      )}

      <Button onClick={handleShowSummary} primary>
        Show Production Summary
      </Button>

      <Table celled>
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell>Machine Number</Table.HeaderCell>
            <Table.HeaderCell>Operator</Table.HeaderCell>
            <Table.HeaderCell>Tyre Code</Table.HeaderCell>
            <Table.HeaderCell>Quantity Produced</Table.HeaderCell>
            <Table.HeaderCell>Production Date</Table.HeaderCell>
            <Table.HeaderCell>Shift</Table.HeaderCell>
          </Table.Row>
        </Table.Header>
        <Table.Body>
          {recordStore.groupedRecords.map(([date, records]) => (
            <>
              <Table.Row key={date}>
                <Table.Cell colSpan={6}>
                  <Header as="h4" content={date} />
                </Table.Cell>
              </Table.Row>
              {records.map((record) => (
                <Table.Row key={record.id}>
                  <Table.Cell>{record.machineNumber}</Table.Cell>
                  <Table.Cell>{record.operatorId}</Table.Cell>
                  <Table.Cell>{record.tyreCode}</Table.Cell>
                  <Table.Cell>{record.quantityProduced}</Table.Cell>
                  <Table.Cell>{record.productionDate?.toDateString()}</Table.Cell>
                  <Table.Cell>{record.shift}</Table.Cell>
                </Table.Row>
              ))}
            </>
          ))}
        </Table.Body>
      </Table>
    </Segment>
  );
});
