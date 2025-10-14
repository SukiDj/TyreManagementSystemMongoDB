import { observer } from 'mobx-react-lite';
import { useStore } from '../../stores/store';
import ActionLogItem from './ActionLogItem';
import { Fragment } from 'react';

export default observer(function ActionLogList() {
    const { logStore } = useStore();
    const { logs } = logStore;

    return (
        <>
            {logs.length > 0 ? (
                logs.map(log => (
                    <Fragment key={log.id}>
                        <ActionLogItem log={log} />  {/* Prikaz svakog loga */}
                    </Fragment>
                ))
            ) : (
                <p>No logs available</p>  // Poruka u slučaju da nema logova
            )}
        </>
    );
});
