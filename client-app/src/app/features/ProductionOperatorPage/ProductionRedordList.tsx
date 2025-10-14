import { Header } from 'semantic-ui-react';
import { useStore } from '../../stores/store';
import { observer } from 'mobx-react-lite';
import ProductionRecordItem from './ProductionRecordItem'; 
import { Fragment } from 'react/jsx-runtime';

export default observer(function ProductionList() {
  const { recordStore } = useStore();
  const { groupedRecords } = recordStore;

  return (
    <>
      {
        groupedRecords.map(([group, records]) => (
          <Fragment key={group}>
            <Header sub color='teal'>
              {group}
            </Header>
            {
              records.map(record => (
                <ProductionRecordItem key={record.id} record={record} />
              ))
            }
          </Fragment>
        ))
      }
    </>
  );
});
