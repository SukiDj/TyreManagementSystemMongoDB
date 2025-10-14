import { Header } from 'semantic-ui-react';
import { useStore } from '../../stores/store';
import { observer } from 'mobx-react-lite';
import SaleRecordItem from './SaleRecordItem.tsx';
import { Fragment } from 'react/jsx-runtime';
import { SaleRecordFromValues } from '../../models/SaleRecord.ts';

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
            {
              records.map(record => (
                <SaleRecordItem key={record.id} record={record} onSubmit={function (updatedValues: SaleRecordFromValues): void {
                  throw new Error('Function not implemented.');
                } } />  // Prikazujemo svaki prodajni zapis
              ))
            }
          </Fragment>
        ))
      }
    </>
  );
});
