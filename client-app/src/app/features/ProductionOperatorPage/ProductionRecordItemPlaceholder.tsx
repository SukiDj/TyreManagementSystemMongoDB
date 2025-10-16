import { Header, Segment } from 'semantic-ui-react';
import { useStore } from '../../stores/store';
import { observer } from 'mobx-react-lite';
import ProductionRecordItem from './ProductionRecordItem';
import { Fragment } from 'react/jsx-runtime';

export default observer(function ProductionList() {
  const { recordStore } = useStore();
  const { groupedRecords } = recordStore;

  if (groupedRecords.length === 0) {
    return (
      <Segment basic secondary textAlign="center" content="No records yet." />
    );
  }

  return (
    <>
      {groupedRecords.map(([group, records]) => (
        <Fragment key={group}>
          <Header sub color="grey" style={{ marginTop: '1rem' }}>
            {group}
          </Header>
          {records.map((record) => (
            <ProductionRecordItem key={record.id} record={record} />
          ))}
        </Fragment>
      ))}
    </>
  );
});
