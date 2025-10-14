import { Grid, Loader, Form, Dropdown, Button, Segment } from 'semantic-ui-react';
import SaleList from './SaleRecordList';
import { observer } from 'mobx-react-lite';
import { useStore } from '../../stores/store';
import { PagingParams } from '../../models/Pagination';
import InfiniteScroll from 'react-infinite-scroller';
import SaleListItemPlaceholder from './SaleRecordItemPlaceholder';
import { useEffect, useState } from 'react';
import SaleRecordItemPlaceholder from './SaleRecordItemPlaceholder';
import SaleRecordList from './SaleRecordList';
import ProductionRecordItemPlaceholder from '../ProductionOperatorPage/ProductionRecordItemPlaceholder';
import ProductionRedordList from '../ProductionOperatorPage/ProductionRedordList';
import { ActionLog } from '../../models/ActionLog';
import ActionLogList from './ActionLogList';

export default observer(function SaleDashboard() {
  const {
    saleRecordStore: {
      setPagingParams,
      pagination,
      loadSaleRecords,
      loadingNext,
      setLoadingNext,
      loadingInitial,
      isSubmitting,
      createRecord,
    },
    tyreStore,
    clientStore,
    userStore: { user }, // Uvoz funkcije za logove
    logStore: { loadLogs },
    recordStore: {
      loadAllProductionRecords,
    },
  } = useStore();

  const [showSales, setShowSales] = useState(true); // Prikaz prodajnih zapisa ili proizvodnih
  const [showProductions, setShowProductions] = useState(false);
  const [logs, setLogs] = useState<ActionLog[]>([]); // Stanje za logove akcija

  // Učitavanje osnovnih podataka prilikom mount-a komponente
  useEffect(() => {
    setPagingParams(new PagingParams(0)); 
    loadAllProductionRecords(); 
    loadSaleRecords(); 
    if (tyreStore.tyreRegistry.size === 0) tyreStore.loadTyres();
    if (clientStore.clientRegistry.size === 0) clientStore.loadClients();
  }, [loadSaleRecords, setPagingParams, tyreStore, clientStore, loadAllProductionRecords]);

  function handleGetNext() {
    setLoadingNext(true);
    setPagingParams(new PagingParams(pagination!.currentPage + 1));
    loadSaleRecords().then(() => setLoadingNext(false));
  }

  function handleGetNextProd() {
    setLoadingNext(true);
    setPagingParams(new PagingParams(pagination!.currentPage + 1));
    loadAllProductionRecords().then(() => setLoadingNext(false));
  }

  // Funkcija za učitavanje logova akcija
  const handleShowLogs = async () => {
    try {
      setShowSales(false);
      setShowProductions(false);
      const fetchedLogs: ActionLog[] = await loadLogs(); // Ensure this returns ActionLog[]
      setLogs(fetchedLogs);
    } catch (error) {
      console.error("Error loading logs:", error);
    }
  };
  

  const unitOptions = [
    { key: 'pcs', text: 'Pieces', value: 'pcs' },
    { key: 'boxes', text: 'Boxes', value: 'boxes' }
  ];

  const targetMarketOptions = [
    { key: 'domestic', text: 'Domestic', value: 'domestic' },
    { key: 'international', text: 'International', value: 'international' }
  ];

  const [tyreCode, setTyreCode] = useState('');
  const [clientId, setClientId] = useState('');
  const [productionOrderId, setProductionOrderId] = useState('');
  const [unitOfMeasure, setUnitOfMeasure] = useState('');
  const [pricePerUnit, setPricePerUnit] = useState('');
  const [quantitySold, setQuantitySold] = useState('');
  const [targetMarket, setTargetMarket] = useState('');
  const [saleDate, setSaleDate] = useState<Date | null>(null);

  const handleSubmit = () => {
    const newSaleRecord = {
      tyreId: tyreCode,
      clientId,
      productionOrderId,
      unitOfMeasure,
      pricePerUnit: parseFloat(pricePerUnit), 
      quantitySold: parseInt(quantitySold),
      saleDate,
      targetMarket
    };
    createRecord(newSaleRecord).then(() => {
      loadSaleRecords(); // Ponovno učitavanje prodajnih zapisa nakon kreiranja novog
    });
  };

  return (
    <Grid style={{ marginTop: '0em' }}>
      <Grid.Column width='10'>
        <Button
          onClick={() => {setShowSales(true); setShowProductions(false)}}
          color={showSales ? 'teal' : 'grey'}
        >
          Show Sale Records
        </Button>
        <Button
          onClick={() => {setShowSales(false); setShowProductions(true)}}
          color={!showSales ? 'teal' : 'grey'}
        >
          Show Production Records
        </Button>

        {/* Dugme za prikaz logova */}
        <Button 
          onClick={handleShowLogs}
          color='blue'
        >
          Show Action Logs
        </Button>

        {/* Display logs if they are loaded */}
        {}



        {showSales ? (
          <>
            <Form onSubmit={handleSubmit}>
              <Form.Field>
                <label>Tyre Code</label>
                <Dropdown
                  placeholder='Select Tyre'
                  fluid
                  selection
                  options={tyreStore.tyreOptions}
                  value={tyreCode}
                  onChange={(_e, { value }) => setTyreCode(value as string)}
                />
              </Form.Field>
              <Form.Field>
                <label>Client</label>
                <Dropdown
                  placeholder='Select Client'
                  fluid
                  selection
                  options={clientStore.clientOptions}
                  value={clientId}
                  onChange={(_e, { value }) => setClientId(value as string)}
                />
              </Form.Field>
              <Form.Field>
                <label>Production Order ID</label>
                <input
                  placeholder='Enter Production Order ID'
                  value={productionOrderId}
                  onChange={(e) => setProductionOrderId(e.target.value)}
                />
              </Form.Field>
              <Form.Field>
                <label>Unit of Measure</label>
                <Dropdown
                  placeholder='Select Unit'
                  fluid
                  selection
                  options={unitOptions}
                  value={unitOfMeasure}
                  onChange={(_e, { value }) => setUnitOfMeasure(value as string)}
                />
              </Form.Field>
              <Form.Field>
                <label>Price Per Unit</label>
                <input
                  type='number'
                  placeholder='Enter Price'
                  value={pricePerUnit}
                  onChange={(e) => setPricePerUnit(e.target.value)}
                />
              </Form.Field>
              <Form.Field>
                <label>Quantity Sold</label>
                <input
                  type='number'
                  placeholder='Enter Quantity'
                  value={quantitySold}
                  onChange={(e) => setQuantitySold(e.target.value)}
                />
              </Form.Field>
              <Form.Field>
                <label>Target Market</label>
                <Dropdown
                  placeholder='Select Market'
                  fluid
                  selection
                  options={targetMarketOptions}
                  value={targetMarket}
                  onChange={(_e, { value }) => setTargetMarket(value as string)}
                />
              </Form.Field>
              <Form.Field>
                <label>Sale Date</label>
                <input
                  type='date'
                  placeholder='Select Sale Date'
                  value={saleDate ? saleDate.toISOString().split('T')[0] : ''}
                  onChange={(e) => setSaleDate(new Date(e.target.value))}
                />
              </Form.Field>
              <Button type='submit' color='teal'>
                Create Sale Record
              </Button>
            </Form>

            {(loadingInitial && !loadingNext) || isSubmitting ? (
              <>
                <SaleRecordItemPlaceholder />
                <SaleRecordItemPlaceholder />
              </>
            ) : (
              <InfiniteScroll
                pageStart={0}
                loadMore={handleGetNext}
                hasMore={!loadingNext && !!pagination && pagination.currentPage < pagination.totalPages}
                initialLoad={false}
              >
                <br />
                <br />
                <SaleRecordList />
              </InfiniteScroll>
            )}
          </>
        ) : showProductions ? (
          (loadingInitial && !loadingNext) || isSubmitting ? (
            <>
              <ProductionRecordItemPlaceholder />
              <ProductionRecordItemPlaceholder />
            </>
          ) : (
            <InfiniteScroll
              pageStart={0}
              loadMore={handleGetNextProd}
              hasMore={!loadingNext && !!pagination && pagination.currentPage < pagination.totalPages}
              initialLoad={false}
            >
              <br/>
              <ProductionRedordList />
            </InfiniteScroll>
          )
        ) : (
          logs.length > 0 && (
            <Segment>
                <h3>Action Logs:</h3>
                <ActionLogList />  {/* This will now display all log details */}
            </Segment>
        )
        )}
      </Grid.Column>
      <Grid.Column width='6'>
        {/* Prazan Grid za potencijalnu upotrebu */}
      </Grid.Column>
      <Grid.Column width={10}>
        <Loader active={loadingNext} />
      </Grid.Column>
    </Grid>
  );
});