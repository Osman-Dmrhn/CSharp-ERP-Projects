// Dosya: src/pages/ActivityLogPage.tsx

import React, { useEffect, useState, useCallback } from 'react';
import { getAllActivityLogs } from '../api/ActivityLogService';
import type { LogFilters } from '../api/ActivityLogService';
import type { ActivityLog} from '../models/LogDtos/ActivityLog';
import type { PaginationInfo } from '../models/LogDtos/PaginationInfo';
import { Container, Table, Spinner, Alert, Badge } from 'react-bootstrap';
import { LogFiltersComponent } from '../components/LogFilters';
import { PaginationComponent } from '../components/Pagination';
import { format } from 'date-fns';

const ActivityLogPage: React.FC = () => {
  const [logs, setLogs] = useState<ActivityLog[]>([]);
  const [pagination, setPagination] = useState<PaginationInfo | null>(null);
  const [filters, setFilters] = useState<LogFilters>({ pageNumber: 1, pageSize: 20 });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchLogs = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const result = await getAllActivityLogs(filters);
      setLogs(result.logs);
      setPagination(result.pagination);
    } catch (err) {
      setError('Loglar alınırken bir hata oluştu.');
    } finally {
      setLoading(false);
    }
  }, [filters]);

  useEffect(() => {
    fetchLogs();
  }, [fetchLogs]);

  const handleFilter = (newFilters: Partial<LogFilters>) => {
    setFilters(prev => ({ ...prev, ...newFilters, pageNumber: 1 }));
  };

  const handlePageChange = (page: number) => {
    setFilters(prev => ({ ...prev, pageNumber: page }));
  };

  return (
    <Container className="mt-4">
      <h2 className="mb-3">Kullanıcı Aktivite Logları</h2>

      <LogFiltersComponent onFilter={handleFilter} />

      {loading && <div className="text-center"><Spinner animation="border" /></div>}
      {error && <Alert variant="danger">{error}</Alert>}

      {!loading && !error && (
        <>
          <div className="table-responsive">
            <Table striped bordered hover responsive>
              <thead className="table-dark">
                <tr>
                  <th>#</th>
                  <th>Kullanıcı Adı</th>
                  <th>Eylem</th>
                  <th>Hedef</th>
                  <th>Durum</th>
                  <th>Tarih</th>
                </tr>
              </thead>
              <tbody>
                {logs.map((log) => (
                  <tr key={log.id}>
                    <td>{log.id}</td>
                    <td>{log.userName}</td>
                    <td>{log.action}</td>
                    <td>{log.targetEntity || '-'}</td>
                    <td>
                      <Badge bg={log.status === 'Başarılı' ? 'success' : 'danger'}>
                        {log.status}
                      </Badge>
                    </td>
                    <td>{format(new Date(log.createdAt), 'dd.MM.yyyy HH:mm:ss')}</td>
                  </tr>
                ))}
              </tbody>
            </Table>
          </div>
          <div className="d-flex justify-content-end">
            <PaginationComponent pagination={pagination!} onPageChange={handlePageChange} />
          </div>
        </>
      )}
    </Container>
  );
};

export default ActivityLogPage;