import { Header } from 'semantic-ui-react';
import { useStore } from '../../stores/store';
import { observer } from 'mobx-react-lite';
import SaleRecordItem from './SaleRecordItem.tsx';
import { Fragment } from 'react/jsx-runtime';

export default observer(function SaleRecordList() {
  const { saleRecordStore } = useStore();
  const { groupedRecords } = saleRecordStore;

  return (
    <>
      {
        groupedRecords.map(([group, records]) => (  // Mapa kroz grupisane zapise
          <Fragment key={group}>
            <Header sub color='teal'>
              {group}  {}
            </Header>
            {records.map((record, idx) => (
              <SaleRecordItem
                key={record.id ?? `${record.clientId}-${record.productionOrderId}-${idx}`}
                record={record}
                onSubmit={() => {}}
              />
            ))}
          </Fragment>
        ))
      }
    </>
  );
});
