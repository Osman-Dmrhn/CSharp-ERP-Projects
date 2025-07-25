import React, { useEffect, useState, useCallback } from 'react';
import { getAllActivityLogs, getLogDetails } from '../api/ActivityLogService';
import type { LogFilters } from '../api/ActivityLogService';
import type { ActivityLog, ActivityLogDetail } from '../models/LogDtos/ActivityLog';
import type { PaginationInfo } from '../models/LogDtos/PaginationInfo';
import { Container, Table, Spinner, Alert, Badge, Button } from 'react-bootstrap';
import { LogFiltersComponent } from '../components/LogFilters';
import { PaginationComponent } from '../components/Pagination';
import { LogDetailModal } from '../components/LogDetailModal'; // Detay modalını import ediyoruz
import { format } from 'date-fns';

const ActivityLogPage: React.FC = () => {
  const [logs, setLogs] = useState<ActivityLog[]>([]);
  const [pagination, setPagination] = useState<PaginationInfo | null>(null);
  const [filters, setFilters] = useState<LogFilters>({ pageNumber: 1, pageSize: 20 });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  // Modal'ı ve detay verisini yönetecek state'ler
  const [selectedLog, setSelectedLog] = useState<ActivityLogDetail | null>(null);
  const [isDetailLoading, setIsDetailLoading] = useState(false);
  const [activeLogId, setActiveLogId] = useState<number | null>(null); // Hangi butonun spinner göstereceğini bilmek için

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
  
  // Detay butonuna tıklandığında çalışacak fonksiyon
  const handleShowDetails = async (logId: number) => {
    setActiveLogId(logId);
    setIsDetailLoading(true);
    setError(null);
    try {
      const result = await getLogDetails(logId);
      if (result.success) {
        setSelectedLog(result.data);
      } else {
        setError(result.message);
      }
    } catch (err) {
      setError("Log detayı alınırken bir hata oluştu.");
    } finally {
      setIsDetailLoading(false);
      setActiveLogId(null);
    }
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
                  <th className="text-center">İncele</th>
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
                    <td className="text-center">
                      <Button 
                        variant="outline-primary" 
                        size="sm" 
                        onClick={() => handleShowDetails(log.id)}
                        disabled={isDetailLoading}
                      >
                        {isDetailLoading && activeLogId === log.id 
                          ? <Spinner as="span" animation="border" size="sm" role="status" aria-hidden="true" />
                          : '👁️ Detay'
                        }
                      </Button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </Table>
          </div>
          <div className="d-flex justify-content-end">
            {pagination && <PaginationComponent pagination={pagination} onPageChange={handlePageChange} />}
          </div>
        </>
      )}
      
      {selectedLog && <LogDetailModal log={selectedLog} onHide={() => setSelectedLog(null)} />}
    </Container>
  );
};

export default ActivityLogPage;