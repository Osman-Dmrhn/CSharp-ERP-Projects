// Dosya: src/components/Pagination.tsx

import React from 'react';
import { Pagination as BootstrapPagination } from 'react-bootstrap';
import type { PaginationInfo } from '../models/LogDtos/PaginationInfo';

interface PaginationProps {
  pagination: PaginationInfo;
  onPageChange: (page: number) => void;
}

export const PaginationComponent: React.FC<PaginationProps> = ({ pagination, onPageChange }) => {
  if (!pagination || pagination.totalPages <= 1) {
    return null;
  }

  return (
    <BootstrapPagination>
      <BootstrapPagination.First onClick={() => onPageChange(1)} disabled={!pagination.hasPrevious} />
      <BootstrapPagination.Prev onClick={() => onPageChange(pagination.currentPage - 1)} disabled={!pagination.hasPrevious} />
      
      <BootstrapPagination.Item active>{pagination.currentPage}</BootstrapPagination.Item>
      
      <BootstrapPagination.Next onClick={() => onPageChange(pagination.currentPage + 1)} disabled={!pagination.hasNext} />
      <BootstrapPagination.Last onClick={() => onPageChange(pagination.totalPages)} disabled={!pagination.hasNext} />
    </BootstrapPagination>
  );
};