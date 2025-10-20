import { observer } from 'mobx-react-lite';
import { useEffect, useState } from 'react';
import { Button, Card, Divider, Dropdown, Form, Grid, Header, Icon, Loader, Segment } from 'semantic-ui-react';
import InfiniteScroll from 'react-infinite-scroller';
import { useStore } from '../../stores/store';
import { PagingParams } from '../../models/Pagination';
import SaleRecordItemPlaceholder from './SaleRecordItemPlaceholder';
import SaleRecordList from './SaleRecordList';
import ProductionRecordItemPlaceholder from '../ProductionOperatorPage/ProductionRecordItemPlaceholder';
import ProductionRedordList from '../ProductionOperatorPage/ProductionRedordList';
import { ActionLog } from '../../models/ActionLog';
import ActionLogList from './ActionLogList';

export default observer(function QSPage() {
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
    productionRecordStore,
    recordStore: { loadAllProductionRecords },
    logStore: { loadLogs }
  } = useStore();

  const [view, setView] = useState<'sales' | 'productions' | 'logs'>('sales');
  const [logs, setLogs] = useState<ActionLog[]>([]);

  useEffect(() => {
    setPagingParams(new PagingParams(0));
    loadAllProductionRecords();
    loadSaleRecords();
    if (productionRecordStore.recordRegistry.size === 0) productionRecordStore.loadAllProductionRecords();
    if (tyreStore.tyreRegistry.size === 0) tyreStore.loadTyres();
    if (clientStore.clientRegistry.size === 0) clientStore.loadClients();
  }, [loadSaleRecords, setPagingParams, tyreStore, clientStore, loadAllProductionRecords]);

  const handleGetNextSales = () => {
    setLoadingNext(true);
    setPagingParams(new PagingParams(pagination!.currentPage + 1));
    loadSaleRecords().then(() => setLoadingNext(false));
  };

  const handleGetNextProd = () => {
    setLoadingNext(true);
    setPagingParams(new PagingParams(pagination!.currentPage + 1));
    loadAllProductionRecords().then(() => setLoadingNext(false));
  };

  const handleShowLogs = async () => {
    setView('logs');
    try {
      const fetched = await loadLogs();
      setLogs(fetched);
    } catch (e) {
      console.error('Error loading logs:', e);
    }
  };

  // form state
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
      loadSaleRecords();
      // reset some inputs
      setQuantitySold('');
      setPricePerUnit('');
    });
  };

  return (
    <Grid stackable>
      <Grid.Row>
        <Grid.Column width={16}>
          <Header as='h2' content='Quality Supervisor' subheader='Register sales, review productions, and view action logs' />
          <Button.Group>
            <Button color={view === 'sales' ? 'teal' : undefined} onClick={() => setView('sales')}>
              <Icon name='shopping cart' /> Sales
            </Button>
            <Button color={view === 'productions' ? 'teal' : undefined} onClick={() => setView('productions')}>
              <Icon name='factory' /> Productions
            </Button>
            <Button color={view === 'logs' ? 'teal' : undefined} onClick={handleShowLogs}>
              <Icon name='history' /> Logs
            </Button>
          </Button.Group>
          <Divider />
        </Grid.Column>
      </Grid.Row>

      <Grid.Row columns={2}>
        <Grid.Column width={8}>
          <Card fluid>
            <Card.Content>
              <Card.Header>
                <Icon name='plus circle' /> Create Sale Record
              </Card.Header>
            </Card.Content>
            <Card.Content>
              <Form onSubmit={handleSubmit}>
                <Form.Field>
                  <label>Client</label>
                  <Dropdown
                    placeholder='Select Client'
                    fluid selection
                    options={clientStore.clientOptions}
                    value={clientId}
                    onChange={(_e, { value }) => setClientId(value as string)}
                  />
                </Form.Field>
                <Form.Group widths='equal'>
                  <Form.Field>
                  <label>Production</label>
                  <Dropdown
                    placeholder='Select Production Order'
                    fluid selection
                    options={productionRecordStore.productionOptions}
                    value={productionOrderId}
                    onChange={(_e, { value }) => setProductionOrderId(value as string)}
                  />
                </Form.Field>
                  <Form.Input
                    label='Quantity'
                    type='number'
                    min='0'
                    value={quantitySold}
                    onChange={e => setQuantitySold(e.target.value)}
                  />
                </Form.Group>
                <Form.Group widths='equal'>
                  <Form.Input
                    label='Price per Unit'
                    type='number'
                    min='0'
                    step='0.01'
                    value={pricePerUnit}
                    onChange={e => setPricePerUnit(e.target.value)}
                  />
                  <Form.Field>
                    <label>Unit</label>
                    <Dropdown
                      placeholder='Select Unit'
                      fluid selection
                      options={unitOptions}
                      value={unitOfMeasure}
                      onChange={(_e, { value }) => setUnitOfMeasure(value as string)}
                    />
                  </Form.Field>
                </Form.Group>
                <Form.Group widths='equal'>
                  <Form.Field>
                    <label>Target Market</label>
                    <Dropdown
                      placeholder='Select Market'
                      fluid selection
                      options={targetMarketOptions}
                      value={targetMarket}
                      onChange={(_e, { value }) => setTargetMarket(value as string)}
                    />
                  </Form.Field>
                  <Form.Input
                    label='Sale Date'
                    type='date'
                    value={saleDate ? saleDate.toISOString().split('T')[0] : ''}
                    onChange={e => setSaleDate(e.target.value ? new Date(e.target.value) : null)}
                  />
                </Form.Group>
                <Button type='submit' color='teal' loading={isSubmitting}>
                  <Icon name='check' /> Create
                </Button>
              </Form>
            </Card.Content>
          </Card>
        </Grid.Column>

        {/* Right column: dynamic content based on view */}
        <Grid.Column width={8}>
          {view === 'sales' && (
            <>
              {(loadingInitial && !loadingNext) || isSubmitting ? (
                <>
                  <SaleRecordItemPlaceholder />
                  <SaleRecordItemPlaceholder />
                </>
              ) : (
                <Segment>
                  <Header as='h3'>
                    <Icon name='list' />
                    <Header.Content>Recent Sales</Header.Content>
                  </Header>
                  <Divider />
                  <InfiniteScroll
                    pageStart={0}
                    loadMore={handleGetNextSales}
                    hasMore={!loadingNext && !!pagination && pagination.currentPage < pagination.totalPages}
                    initialLoad={false}
                  >
                    <SaleRecordList />
                  </InfiniteScroll>
                  <Loader active={loadingNext} inline='centered' />
                </Segment>
              )}
            </>
          )}

          {view === 'productions' && (
            <>
              {(loadingInitial && !loadingNext) || isSubmitting ? (
                <>
                  <ProductionRecordItemPlaceholder />
                  <ProductionRecordItemPlaceholder />
                </>
              ) : (
                <Segment>
                  <Header as='h3'>
                    <Icon name='settings' />
                    <Header.Content>Recent Productions</Header.Content>
                  </Header>
                  <Divider />
                  <InfiniteScroll
                    pageStart={0}
                    loadMore={handleGetNextProd}
                    hasMore={!loadingNext && !!pagination && pagination.currentPage < pagination.totalPages}
                    initialLoad={false}
                  >
                    <ProductionRedordList />
                  </InfiniteScroll>
                  <Loader active={loadingNext} inline='centered' />
                </Segment>
              )}
            </>
          )}

          {view === 'logs' && (
            <Segment>
              <Header as='h3'>
                <Icon name='history' />
                <Header.Content>Action Logs</Header.Content>
              </Header>
              <Divider />
              <ActionLogList />
            </Segment>
          )}
        </Grid.Column>
      </Grid.Row>
    </Grid>
  );
});
