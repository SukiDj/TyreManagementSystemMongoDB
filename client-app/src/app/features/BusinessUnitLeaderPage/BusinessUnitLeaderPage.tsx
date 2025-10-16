import { useState, useEffect } from 'react';
import { observer } from 'mobx-react-lite';
import { Grid, Segment, Header, Icon, Button } from 'semantic-ui-react';
import { useStore } from '../../stores/store';
import ProductionList from './ProductionList';
import SaleList from './SaleList';
import StockBalanceReport from './StockBalanceReport';

type Section = 'production' | 'sales' | 'stock';

export default observer(function BusinessUnitLeaderPage() {
  const { saleRecordStore, recordStore } = useStore();
  const [section, setSection] = useState<Section>('production');

  useEffect(() => {
    // warm up caches so switching tabs is instant
    saleRecordStore.loadSaleRecords();
    recordStore.loadAllProductionRecords();
  }, [saleRecordStore, recordStore]);

  return (
    <Grid style={{ marginTop: '1.5rem' }}>
      {/* Left: section switcher / description */}
      <Grid.Column width={16}>
        <Segment raised>
          <Header as="h3">
            <Icon name="settings" color="blue" />
            <Header.Content>
              Business Unit Leader
              <Header.Subheader>
                View production, sales and stock reports
              </Header.Subheader>
            </Header.Content>
          </Header>

          <Button.Group fluid>
            <Button
              primary={section === 'production'}
              basic={section !== 'production'}
              content="Production"
              onClick={() => setSection('production')}
            />
            <Button
              primary={section === 'sales'}
              basic={section !== 'sales'}
              content="Sales"
              onClick={() => setSection('sales')}
            />
            <Button
              primary={section === 'stock'}
              basic={section !== 'stock'}
              content="Stock"
              onClick={() => setSection('stock')}
            />
          </Button.Group>

          <Segment basic>
            {section === 'production' && (
              <>
                <Header as="h4">Production view</Header>
                <p>Summaries by day, shift, machine, or operator.</p>
              </>
            )}
            {section === 'sales' && (
              <>
                <Header as="h4">Sales view</Header>
                <p>Recent tyre sales with client and pricing.</p>
              </>
            )}
            {section === 'stock' && (
              <>
                <Header as="h4">Stock view</Header>
                <p>Pick a date to see stock balance by tyre code.</p>
              </>
            )}
          </Segment>
        </Segment>
      </Grid.Column>

      {/* Right: data card */}
      <Grid.Column width={16}>
        <Header as="h3">
          <Icon
            name={section === 'production' ? 'industry' : section === 'sales' ? 'shopping cart' : 'boxes'}
            color="blue"
          />
          <Header.Content>
            {section === 'production' && 'Production Records'}
            {section === 'sales' && 'Sale Records'}
            {section === 'stock' && 'Stock Balance'}
          </Header.Content>
        </Header>

        {section === 'production' && <ProductionList />}
        {section === 'sales' && <SaleList />}
        {section === 'stock' && <StockBalanceReport />}
      </Grid.Column>
    </Grid>
  );
});
