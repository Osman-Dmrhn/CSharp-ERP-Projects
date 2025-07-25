import React, { useEffect, useState, useCallback } from 'react';
import { getAllOrders, createOrder, updateOrder, deleteOrder } from '../api/OrderService';
import { getAllProducts } from '../api/ProductService';
import type { Order } from '../models/OrderDtos/Order';
import type { CreateOrderDto } from '../models/OrderDtos/CreateOrderDto';
import type { OrderUpdateDto } from '../models/OrderDtos/OrderUpdate';
import type { ProductDto } from '../models/ProductDtos/ProductDto';
import type { OrderFilters } from '../models/OrderDtos/OrderFilters ';
import type { PaginationInfo } from '../models/LogDtos/PaginationInfo';
import { PaginationComponent } from '../components/Pagination';
import { Spinner, Alert, Table, Badge, Button, Modal, Form, Row, Col, Container } from 'react-bootstrap';

const emptyCreateOrder: CreateOrderDto = { productId: 0, customerName: '', quantity: 1 };

interface EditOrderModalProps {
  order: Order;
  productList: ProductDto[];
  onSave: (data: OrderUpdateDto) => void;
  onHide: () => void;
}

const EditOrderModal: React.FC<EditOrderModalProps> = ({ order, productList, onSave, onHide }) => {
  const [formData, setFormData] = useState<OrderUpdateDto>({
    productId: order.productId,
    customerName: order.customerName,
    quantity: order.quantity,
    status: order.status as OrderUpdateDto['status'], // Tip dönüşümü
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: name === 'quantity' || name === 'productId' ? Number(value) : value }));
  };

  const handleSubmit = () => onSave(formData);

  return (
    <Modal show onHide={onHide} centered>
      <Modal.Header closeButton><Modal.Title>Sipariş Güncelle</Modal.Title></Modal.Header>
      <Modal.Body>
        <Form.Group className="mb-3">
          <Form.Label>Ürün</Form.Label>
          <Form.Select name="productId" value={formData.productId} onChange={handleChange}>
            {productList.map(p => <option key={p.productId} value={p.productId}>{p.name}</option>)}
          </Form.Select>
        </Form.Group>
        {/* Diğer form alanları (customerName, quantity, status) */}
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onHide}>İptal</Button>
        <Button variant="primary" onClick={handleSubmit}>Kaydet</Button>
      </Modal.Footer>
    </Modal>
  );
};

