import { Button, Icon, Item, Segment, Form } from 'semantic-ui-react';
import { ProductionRecord } from '../../models/ProductionRecord';
import { observer } from 'mobx-react-lite';
import { useStore } from '../../stores/store';
import { useState } from 'react';

interface Props {
  record: ProductionRecord;
}

export default observer(function ProductionRecordItem({ record }: Props) {
  const { userStore: { isQualitySupervisor }, recordStore } = useStore();
  const [isEditing, setIsEditing] = useState(false);
  const [updated, setUpdated] = useState({
    shift: record.shift,
    quantityProduced: record.quantityProduced,
  });

  function handleChange(
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) {
    const { name, value } = e.target;
    setUpdated((p) => ({ ...p, [name]: name === 'shift' ? Number(value) : Number(value) }));
  }

  async function handleSubmit() {
    await recordStore.updateRecord({
      id: record.id!,
      shift: updated.shift,
      quantityProduced: updated.quantityProduced,
      tyreId: record.tyreCode,
      machineId: record.machineNumber.toString(),
    });
    setIsEditing(false);
  }

  return (
    <Segment.Group>
      <Segment>
        <Item.Group>
          <Item>
            <Item.Content>
              <Item.Header>Machine #{record.machineNumber}</Item.Header>
              <Item.Meta>Tyre: {record.tyreCode}</Item.Meta>
              <Item.Description>Operator ID: {record.operatorId}</Item.Description>
            </Item.Content>
          </Item>
        </Item.Group>
      </Segment>

      <Segment>
        <span style={{ display: 'inline-flex', gap: 18 }}>
          <span>
            <Icon name="calendar" /> {record.productionDate?.toDateString()}
          </span>
          <span>
            <Icon name="clock" /> Shift {record.shift}
          </span>
          <span>
            <Icon name="boxes" /> Qty {record.quantityProduced}
          </span>
        </span>
      </Segment>

      {isQualitySupervisor && (
        <Segment clearing>
          <Button
            basic
            color="blue"
            floated="right"
            content={isEditing ? 'Cancel' : 'Edit'}
            onClick={() => setIsEditing((v) => !v)}
          />
        </Segment>
      )}

      {isEditing && (
        <Segment>
          <Form onSubmit={handleSubmit}>
            <Form.Group widths="equal">
              <Form.Input
                label="Shift"
                name="shift"
                type="number"
                value={updated.shift}
                onChange={handleChange}
              />
              <Form.Input
                label="Quantity"
                name="quantityProduced"
                type="number"
                value={updated.quantityProduced}
                onChange={handleChange}
              />
            </Form.Group>
            <Button positive content="Save" />
          </Form>
        </Segment>
      )}
    </Segment.Group>
  );
});
