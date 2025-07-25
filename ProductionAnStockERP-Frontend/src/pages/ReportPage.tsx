import React, { useState, useMemo, useEffect } from 'react';
import { Container, Button, Card, Form, Spinner, Row, Col, Alert } from 'react-bootstrap';
import { useAuth} from '../contexts/AuthContext';
import type { UserRole } from '../models/UserDtos/UserRole';
import * as ReportService from '../api/ReportService';
import { getAllProducts } from '../api/ProductService';
import type { ProductDto } from '../models/ProductDtos/ProductDto';

import type { OrderFilters } from '../models/OrderDtos/OrderFilters ';
import type { ProductFilters } from '../models/ProductDtos/ProductFilters';
import type { ProductionOrderFilters } from '../models/ProductionOrderDtos/ProductionOrderFilters';
import type { StockTransactionFilters } from '../models/StockTransactionDtos/StockTransactionFilters';
import type { LogFilters } from '../api/ActivityLogService';
import DatePicker from 'react-datepicker';
import "react-datepicker/dist/react-datepicker.css";

const allReportTypes = [
    { key: 'orders', label: 'Sipariş Raporu', roles: ['Admin', 'SalesManager'] as UserRole[] },
    { key: 'products', label: 'Ürün Raporu', roles: ['Admin', 'Producer'] as UserRole[] },
    { key: 'production-orders', label: 'Üretim Emri Raporu', roles: ['Admin', 'Producer'] as UserRole[] },
    { key: 'stock-transactions', label: 'Stok Hareket Raporu', roles: ['Admin', 'Producer'] as UserRole[] },
    { key: 'logs', label: 'Aktivite Log Raporu', roles: ['Admin'] as UserRole[] },
];

const ReportPage: React.FC = () => {
    const { user } = useAuth();
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState<string | null>(null);
    const [productList, setProductList] = useState<ProductDto[]>([]);
    
    const availableReports = useMemo(() => 
        allReportTypes.filter(rt => user?.role && rt.roles.includes(user.role as UserRole)),
    [user?.role]);
    
    const [selectedReport, setSelectedReport] = useState<string>('');
    const [filters, setFilters] = useState<any>({});

    useEffect(() => {
        if (availableReports.length > 0 && !selectedReport) {
            setSelectedReport(availableReports[0].key);
        }
    }, [availableReports, selectedReport]);

    useEffect(() => {
        const fetchFilterData = async () => {
            try {
                const productResult = await getAllProducts({ pageSize: 1000 });
                if (productResult.products) setProductList(productResult.products);
            } catch (err) {
                setError("Filtre verileri yüklenemedi.");
            }
        };
        fetchFilterData();
    }, []);

    // DÜZELTME: 'value' parametresi artık Date veya null olabilir.
    const handleFilterChange = (field: string, value: string | Date | null) => {
        setFilters((prev: any) => ({ ...prev, [field]: value }));
    }

    const handleGenerateReport = async () => {
        setLoading(true);
        setError(null);
        try {
            const apiFilters = { ...filters };
            if (apiFilters.startDate) apiFilters.startDate = new Date(apiFilters.startDate).toISOString();
            if (apiFilters.endDate) apiFilters.endDate = new Date(apiFilters.endDate).toISOString();

            switch (selectedReport) {
                case 'orders': await ReportService.generateOrdersReport(apiFilters as OrderFilters); break;
                case 'products': await ReportService.generateProductsReport(apiFilters as ProductFilters); break;
                case 'production-orders': await ReportService.generateProductionOrdersReport(apiFilters as ProductionOrderFilters); break;
                case 'stock-transactions': await ReportService.generateStockTransactionsReport(apiFilters as StockTransactionFilters); break;
                case 'logs': await ReportService.generateLogsReport(apiFilters as LogFilters); break;
                default: throw new Error("Geçersiz rapor tipi seçildi.");
            }
        } catch (err: any) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };
    
    const renderFilters = () => {
        switch (selectedReport) {
            case 'orders':
                return (
                    <Form.Group as={Row} className="mb-3"><Form.Label column sm={3}>Müşteri Adı</Form.Label><Col sm={9}><Form.Control type="text" onChange={e => handleFilterChange('customerName', e.target.value)} /></Col></Form.Group>
                );
            case 'products':
                 return (<Form.Group as={Row} className="mb-3"><Form.Label column sm={3}>Ürün Adı</Form.Label><Col sm={9}><Form.Control type="text" onChange={e => handleFilterChange('name', e.target.value)} /></Col></Form.Group>);
            case 'logs':
                return (
                    <>
                        <Form.Group as={Row} className="mb-2"><Form.Label column sm={3}>Başlangıç Tarihi</Form.Label><Col sm={9}>
                            <DatePicker selected={filters.startDate || null} 
                                // DÜZELTME: (date: Date | null)
                                onChange={(date: Date | null) => handleFilterChange('startDate', date)} 
                                className="form-control" dateFormat="dd/MM/yyyy" isClearable />
                        </Col></Form.Group>
                        <Form.Group as={Row} className="mb-2"><Form.Label column sm={3}>Bitiş Tarihi</Form.Label><Col sm={9}>
                            <DatePicker selected={filters.endDate || null} 
                                // DÜZELTME: (date: Date | null)
                                onChange={(date: Date | null) => handleFilterChange('endDate', date)} 
                                className="form-control" dateFormat="dd/MM/yyyy" isClearable />
                        </Col></Form.Group>
                    </>
                );
            default:
                return <p className="text-muted">Bu rapor için ek filtre bulunmamaktadır.</p>;
        }
    }

    return (
        <Container className="mt-4">
            <h2>Raporlama</h2>
            <p>İstediğiniz raporu oluşturmak için ilgili filtreleri seçip butona tıklayın.</p>
            <Card className="mt-4">
                <Card.Header as="h5">Rapor Oluştur</Card.Header>
                <Card.Body>
                    <Form.Group as={Row} className="mb-3">
                        <Form.Label column sm={3} className="fw-bold">Rapor Türü Seçin</Form.Label>
                        <Col sm={9}>
                           <Form.Select value={selectedReport} onChange={e => { setSelectedReport(e.target.value); setFilters({}); }}>
                                {availableReports.map(rt => <option key={rt.key} value={rt.key}>{rt.label}</option>)}
                           </Form.Select>
                        </Col>
                    </Form.Group>
                    <hr/>
                    <h5 className="mb-3">Filtreler</h5>
                    {renderFilters()}
                    <div className="d-grid mt-4">
                        <Button variant="primary" size="lg" disabled={loading} onClick={handleGenerateReport}>
                            {loading ? <><Spinner as="span" size="sm" /> Oluşturuluyor...</> : 'PDF Raporu Oluştur'}
                        </Button>
                    </div>
                    {error && <Alert variant="danger" className="mt-3">{error}</Alert>}
                </Card.Body>
            </Card>
        </Container>
    );
};

export default ReportPage;