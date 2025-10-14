import { Grid, Loader, Form, Dropdown, Button } from 'semantic-ui-react';
import ProductionList from './ProductionRedordList';
import { observer } from 'mobx-react-lite';
import { useStore } from '../../stores/store';
import { PagingParams } from '../../models/Pagination';
import InfiniteScroll from 'react-infinite-scroller';
import ProductionListItemPlaceholder from './ProductionRecordItemPlaceholder';
import { useEffect, useState } from 'react';

export default observer(function ProductionDashboard() {
  const {
    recordStore: {
      setPagingParams,
      pagination,
      loadProductionRecords,
      loadingNext,
      setLoadingNext,
      loadingInitial,
      isSubmitting
    },
    userStore: {
      user
    }
  } = useStore();

  const {tyreStore, machineStore, productionRecordStore} = useStore();
 
  useEffect(() => {
    setPagingParams(new PagingParams(0));
    loadProductionRecords(user!.userName); 
    console.log(productionRecordStore.groupedRecords);
    if (tyreStore.tyreRegistry.size === 0) tyreStore.loadTyres();
    if (machineStore.machineRegistry.size === 0) machineStore.loadMachines();
  }, [loadProductionRecords, setPagingParams, tyreStore]);

  function handleGetNext() {
    setLoadingNext(true);
    setPagingParams(new PagingParams(pagination!.currentPage + 1));
    loadProductionRecords(user!.userName).then(() => setLoadingNext(false));
  }



  const shiftOptions = [
    { key: '1', text: '1st Shift', value: '1' },
    { key: '2', text: '2nd Shift', value: '2' },
    { key: '3', text: '3rd Shift', value: '3' }
  ];

  const [shiftValue, setShiftValue] = useState('');

  const handleSubmit = () => {
    
    const newRecord = {
      tyreId: tyreCode,
      machineId: machineNumber,
      shift: shift,
      quantityProduced: parseInt(quantityProd), // Make sure it's a number
    };
    console.log(newRecord);
    productionRecordStore.createRecord(newRecord).then(() => {
      loadProductionRecords(user!.userName); // Load records after creating the new one
  });

  }
  const [machineNumber, setmachineNumber] = useState('');
  const [tyreCode, setyreCode] = useState('');
  const [shift, setShift] = useState(Number);
  const [quantityProd, setquantityProd] = useState('');

  return (
    <Grid style={{ marginTop: '0em' }}>
      <Grid.Column width='10'>
        {/* Form for creating a new production record */}
        <Form onSubmit={handleSubmit}>
          <Form.Field>
            <label>Machine</label>
            <Dropdown 
            placeholder='Select Machine' 
            fluid
             selection
              options={machineStore.machineOptions}
              value={machineNumber} // Controlled component
              onChange={(_e, { value }) => {
                setmachineNumber(value as string);
                
              }}
               />
          </Form.Field>
          <Form.Field>
            <label>Tyre Name</label>
            <Dropdown placeholder='Select Tyre' 
            fluid
             selection 
            options={tyreStore.tyreOptions} 
            value={tyreCode} // Controlled component
              onChange={(_e, { value }) => {
                setyreCode(value as string);
                
              }}
            />
          </Form.Field>
          <Form.Field>
            <label>Shift</label>
            <Dropdown placeholder='Select Shift' fluid selection options={shiftOptions} value={shiftValue} // Controlled component
              onChange={(_e, { value }) => {
                setShift(Number(value));
                setShiftValue(value as string);
              }}/>
          </Form.Field>
          <Form.Field>
            <label>Quantity Produced</label>
            <input 
              type='number' 
              placeholder='Enter Quantity' 
              value={quantityProd} // Controlled component
              onChange={(e) => setquantityProd(e.target.value)} // Update local state
            />
          </Form.Field>
          <Button type='submit' color='teal'>Create Production Record</Button>
        </Form>

        {/* Loader or production list based on loading state */}
        {(loadingInitial && !loadingNext) || isSubmitting ? (
          <>
            <ProductionListItemPlaceholder />
            <ProductionListItemPlaceholder />
          </>
        ) : (
          <InfiniteScroll
            pageStart={0}
            loadMore={handleGetNext}
            hasMore={!loadingNext && !!pagination && pagination.currentPage < pagination.totalPages}
            initialLoad={false}
          >
            <br/>
            <br/>

            <ProductionList />
          </InfiniteScroll>
        )}
      </Grid.Column>
      <Grid.Column width='6'>
        {/* You can add filters here if necessary */}
      </Grid.Column>
      <Grid.Column width={10}>
        <Loader active={loadingNext} />
      </Grid.Column>
    </Grid>
  );
});
