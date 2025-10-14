import { useState, useEffect } from 'react';
import { Button, Grid, Loader, Form } from 'semantic-ui-react';
import SaleList from './SaleList';
import ProductionList from './ProductionList';
import ProductionSummary from './ProductionSummary';
import StockBalanceReport from './StockBalanceReport';
import { useStore } from '../../stores/store';
import { observer } from 'mobx-react-lite';

export default observer(function BusinessUnitLeaderDashboard() {
  const { saleRecordStore, productionRecordStore } = useStore();
  const [showSales, setShowSales] = useState(false);
  const [showStockBalance, setShowStockBalance] = useState(false);

  useEffect(() => {
    saleRecordStore.loadSaleRecords();
    productionRecordStore.loadAllProductionRecords();
  }, [saleRecordStore, productionRecordStore]);

  return (
    <Grid>
      <Grid.Column width={10}>
        <Button onClick={() => { setShowSales(true); setShowStockBalance(false); }}>
          Show Sale Records
        </Button>
        <Button onClick={() => { setShowSales(false); setShowStockBalance(false); }}>
          Show Production Records
        </Button>
        <Button onClick={() => { setShowStockBalance(true); setShowSales(false); }}>
          Show Stock Balance
        </Button>

        {showSales ? (
          <SaleList />
        ) : showStockBalance ? (
          <StockBalanceReport />
        ) : (
          <ProductionList />
        )}
      </Grid.Column>
    </Grid>
  );
});