// --- Ana Sayfa Bileşeni ---
const OrderPage: React.FC = () => {
  const [orders, setOrders] = useState<Order[]>([]);
  const [productList, setProductList] = useState<ProductDto[]>([]); // Ürün listesi için state
  const [pagination, setPagination] = useState<PaginationInfo | null>(null);
  const [filters, setFilters] = useState<OrderFilters>({ pageNumber: 1, pageSize: 10 });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const [newOrder, setNewOrder] = useState<CreateOrderDto>(emptyCreateOrder);
  const [editingOrder, setEditingOrder] = useState<Order | null>(null);

  // Ürün listesini bir kereye mahsus çek
  useEffect(() => {
    const fetchProducts = async () => {
      try {
        const result = await getAllProducts({ pageSize: 1000 }); // Tüm ürünleri çekmek için
        if (result.products) {
          setProductList(result.products);
          if (result.products.length > 0) {
            setNewOrder(prev => ({ ...prev, productId: result.products[0].productId }));
          }
        }
      } catch (err) {
        setError("Ürün listesi alınamadı.");
      }
    };
    fetchProducts();
  }, []);

  const fetchOrders = useCallback(async () => {
    setLoading(true);
    try {
      const result = await getAllOrders(filters);
      setOrders(result.orders || []);
      setPagination(result.pagination);
    } catch (err) {
      setError('Siparişler alınırken bir hata oluştu.');
      setOrders([]);
    } finally {
      setLoading(false);
    }
  }, [filters]);

  useEffect(() => {
    fetchOrders();
  }, [fetchOrders]);

  const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault();
    if(newOrder.productId === 0) {
        alert("Lütfen bir ürün seçin.");
        return;
    }
    const result = await createOrder(newOrder);
    if (result.success) {
      fetchOrders();
      setNewOrder({ ...emptyCreateOrder, productId: productList[0]?.productId || 0 });
    } else { setError(result.message); }
  };

  const handleUpdate = async (updateData: OrderUpdateDto) => {
    if (editingOrder) {
      const result = await updateOrder(editingOrder.orderId, updateData);
      if (result.success) {
        fetchOrders();
        setEditingOrder(null);
      } else { setError(result.message); }
    }
  };

  const handleDelete = async (id: number) => {
    if(window.confirm("Bu siparişi silmek istediğinizden emin misiniz?")) {
        const result = await deleteOrder(id);
        if (result.success) { fetchOrders(); }
        else { setError(result.message); }
    }
  };

  const handlePageChange = (page: number) => {
    setFilters(prev => ({ ...prev, pageNumber: page }));
  };

  return (
    <Container fluid className="p-4">
      <div className="bg-white rounded shadow p-4 mb-4">
        <h2 className="text-primary mb-4">Yeni Sipariş Oluştur</h2>
        <Form onSubmit={handleCreate}>
          <Row className="g-3">
            <Col md={4}>
              <Form.Label>Ürün</Form.Label>
              <Form.Select value={newOrder.productId} onChange={(e) => setNewOrder({ ...newOrder, productId: Number(e.target.value) })}>
                <option value={0} disabled>Seçiniz...</option>
                {productList.map(p => <option key={p.productId} value={p.productId}>{p.name}</option>)}
              </Form.Select>
            </Col>
            <Col md={4}>
              <Form.Label>Müşteri Adı</Form.Label>
              <Form.Control type="text" placeholder="Müşteri Adı" value={newOrder.customerName} onChange={(e) => setNewOrder({ ...newOrder, customerName: e.target.value })} required />
            </Col>
            <Col md={2}>
              <Form.Label>Miktar</Form.Label>
              <Form.Control type="number" placeholder="Miktar" value={newOrder.quantity} onChange={(e) => setNewOrder({ ...newOrder, quantity: Number(e.target.value) })} required />
            </Col>
            <Col md={2} className="align-self-end">
              <Button type="submit" className="w-100">Ekle</Button>
            </Col>
          </Row>
        </Form>
      </div>

      <div className="bg-white rounded shadow p-4">
        <h3 className="mb-3">Sipariş Listesi</h3>
        {loading ? <div className="text-center"><Spinner /></div> : error ? <Alert variant="danger">{error}</Alert> : (
          <>
            <Table striped bordered hover responsive>
              <thead className="table-dark">
                <tr>
                  <th>#ID</th><th>Müşteri Adı</th><th>Ürün Adı</th><th>Miktar</th><th>Durum</th><th>Tarih</th><th className="text-center">İşlemler</th>
                </tr>
              </thead>
              <tbody>
                {orders.map((order) => (
                  <tr key={order.orderId}>
                    <td>{order.orderId}</td>
                    <td>{order.customerName}</td>
                    <td>{order.productName}</td>
                    <td>{order.quantity}</td>
                    <td><Badge>{order.status}</Badge></td>
                    <td>{new Date(order.createdAt).toLocaleDateString('tr-TR')}</td>
                    <td className="text-center">
                      <Button variant="warning" size="sm" className="me-2" onClick={() => setEditingOrder(order)}>Düzenle</Button>
                      <Button variant="danger" size="sm" onClick={() => handleDelete(order.orderId)}>Sil</Button>
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

      {editingOrder && <EditOrderModal order={editingOrder} productList={productList} onSave={handleUpdate} onHide={() => setEditingOrder(null)} />}
    </Container>
  );
};

export default OrderPage;