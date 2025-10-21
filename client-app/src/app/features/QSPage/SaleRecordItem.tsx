import { Button, Icon, Item, Segment, Form } from 'semantic-ui-react';
import { SaleRecord, SaleRecordFromValues } from '../../models/SaleRecord';
import { observer } from 'mobx-react-lite';
import { useStore } from '../../stores/store';
import { useState } from 'react';

interface Props {
  record: SaleRecord;
}

export default observer(function SaleRecordItem({ record }: Props) {
  const { userStore: { isQualitySupervisor }, saleRecordStore } = useStore();

  const [isEditing, setIsEditing] = useState(false);
  const [updated, setUpdated] = useState({
    pricePerUnit: record.pricePerUnit,
    quantitySold: record.quantitySold
  });

  function handleChange(e: React.ChangeEvent<HTMLInputElement>) {
    const { name, value } = e.target;
    if (name === 'pricePerUnit') {
      const n = Number(value);
      if (Number.isFinite(n)) setUpdated(p => ({ ...p, pricePerUnit: n }));
    } else if (name === 'quantitySold') {
      const n = Math.trunc(Number(value));
      if (Number.isFinite(n)) setUpdated(p => ({ ...p, quantitySold: n }));
    }
  }

  async function handleSubmit() {
    const payload: SaleRecordFromValues = {
      tyreId: record.tyreCode,
      clientId: record.clientId,
      productionOrderId: record.productionOrderId,
      targetMarket: record.targetMarket,
      unitOfMeasure: record.unitOfMeasure,
      pricePerUnit: updated.pricePerUnit,
      quantitySold: updated.quantitySold
    };

    await saleRecordStore.updateRecord(record.id!, payload);
    setIsEditing(false);
  }

  async function handleDelete() {
    await saleRecordStore.deleteSaleRecord(record.id!);
  }

  return (
    <Segment.Group>
      <Segment>
        <Item.Group>
          <Item>
            <Item.Content>
              <Item.Header>Tyre: {record.tyreType}</Item.Header>
              <Item.Meta>Tyre Code: {record.tyreCode}</Item.Meta>
              <Item.Description>Client: {record.clientName}</Item.Description>
              <Item.Description>Production Order ID: {record.productionOrderId}</Item.Description>
              <Item.Description>Target Market: {record.targetMarket}</Item.Description>
              <Item.Description>Unit of Measure: {record.unitOfMeasure}</Item.Description>
            </Item.Content>
          </Item>
        </Item.Group>
      </Segment>

      <Segment>
        <span style={{ display: 'inline-flex', gap: 18 }}>
          <span>
            <Icon name="calendar" /> {record.saleDate ? record.saleDate.toDateString() : 'N/A'}
          </span>
          <span>
            <Icon name="dollar sign" /> Price per unit: ${record.pricePerUnit}
          </span>
          <span>
            <Icon name="box" /> Quantity sold: {record.quantitySold}
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
            onClick={() => setIsEditing(v => !v)}
          />
          <Button
            color="red"
            floated="right"
            content="Delete"
            onClick={handleDelete}
          />
        </Segment>
      )}

      {isEditing && (
        <Segment>
          <Form onSubmit={handleSubmit}>
            <Form.Group widths="equal">
              <Form.Input
                label="Price Per Unit"
                name="pricePerUnit"
                type="number"
                step="0.01"
                min="0"
                value={updated.pricePerUnit}
                onChange={handleChange}
              />
              <Form.Input
                label="Quantity Sold"
                name="quantitySold"
                type="number"
                step="1"
                min="0"
                value={updated.quantitySold}
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
