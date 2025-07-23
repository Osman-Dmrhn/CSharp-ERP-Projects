import React, { useEffect, useState } from 'react';
import { getAllActivityLogs } from '../api/ActivityLogService';
import type { ActivityLog } from '../models/LogDtos/ActivityLog';
import { Container, Table, Spinner, Alert } from 'react-bootstrap';

const ActivityLogPage: React.FC = () => {
  const [logs, setLogs] = useState<ActivityLog[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchLogs = async () => {
      try {
        const result = await getAllActivityLogs();
        if (result.success) {
          setLogs(result.data);
        } else {
          setError(result.message);
        }
      } catch (err) {
        setError('Loglar alınırken bir hata oluştu.');
      } finally {
        setLoading(false);
      }
    };

    fetchLogs();
  }, []);

  return (
    <Container className="mt-4">
      <h2 className="mb-4">Kullanıcı Aktivite Logları</h2>

      {loading && <Spinner animation="border" />}
      {error && <Alert variant="danger">{error}</Alert>}

      {!loading && !error && (
        <div className="table-responsive">
          <Table striped bordered hover responsive>
            <thead className="table-dark">
              <tr>
                <th>#</th>
                <th>Kullanıcı ID</th>
                <th>Kullanıcı Adı</th>
                <th>İşlem</th>
                <th>Tarih</th>
              </tr>
            </thead>
            <tbody>
              {logs.map((log) => (
                <tr key={log.id}>
                  <td>{log.id}</td>
                  <td>{log.userId}</td>
                  <td>{log.userName}</td>
                  <td>{log.action}</td>
                  <td>{new Date(log.createdAt).toLocaleString('tr-TR')}</td>
                </tr>
              ))}
            </tbody>
          </Table>
        </div>
      )}
    </Container>
  );
};

export default ActivityLogPage;
