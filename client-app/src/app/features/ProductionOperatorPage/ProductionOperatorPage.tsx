import { useEffect, useState } from 'react';
import { observer } from 'mobx-react-lite';
import { useStore } from '../../stores/store';
import {
  Grid,
  Segment,
  Header,
  Icon,
  Form,
  Dropdown,
  Button,
  Loader,
} from 'semantic-ui-react';
import ProductionList from './ProductionRedordList';
import ProductionListItemPlaceholder from './ProductionRecordItemPlaceholder';
import { PagingParams } from '../../models/Pagination';

export default observer(function ProductionOperatorPage() {
  const {
    recordStore: {
      setPagingParams,
      pagination,
      loadProductionRecords,
      loadingNext,
      setLoadingNext,
      loadingInitial,
      isSubmitting,
    },
    userStore: { user },
    tyreStore,
    machineStore,
    productionRecordStore,
  } = useStore();

  // form state
  const [machineNumber, setMachineNumber] = useState('');
  const [tyreCode, setTyreCode] = useState('');
  const [shiftValue, setShiftValue] = useState('');
  const [quantityProd, setQuantityProd] = useState('');

  const shiftOptions = [
    { key: '1', text: '1st shift', value: '1' },
    { key: '2', text: '2nd shift', value: '2' },
    { key: '3', text: '3rd shift', value: '3' },
  ];

  useEffect(() => {
    setPagingParams(new PagingParams(0));
    if (user?.userName) loadProductionRecords(user.userName);

    if (tyreStore.tyreRegistry.size === 0) tyreStore.loadTyres();
    if (machineStore.machineRegistry.size === 0) machineStore.loadMachines();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [user?.userName]);

  function handleGetNext() {
    if (!pagination) return;
    setLoadingNext(true);
    setPagingParams(new PagingParams(pagination.currentPage + 1));
    if (user?.userName) {
      loadProductionRecords(user.userName).finally(() => setLoadingNext(false));
    }
  }

  const handleSubmit = async () => {
    const payload = {
      tyreId: tyreCode,
      machineId: machineNumber,
      shift: Number(shiftValue),
      quantityProduced: parseInt(quantityProd || '0', 10),
    };

    await productionRecordStore.createRecord(payload);
    if (user?.userName) loadProductionRecords(user.userName);
    setQuantityProd('');
  };

  return (
    <Grid style={{ marginTop: '1.5rem' }}>
      {/* Left: create form */}
      <Grid.Column width={10}>
        <Segment raised>
          <Header as="h3">
            <Icon name="plus circle" color="blue" />
            <Header.Content>
              Create Production Record
              <Header.Subheader>
                Select machine, tyre, shift and quantity
              </Header.Subheader>
            </Header.Content>
          </Header>

          <Form onSubmit={handleSubmit}>
            <Form.Field>
              <label>Machine</label>
              <Dropdown
                placeholder="Select machine"
                fluid
                selection
                options={machineStore.machineOptions}
                value={machineNumber}
                onChange={(_e, { value }) => setMachineNumber(value as string)}
              />
            </Form.Field>

            <Form.Field>
              <label>Tyre</label>
              <Dropdown
                placeholder="Select tyre"
                fluid
                selection
                options={tyreStore.tyreOptions}
                value={tyreCode}
                onChange={(_e, { value }) => setTyreCode(value as string)}
              />
            </Form.Field>

            <Form.Group widths="equal">
              <Form.Field>
                <label>Shift</label>
                <Dropdown
                  placeholder="Select shift"
                  fluid
                  selection
                  options={shiftOptions}
                  value={shiftValue}
                  onChange={(_e, { value }) => setShiftValue(value as string)}
                />
              </Form.Field>

              <Form.Input
                label="Quantity"
                placeholder="Enter quantity"
                type="number"
                value={quantityProd}
                onChange={(e) => setQuantityProd(e.target.value)}
              />
            </Form.Group>

            <Button
              primary
              icon="check"
              content="Create"
              loading={isSubmitting}
              disabled={!machineNumber || !tyreCode || !shiftValue || !quantityProd}
            />
          </Form>
        </Segment>
      </Grid.Column>

      {/* Right: recent list */}
      <Grid.Column width={6}>
        <Segment raised>
          <Header as="h3">
            <Icon name="list" color="blue" />
            <Header.Content>Recent Productions</Header.Content>
          </Header>

          {(loadingInitial && !loadingNext) || isSubmitting ? (
            <>
              <ProductionListItemPlaceholder />
              <ProductionListItemPlaceholder />
            </>
          ) : (
            <>
              <ProductionList />
              <div style={{ paddingTop: '0.5rem' }}>
                {pagination && pagination.currentPage < pagination.totalPages && (
                  <Button
                    basic
                    fluid
                    content="Load more"
                    loading={loadingNext}
                    onClick={handleGetNext}
                  />
                )}
              </div>
            </>
          )}
        </Segment>
      </Grid.Column>

      <Grid.Column width={10}>
        <Loader active={loadingNext} />
      </Grid.Column>
    </Grid>
  );
});
