import { Button, Icon, Item, Label, Segment, Form } from "semantic-ui-react";
import { ProductionRecord } from "../../models/ProductionRecord";
import { observer } from "mobx-react-lite";
import { useStore } from '../../stores/store';
import { useState } from "react";

interface Props {
  record: ProductionRecord;
}

export default observer(function ProductionRecordItem({ record }: Props) {
  const { userStore: { isQualitySupervisor }, recordStore } = useStore();
  const [isEditing, setIsEditing] = useState(false); // State to toggle edit form visibility
  const [updatedRecord, setUpdatedRecord] = useState({
    shift: record.shift,
    productionDate: record.productionDate?.toISOString().split("T")[0],
    quantityProduced: record.quantityProduced,
  });

  function handleInputChange(event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) {
    const { name, value } = event.target;
    setUpdatedRecord({ ...updatedRecord, [name]: value });
  }

  function handleSubmit() {
    recordStore.updateRecord({
      id: record.id!,
      shift: updatedRecord.shift,
      quantityProduced: updatedRecord.quantityProduced,
      tyreId: record.tyreCode,
      machineId: record.machineNumber.toString()
    }).then(() => {
      setIsEditing(false);
    });
  }

  return (
    <Segment.Group>
      <Segment>
        <Item.Group>
          <Item>
            <Item.Content>
              <Item.Header>Machine Number: {record.machineNumber}</Item.Header>
              <Item.Description>Operator ID: {record.operatorId}</Item.Description>
              <Item.Description>Tyre Code: {record.tyreCode}</Item.Description>
            </Item.Content>
          </Item>
        </Item.Group>
      </Segment>

      <Segment>
        <span>
          <Icon name="calendar" /> {record.productionDate!.toDateString()}
          <Icon name="clock" /> Shift: {record.shift}
        </span>
      </Segment>

      <Segment>
        <span>
          <Icon name="box" /> Quantity Produced: {record.quantityProduced}
        </span>
      </Segment>

      {isQualitySupervisor && (
        <Segment clearing>
          <Button
            color="blue"
            floated="right"
            content={isEditing ? "Cancel" : "Edit"}
            onClick={() => setIsEditing(!isEditing)} // Toggle the form visibility
          />
          <Button
            color="red"
            floated="right"
            content="Delete"
            onClick={() => console.log("Delete Record", record.id)}
          />
        </Segment>
      )}

      {isEditing && (
        <Segment>
          <Form onSubmit={handleSubmit}>
            <Form.Input
              label="Shift"
              placeholder="Shift"
              name="shift"
              value={updatedRecord.shift}
              onChange={handleInputChange}
            />
            <Form.Input
              label="Quantity Produced"
              placeholder="Quantity Produced"
              name="quantityProduced"
              value={updatedRecord.quantityProduced}
              onChange={handleInputChange}
            />
            <Button positive type="submit" content="Save" />
          </Form>
        </Segment>
      )}
    </Segment.Group>
  );
});
