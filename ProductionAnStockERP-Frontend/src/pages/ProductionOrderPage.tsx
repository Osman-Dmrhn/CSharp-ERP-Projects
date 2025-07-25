// Dosya: src/pages/ProductionOrderPage.tsx

import React, { useEffect, useState, useCallback } from 'react';
import { getAllProductionOrders, createProductionOrder, updateProductionOrder, deleteProductionOrder } from '../api/productionOrderService';
import { getAllProducts } from '../api/ProductService';
import { getAllOrders } from '../api/OrderService';
import type { ProductionOrder} from '../models/ProductionOrderDtos/ProductionOrder';
import type { ProductionOrderCreateDto } from '../models/ProductionOrderDtos/ProductionOrderCreateDto';
import type { ProductionOrderUpdateDto } from '../models/ProductionOrderDtos/ProductionOrderUpdateDto';
import type { ProductionOrderFilters } from '../models/ProductionOrderDtos/ProductionOrderFilters';
import type { ProductDto } from '../models/ProductDtos/ProductDto';
import type { Order } from '../models/OrderDtos/Order';
import type { PaginationInfo } from '../models/LogDtos/PaginationInfo';
import { PaginationComponent } from '../components/Pagination';
import { Spinner, Alert, Table, Badge, Button, Modal, Form, Row, Col, Container } from 'react-bootstrap';
import type { ProductionOrderStatus } from '../models/ProductionOrderDtos/ProductionOrderStatus';

const emptyCreateOrder: ProductionOrderCreateDto = { productId: 0, quantity: 1 };

// --- Düzenleme Modalı Bileşeni ---
interface EditModalProps {
  productionOrder: ProductionOrder;
  productList: ProductDto[];
  onSave: (data: ProductionOrderUpdateDto) => void;
  onHide: () => void;
}

const EditProductionOrderModal: React.FC<EditModalProps> = ({ productionOrder, productList, onSave, onHide }) => {
  const [formData, setFormData] = useState<ProductionOrderUpdateDto>({
    productId: productionOrder.productId,
    quantity: productionOrder.quantity,
    status: productionOrder.status,
  });

  // DÜZELTME: Event tipi react-bootstrap ile uyumlu hale getirildi.
  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: name === 'quantity' || name === 'productId' ? Number(value) : value }));
  };

  const handleSubmit = () => onSave(formData);

  return (
    <Modal show onHide={onHide} centered>
      <Modal.Header closeButton><Modal.Title>Üretim Emrini Güncelle</Modal.Title></Modal.Header>
      <Modal.Body>
        <Form.Group className="mb-3">
          <Form.Label>Ürün</Form.Label>
          <Form.Select name="productId" value={formData.productId} onChange={handleChange}>
            {productList.map(p => <option key={p.productId} value={p.productId}>{p.name}</option>)}
          </Form.Select>
        </Form.Group>
        <Form.Group className="mb-3">
            <Form.Label>Miktar</Form.Label>
            <Form.Control type="number" name="quantity" value={formData.quantity} onChange={handleChange} />
        </Form.Group>
        <Form.Group className="mb-3">
          <Form.Label>Durum</Form.Label>
          <Form.Select name="status" value={formData.status} onChange={handleChange}>
            <option value="Started">Başladı</option>
            <option value="InProgress">Devam Ediyor</option>
            <option value="Completed">Tamamlandı</option>
          </Form.Select>
        </Form.Group>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onHide}>İptal</Button>
        <Button variant="primary" onClick={handleSubmit}>Kaydet</Button>
      </Modal.Footer>
    </Modal>
  );
};


