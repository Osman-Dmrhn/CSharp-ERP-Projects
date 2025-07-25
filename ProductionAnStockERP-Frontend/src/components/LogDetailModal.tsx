// Dosya: src/components/LogDetailModal.tsx

import React from 'react';
import { Modal, Button, Table, Badge } from 'react-bootstrap';
import type { ActivityLogDetail } from '../models/LogDtos/ActivityLog';
import { format } from 'date-fns';

interface LogDetailModalProps {
  log: ActivityLogDetail;
  onHide: () => void;
}

// JSON formatındaki değişiklikleri daha okunaklı hale getiren yardımcı bileşen
const ChangesTable: React.FC<{ changesJson: string }> = ({ changesJson }) => {
  try {
    const changes = JSON.parse(changesJson);
    // Eğer changes boş bir nesne değilse tabloyu oluştur
    if (Object.keys(changes).length === 0) return <p>Detaylı değişiklik kaydı bulunmuyor.</p>;

    return (
      <Table striped bordered size="sm">
        <thead className="table-light">
          <tr>
            <th>Alan</th>
            <th>Eski Değer</th>
            <th>Yeni Değer</th>
          </tr>
        </thead>
        <tbody>
          {Object.entries(changes).map(([key, value]: [string, any]) => (
            <tr key={key}>
              <td><strong>{key}</strong></td>
              <td>{value.Old !== null && value.Old !== undefined ? value.Old.toString() : <em>Boş</em>}</td>
              <td>{value.New !== null && value.New !== undefined ? value.New.toString() : <em>Boş</em>}</td>
            </tr>
          ))}
        </tbody>
      </Table>
    );
  } catch (error) {
    return <p>Değişiklik detayı bulunmuyor veya format hatalı.</p>;
  }
};

export const LogDetailModal: React.FC<LogDetailModalProps> = ({ log, onHide }) => {
  if (!log) return null;

  return (
    <Modal show onHide={onHide} centered size="lg">
      <Modal.Header closeButton>
        <Modal.Title>Log Detayı: #{log.id}</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <h5>Genel Bilgiler</h5>
        <Table bordered>
          <tbody>
            <tr>
              <td><strong>Kullanıcı</strong></td>
              <td>{log.userName}</td>
            </tr>
            <tr>
              <td><strong>Eylem</strong></td>
              <td>{log.action}</td>
            </tr>
            <tr>
              <td><strong>Durum</strong></td>
              <td>
                <Badge bg={log.status === 'Başarılı' ? 'success' : 'danger'}>{log.status}</Badge>
              </td>
            </tr>
            <tr>
              <td><strong>Hedef</strong></td>
              <td>{log.targetEntity}{log.targetEntityId ? ` (#${log.targetEntityId})` : ''}</td>
            </tr>
            <tr>
              <td><strong>Tarih</strong></td>
              <td>{format(new Date(log.createdAt), 'dd.MM.yyyy HH:mm:ss')}</td>
            </tr>
          </tbody>
        </Table>

        <h5 className="mt-4">Değişiklik Detayları</h5>
        {log.changes ? <ChangesTable changesJson={log.changes} /> : <p>Detaylı değişiklik kaydı bulunmuyor.</p>}
        
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onHide}>Kapat</Button>
      </Modal.Footer>
    </Modal>
  );
};