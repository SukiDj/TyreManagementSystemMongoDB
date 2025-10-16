import { observer } from 'mobx-react-lite';
import { useStore } from '../../stores/store';
import { Segment, Header, Table, Button, Dropdown, Form, Icon } from 'semantic-ui-react';
import { useEffect, useState } from 'react';
import React from 'react';

export default observer(function ProductionList() {
  const { recordStore } = useStore();
  const [summaryType, setSummaryType] = useState<string | null>(null);
  const [selectedDate, setSelectedDate] = useState<string>('');
  const [selectedShift, setSelectedShift] = useState<number | null>(null);
  const [selectedMachineId, setSelectedMachineId] = useState<string>('');
  const [selectedOperatorId, setSelectedOperatorId] = useState<string>('');
  const { machineStore } = useStore();
  const { userStore } = useStore();

  useEffect(() => {
    recordStore.loadAllProductionRecords();
    machineStore.loadMachines();
    userStore.getAllOperators();
  }, [recordStore, machineStore, userStore]);

  const summaryOptions = [
    { key: 'day', text: 'Production by day', value: 'day' },
    { key: 'shift', text: 'Production by shift', value: 'shift' },
    { key: 'machine', text: 'Production by machine', value: 'machine' },
    { key: 'operator', text: 'Production by operator', value: 'operator' },
  ];

  const fetchSummary = async () => {
    if (!summaryType) return;
    switch (summaryType) {
      case 'day':
        if (selectedDate) await recordStore.loadProductionByDay(new Date(selectedDate));
        break;
      case 'shift':
        if (selectedShift !== null) await recordStore.loadProductionByShift(selectedShift);
        break;
      case 'machine':
        if (selectedMachineId) await recordStore.loadProductionByMachine(selectedMachineId);
        break;
      case 'operator':
        if (selectedOperatorId) await recordStore.loadProductionByOperator(selectedOperatorId);
        break;
    }
  };

  return (
    <>
      <Segment>
        <Header as="h4">
          <Icon name="filter" />
          <Header.Content>Filters</Header.Content>
        </Header>

        <Form>
          <Form.Group widths="equal">
            <Form.Field>
              <label>Summary</label>
              <Dropdown
                placeholder="Select summary"
                fluid
                selection
                options={summaryOptions}
                onChange={(_e, { value }) => setSummaryType(value as string)}
              />
            </Form.Field>

            {summaryType === 'day' && (
              <Form.Input
                label="Date"
                type="date"
                value={selectedDate}
                onChange={(e) => setSelectedDate(e.target.value)}
              />
            )}
            {summaryType === 'shift' && (
              <Form.Input
                label="Shift"
                type="number"
                value={selectedShift || ''}
                onChange={(e) => setSelectedShift(parseInt(e.target.value))}
              />
            )}
            {summaryType === 'machine' && (
              <Form.Field>
              <label>Machine</label>
              <Dropdown
                placeholder='Select Machine'
                fluid selection
                options={machineStore.machineOptions}
                value={selectedMachineId}
                onChange={(_e, { value }) => setSelectedMachineId(value as string)}
              />
              </Form.Field>
            )}
            {summaryType === 'operator' && (
              <Form.Field>
              <label>Operator</label>
              <Dropdown
                placeholder='Select Operator'
                fluid selection
                options={userStore.OperatorsOptions}
                value={selectedOperatorId}
                onChange={(_e, { value }) => setSelectedOperatorId(value as string)}
              />
              </Form.Field>
            )}
          </Form.Group>

          <Button primary onClick={fetchSummary} icon="search" content="Show" />
        </Form>
      </Segment>

      <Segment>
        <Table celled>
          <Table.Header>
            <Table.Row>
              <Table.HeaderCell>Machine</Table.HeaderCell>
              <Table.HeaderCell>Operator</Table.HeaderCell>
              <Table.HeaderCell>Tyre Code</Table.HeaderCell>
              <Table.HeaderCell>Quantity</Table.HeaderCell>
              <Table.HeaderCell>Date</Table.HeaderCell>
              <Table.HeaderCell>Shift</Table.HeaderCell>
            </Table.Row>
          </Table.Header>
          <Table.Body>
            {recordStore.groupedRecords.map(([date, records]) => (
              <React.Fragment key={date}>
                <Table.Row>
                  <Table.Cell colSpan={6}>
                    <Header as="h4" content={date} />
                  </Table.Cell>
                </Table.Row>

                {records.map((r) => (
                  <Table.Row key={r.id}>
                    <Table.Cell>{r.machineNumber}</Table.Cell>
                    <Table.Cell>{r.operatorId}</Table.Cell>
                    <Table.Cell>{r.tyreCode}</Table.Cell>
                    <Table.Cell>{r.quantityProduced}</Table.Cell>
                    <Table.Cell>{r.productionDate?.toDateString()}</Table.Cell>
                    <Table.Cell>{r.shift}</Table.Cell>
                  </Table.Row>
                ))}
              </React.Fragment>
            ))}
          </Table.Body>

        </Table>
      </Segment>
    </>
  );
});
