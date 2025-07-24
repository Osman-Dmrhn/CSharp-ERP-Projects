// Dosya: src/components/LogFilters.tsx

import React, { useState } from 'react';
import { Form, Row, Col, Button } from 'react-bootstrap';
import DatePicker from 'react-datepicker';
import "react-datepicker/dist/react-datepicker.css";
import type { LogFilters } from '../api/ActivityLogService';

interface LogFiltersProps {
  onFilter: (filters: Partial<LogFilters>) => void;
}

export const LogFiltersComponent: React.FC<LogFiltersProps> = ({ onFilter }) => {
  const [startDate, setStartDate] = useState<Date | null>(null);
  const [endDate, setEndDate] = useState<Date | null>(null);
  const [userName, setUserName] = useState('');

  const handleFilter = () => {
    onFilter({
      startDate: startDate ? startDate.toISOString() : undefined,
      endDate: endDate ? endDate.toISOString() : undefined,
      userName: userName || undefined,
    });
  };

  const clearFilters = () => {
      setStartDate(null);
      setEndDate(null);
      setUserName('');
      onFilter({ startDate: undefined, endDate: undefined, userName: undefined });
  }

  return (
    <div className="p-3 mb-4 bg-light rounded-3">
      <Row className="g-3 align-items-end">
        <Col md={3}>
          <Form.Group controlId="userName">
            <Form.Label>Kullanıcı Adı</Form.Label>
            <Form.Control type="text" value={userName} onChange={e => setUserName(e.target.value)} />
          </Form.Group>
        </Col>
        <Col md={3}>
          <Form.Group controlId="startDate">
            <Form.Label>Başlangıç Tarihi</Form.Label>
            <DatePicker selected={startDate} onChange={(date) => setStartDate(date)} className="form-control" dateFormat="dd/MM/yyyy" />
          </Form.Group>
        </Col>
        <Col md={3}>
          <Form.Group controlId="endDate">
            <Form.Label>Bitiş Tarihi</Form.Label>
            <DatePicker selected={endDate} onChange={(date) => setEndDate(date)} className="form-control" dateFormat="dd/MM/yyyy" />
          </Form.Group>
        </Col>
        <Col md={3} className="d-flex">
            <Button variant="primary" onClick={handleFilter} className="me-2">Filtrele</Button>
            <Button variant="secondary" onClick={clearFilters}>Temizle</Button>
        </Col>
      </Row>
    </div>
  );
};