// --- Ana Sayfa Bileşeni ---
const ProductionOrderPage: React.FC = () => {
  const [productionOrders, setProductionOrders] = useState<ProductionOrder[]>([]);
  const [productList, setProductList] = useState<ProductDto[]>([]);
  const [orderList, setOrderList] = useState<Order[]>([]);
  const [pagination, setPagination] = useState<PaginationInfo | null>(null);
  const [filters, setFilters] = useState<ProductionOrderFilters>({ pageNumber: 1, pageSize: 15 });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const [newProductionOrder, setNewProductionOrder] = useState<ProductionOrderCreateDto>(emptyCreateOrder);
  const [editingProductionOrder, setEditingProductionOrder] = useState<ProductionOrder | null>(null);

  useEffect(() => {
    const fetchDropdownData = async () => {
      try {
        const productResult = await getAllProducts({ pageSize: 1000 });
        if (productResult.products) setProductList(productResult.products);

        const orderResult = await getAllOrders({ pageSize: 1000 });
        if (orderResult.orders) setOrderList(orderResult.orders);
        
        if (productResult.products?.length > 0) {
          setNewProductionOrder(prev => ({ ...prev, productId: productResult.products[0].productId }));
        }

      } catch (err) { setError("Form verileri yüklenemedi."); }
    };
    fetchDropdownData();
  }, []);

  const fetchProductionOrders = useCallback(async () => {
    setLoading(true);
    try {
      const result = await getAllProductionOrders(filters);
      setProductionOrders(result.productionOrders || []);
      setPagination(result.pagination);
    } catch (err) {
      setError('Üretim emirleri alınırken bir hata oluştu.');
    } finally {
      setLoading(false);
    }
  }, [filters]);

  useEffect(() => {
    fetchProductionOrders();
  }, [fetchProductionOrders]);

  const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault();
    if(newProductionOrder.productId === 0) { alert("Lütfen bir ürün seçin."); return; }
    const result = await createProductionOrder(newProductionOrder);
    if (result.success) {
      fetchProductionOrders();
      setNewProductionOrder({ ...emptyCreateOrder, productId: productList[0]?.productId || 0 });
    } else { setError(result.message); }
  };

  const handleUpdate = async (updateData: ProductionOrderUpdateDto) => {
    if (editingProductionOrder) {
      const result = await updateProductionOrder(editingProductionOrder.productionId, updateData);
      if (result.success) {
        fetchProductionOrders();
        setEditingProductionOrder(null);
      } else { setError(result.message); }
    }
  };

  const handleDelete = async (id: number) => {
    if(window.confirm("Bu üretim emrini silmek istediğinizden emin misiniz?")) {
        const result = await deleteProductionOrder(id);
        if (result.success) { fetchProductionOrders(); }
        else { setError(result.message); }
    }
  };

  const handlePageChange = (page: number) => {
    setFilters(prev => ({ ...prev, pageNumber: page }));
  };
  
  const getStatusBadge = (status: ProductionOrderStatus) => {
    switch(status) {
        case 'Started': return 'secondary';
        case 'InProgress': return 'primary';
        case 'Completed': return 'success';
        default: return 'light';
    }
  }

  return (
    <Container fluid className="p-4">
      <div className="bg-white rounded shadow p-4 mb-4">
        <h2 className="text-primary mb-4">Yeni Üretim Emri</h2>
        <Form onSubmit={handleCreate}>
          <Row className="g-3">
            <Col md={4}>
              <Form.Label>Ürün</Form.Label>
              <Form.Select value={newProductionOrder.productId} onChange={(e) => setNewProductionOrder({ ...newProductionOrder, productId: Number(e.target.value) })}>
                <option value={0} disabled>Seçiniz...</option>
                {productList.map(p => <option key={p.productId} value={p.productId}>{p.name}</option>)}
              </Form.Select>
            </Col>
            <Col md={3}>
              <Form.Label>İlişkili Sipariş (İsteğe Bağlı)</Form.Label>
              <Form.Select value={newProductionOrder.orderId || ''} onChange={(e) => setNewProductionOrder({ ...newProductionOrder, orderId: Number(e.target.value) || undefined })}>
                <option value="">Yok</option>
                {orderList.map(o => <option key={o.orderId} value={o.orderId}>#{o.orderId} - {o.customerName}</option>)}
              </Form.Select>
            </Col>
            <Col md={2}>
              <Form.Label>Miktar</Form.Label>
              <Form.Control type="number" placeholder="Miktar" value={newProductionOrder.quantity} onChange={(e) => setNewProductionOrder({ ...newProductionOrder, quantity: Number(e.target.value) })} required />
            </Col>
            <Col md={3} className="align-self-end">
              <Button type="submit" className="w-100">Üretim Emri Oluştur</Button>
            </Col>
          </Row>
        </Form>
      </div>

      <div className="bg-white rounded shadow p-4">
        <h3 className="mb-3">Üretim Emirleri Listesi</h3>
        {loading ? <div className="text-center"><Spinner /></div> : error ? <Alert variant="danger">{error}</Alert> : (
          <>
            <Table striped bordered hover responsive>
              <thead className="table-dark">
                <tr>
                  <th>#ID</th><th>Ürün Adı</th><th>Miktar</th><th>Durum</th><th>Oluşturan</th><th>Tarih</th><th className="text-center">İşlemler</th>
                </tr>
              </thead>
              <tbody>
                {productionOrders.map((order) => (
                  <tr key={order.productionId}>
                    <td>{order.productionId}</td>
                    <td>{order.productName}</td>
                    <td>{order.quantity}</td>
                    <td><Badge bg={getStatusBadge(order.status)}>{order.status}</Badge></td>
                    <td>{order.createdByUserName}</td>
                    <td>{new Date(order.createdAt).toLocaleString('tr-TR')}</td>
                    <td className="text-center">
                      <Button variant="warning" size="sm" className="me-2" onClick={() => setEditingProductionOrder(order)}>Düzenle</Button>
                      <Button variant="danger" size="sm" onClick={() => handleDelete(order.productionId)}>Sil</Button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </Table>
            <div className="d-flex justify-content-end">
              {pagination && <PaginationComponent pagination={pagination} onPageChange={handlePageChange} />}
            </div>
          </>
        )}
      </div>

      {editingProductionOrder && <EditProductionOrderModal productionOrder={editingProductionOrder} productList={productList} onSave={handleUpdate} onHide={() => setEditingProductionOrder(null)} />}
    </Container>
  );
};

export default ProductionOrderPage;