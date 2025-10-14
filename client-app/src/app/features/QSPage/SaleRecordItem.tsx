import { Button, Form, Icon, Item, Segment } from "semantic-ui-react";
import { SaleRecord, SaleRecordFromValues } from "../../models/SaleRecord";
import { Link } from "react-router-dom";
import { observer } from "mobx-react-lite";
import { useStore } from "../../stores/store"; // Uvoz store-a
import { useState } from "react";

interface Props {
  record: SaleRecord;
  onSubmit: (updatedValues: SaleRecordFromValues) => void;
}

export default observer(function SaleRecordItem({ record }: Props) {
  const { saleRecordStore } = useStore(); 
  const [isDeleting, setIsDeleting] = useState(false); 
  const [isEditing, setIsEditing] = useState(false); 
  const [updatedValues, setUpdatedValues] = useState<SaleRecordFromValues>({
    tyreId: record.tyreCode,
    clientId: record.clientId,
    productionOrderId: record.productionOrderId,
    targetMarket: record.targetMarket,
    pricePerUnit: record.pricePerUnit,
    unitOfMeasure: record.unitOfMeasure,
    quantitySold: record.quantitySold
  });

  const handleDelete = async (id: string) => {
    setIsDeleting(true);
    try {
      await saleRecordStore.deleteSaleRecord(id);
    } catch (error) {
      console.error("Error deleting record:", error);
    } finally {
      setIsDeleting(false); // Reset loading stanja
    }
  };

  // Funkcija za submit izmena
  const handleSubmit = async () => {
    try {
      console.log(record.id);
      await saleRecordStore.updateRecord(record.id, updatedValues);
      setIsEditing(false); // Zatvara formu nakon uspešne izmene
    } catch (error) {
      console.error("Error updating record:", error);
    }
  };

  // Funkcija za editovanje
  const handleEditClick = () => {
    setIsEditing(true); // Otvara formu za editovanje
  };

  // Funkcija za ažuriranje vrednosti forme
  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setUpdatedValues({ ...updatedValues, [name]: value });
  };

  return (
    <Segment.Group>
      <Segment>
        <Item.Group>
          <Item>
            <Item.Content>
              <Item.Header>
                Tyre Code: {record.tyreCode}
              </Item.Header>
              <Item.Description>
                Client ID: {record.clientId}
              </Item.Description>
              <Item.Description>
                Production Order ID: {record.productionOrderId}
              </Item.Description>
              <Item.Description>
                Target Market: {record.targetMarket}
              </Item.Description>
            </Item.Content>
          </Item>
        </Item.Group>
      </Segment>

      <Segment>
        <span>
          <Icon name="calendar" /> Sale Date: {record.saleDate ? record.saleDate.toDateString() : 'N/A'}
        </span>
      </Segment>

      <Segment>
        <span>
          <Icon name="dollar sign" /> Price Per Unit: ${record.pricePerUnit}
        </span>
      </Segment>

      <Segment>
        <span>
          <Icon name="box" /> Quantity Sold: {record.quantitySold}
        </span>
      </Segment>

      {isEditing ? (
        <Segment>
          <Form>
            <Form.Input
              label="Tyre Code"
              name="tyreCode"
              value={updatedValues.tyreId}
              onChange={handleInputChange}
            />
            <Form.Input
              label="Client ID"
              name="clientId"
              value={updatedValues.clientId}
              onChange={handleInputChange}
            />
            <Form.Input
              label="Production Order ID"
              name="productionOrderId"
              value={updatedValues.productionOrderId}
              onChange={handleInputChange}
            />
            <Form.Input
              label="Target Market"
              name="targetMarket"
              value={updatedValues.targetMarket}
              onChange={handleInputChange}
            />
            <Form.Input
              label="Unit of measure"
              name="unitOfMeasure"
              value={updatedValues.unitOfMeasure}
              onChange={handleInputChange}
            />
            <Form.Input
              label="Price Per Unit"
              name="pricePerUnit"
              value={updatedValues.pricePerUnit}
              onChange={handleInputChange}
            />
            <Form.Input
              label="Quantity Sold"
              name="quantitySold"
              value={updatedValues.quantitySold}
              onChange={handleInputChange}
            />
            <Button color="green" onClick={handleSubmit} content="Save" />
            <Button color="grey" onClick={() => setIsEditing(false)} content="Cancel" />
          </Form>
        </Segment>
      ) : (
        <Segment clearing>
          <Button
            color="blue"
            floated="right"
            content="Edit"
            onClick={handleEditClick}
          />
          <Button
            color="red"
            floated="right"
            loading={isDeleting}
            content="Delete"
            onClick={() => handleDelete(record.id!)}
          />
        </Segment>
      )}
    </Segment.Group>
  );
});