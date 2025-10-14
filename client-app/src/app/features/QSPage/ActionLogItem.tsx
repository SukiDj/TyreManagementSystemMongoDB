import { ActionLog } from '../../models/ActionLog';

interface Props {
    log: ActionLog;
}

export default function ActionLogItem({ log }: Props) {
    return (
        <div>
            <h4>{log.actionName}</h4>
            <p>User: {log.userId}</p>
            <p>Timestamp: {new Date(log.timestamp).toLocaleString()}</p>
            <p>Details: {log.details}</p>
        </div>
    );
}
